﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour, IWeapon
{
    private int playerNum = 0;
    private readonly float indicatorRange = 2.5f;
    private Vector3 aimVec;
    private Vector3 indicatorVec;

    private Vector3 hookVec;
    private readonly int hookSpeed = 30;
    private readonly int hookMaxTravelTime = 1;
    private float hookCurrentTravelTime = 0;
    private bool hookIsTraveling = false;
    private bool hookHitEnvironment = false;
    private bool hookInUse = false;
    private bool hookIsExpired = true;

    public GameObject aimIndicator;
    public GameObject grapplingHook;
    public LayerMask hookAble;
    public LineRenderer line;

    private bool disableHook;
    private DistanceJoint2D joint;
    private Rigidbody2D rb;
    private PlayerMovement pm;

    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<DistanceJoint2D>();
        playerNum = GetComponentInParent<PlayerController>().playerNum;
        rb = GetComponent<Rigidbody2D>();
        pm = GetComponent<PlayerMovement>();
        disableHook = false;
    }

    // Update is called once per frame
    void Update()
    {

        var aimX = (playerNum == 1) ? Input.GetAxisRaw("AimX1") : Input.GetAxisRaw("AimX2");
        var aimY = (playerNum == 1) ? Input.GetAxisRaw("AimY1") : Input.GetAxisRaw("AimY2");
        aimVec.x = aimX;
        aimVec.y = aimY * -1;

        if(!disableHook){
            indicatorVec = transform.position + aimVec * indicatorRange;
            aimIndicator.transform.position =  indicatorVec;
        }
        
        var fireGrapplingHook = (playerNum == 1) ? Input.GetAxisRaw("Fire1") : Input.GetAxisRaw("Fire2");

        // is GH triggered
        if (fireGrapplingHook > .2f && !disableHook)
        {
            // Debug.Log(isNewTrigger.ToString());
            // is it initial shot
            if (!hookIsTraveling && !hookInUse && IsAimingForSomthing() && hookIsExpired)
            {
                // initial shot
                hookVec = aimVec.normalized;
                hookIsTraveling = true;
                hookInUse = true;
                hookIsExpired = false;
                grapplingHook.transform.position = transform.position;
            }
            // did GH hit anything
            else if(hookHitEnvironment)
            {
                // swinging
                pm.enableDrag(false);
                pm.enableHorizontalMovement(false);
                line.SetPosition(0, transform.position);
            }
            // is GH traveling in air
            else if (hookIsTraveling)
            {

                // did hook exceed max travel time
                if (hookCurrentTravelTime > hookMaxTravelTime)
                {
                    ResetHook();
                } else
                {
                    hookCurrentTravelTime += Time.deltaTime;
                }

                // hook is traveling
                var collidersHit = Physics2D.OverlapCircleAll(grapplingHook.transform.position, .2f, hookAble);
                var hookHitDetection = IsValidHit(collidersHit);

                grapplingHook.transform.position += hookVec * hookSpeed * Time.deltaTime;

                line.SetPosition(0, transform.position);
                line.SetPosition(1, grapplingHook.transform.position);
                line.enabled = true;

                if (hookHitDetection)
                {
                    // latch to environment
                    hookHitEnvironment = true;
                    hookIsTraveling = false;
                    joint.connectedAnchor = grapplingHook.transform.position;
                    var distOffset = (pm.isGrounded) ? -1 : 0;
                    joint.distance = Vector2.Distance(grapplingHook.transform.position, transform.position) + distOffset;
                    joint.enabled = true;
                }

            }
            // shouldn't GH fire at all
            else
            {
                grapplingHook.transform.position = transform.position;
            }
        } else if(hookInUse)
        {
            // reset hook
            ResetHook();
            hookIsExpired = true;
        }
        else
        {
            // while hook not in use, follow player
            grapplingHook.transform.position = transform.position;
            hookIsExpired = true;
        }
    }

    private void OnEnable()
    {
        grapplingHook.transform.position = transform.position;
    }

    private bool IsAimingForSomthing()
    {
        return aimVec != Vector3.zero;
    }

    private void ResetHook()
    {
        hookInUse = false;
        hookIsTraveling = false;
        joint.enabled = false;
        line.enabled = false;
        hookHitEnvironment = false;
        hookIsExpired = true;
        pm.enableDrag(true);
        pm.enableHorizontalMovement(true);
        hookCurrentTravelTime = 0;
        grapplingHook.transform.position = transform.position;
    }

    private bool IsValidHit(Collider2D[] cols)
    {
        if (cols.Length <= 0) return false;
 
        foreach (var col in cols)
        {
            var p = col.gameObject.GetComponentInParent<PlayerController>();
            // make sure you dont hit a player
            if (p != null && p.playerNum == playerNum)
            {
                //Debug.Log("hit this");
                return false;
            }
        }
        return true;
    }

    public void EnableHook(bool enable){
        disableHook = !enable;
    }

    public void EnableWeapon(bool shouldEnable)
    {
        enabled = shouldEnable;
        grapplingHook.SetActive(shouldEnable);
    }

    public void DisposeWeapon()
    {
        ResetHook();
        aimIndicator.transform.position = transform.position;
    }

}
