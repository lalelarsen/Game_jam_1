﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour, IWeapon
{

    public GameObject gunGfx;
    public GameObject playerPos;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPos;

    private PlayerController playerCon;
    private int playerNum = 0;
    private float shootCDMax = .2f;
    private float shootCDActual = 0f;
    private Vector3 moveDir = Vector3.right;

    private Vector3 aimVec;
    private Vector3 gunVec;
    private Vector3 indicatorVec;

    // Start is called before the first frame update
    void Start()
    {
        playerCon = GetComponentInParent<PlayerController>();
        playerNum = playerCon.playerNum;
        gunVec = moveDir;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerCon.moveDir < 0)
        {
            moveDir = Vector3.down;
            gunVec = moveDir;
        } else if (playerCon.moveDir > 0)
        {
            moveDir = Vector3.up;
        }

        var aimY = (playerNum == 1) ? Input.GetAxisRaw("AimX1") : Input.GetAxisRaw("AimX2");
        var aimX = (playerNum == 1) ? Input.GetAxisRaw("AimY1") : Input.GetAxisRaw("AimY2");
        aimVec.x = aimX * 1;
        aimVec.y = aimY * 1;

        var fireGun = (playerNum == 1) ? Input.GetAxisRaw("Fire1") : Input.GetAxisRaw("Fire2");

        UpdateGunAim();

        if(shootCDActual < 0)
        {
            if (fireGun > .2f)
            {
                // fire bullet in aim dir
                FireBullet();
            } 
        }
        else
        {
            shootCDActual -= Time.deltaTime;
        }

    }

    private void FireBullet()
    {
        // find bullet rotation
        var tempAim = gunVec;
        Debug.Log(tempAim.ToString());
        //if (Mathf.Abs(aimVec.x) < 0.1 && Mathf.Abs(aimVec.y) < 0.1)
        //{
        //    tempAim.x = 0;
        //    tempAim.y = 1;
        //}
        float rot_z = Mathf.Atan2(tempAim.y, tempAim.x) * Mathf.Rad2Deg;

        // instantite bullet
        Instantiate(bulletPrefab, bulletSpawnPos.position, Quaternion.Euler(0f, 0f, rot_z - 90));

        // set cooldown
        shootCDActual = shootCDMax;
    }

    private void UpdateGunAim()
    {
        transform.position = playerPos.transform.position;

        if (aimVec == Vector3.zero)
        {
            gunVec = moveDir;

        }
        else
        {
            if(Mathf.Abs(Vector3.Angle(Vector3.left+Vector3.down, aimVec)) < 22)
            {
                //Debug.Log("left up");
                gunVec.x = 1;
                gunVec.y = -1;
            }
            else if (Mathf.Abs(Vector3.Angle(Vector3.left + Vector3.up, aimVec)) < 22)
            {
                //Debug.Log("right up");
                gunVec.x = 1;
                gunVec.y = 1;
            }
            else if (Mathf.Abs(Vector3.Angle(Vector3.left, aimVec)) <= 22)
            {
                //Debug.Log("up");
                gunVec.x = 1;
                gunVec.y = 0;
            }
            else if (Mathf.Abs(Vector3.Angle(Vector3.down, aimVec)) <= 22)
            {
                //Debug.Log("left");
                gunVec.x = 0;
                gunVec.y = -1;
            }
            else if (Mathf.Abs(Vector3.Angle(Vector3.up, aimVec)) <= 22)
            {
                //Debug.Log("right");
                gunVec.x = 0;
                gunVec.y = 1;
            }
            else
            {
                gunVec = moveDir;
            }
        }

        //if(aimVec != Vector3.zero)
        //{
        //    gunVec.x = aimVec.x * -1;
        //    gunVec.y = aimVec.y * 1;
        //}

        // calculate gun gfx rotation
        float heading = Mathf.Atan2(gunVec.x, gunVec.y);
        transform.rotation = Quaternion.Euler(0f, 0f, heading * Mathf.Rad2Deg);
        transform.Rotate(gunVec.x, gunVec.y, gunVec.z, Space.World);
    }

    public void EnableWeapon(bool shouldEnable)
    {
        enabled = shouldEnable;
        gunGfx.SetActive(shouldEnable);
    }

    public void DisposeWeapon()
    {
        // Nothing to dispose
    }
}
