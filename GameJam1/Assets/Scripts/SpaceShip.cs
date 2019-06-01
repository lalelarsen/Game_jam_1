﻿using System;
using UnityEngine;

public class SpaceShip : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update

    Rigidbody2D rb;
    public int playerNum;
    public float power;
    public bool coreEngine1;
    public bool coreEngine2;
    private Vector2 forceR;
    private Vector2 forceL;
    private Vector3 tempPos;
    private Vector3 rightEnginePos;
    private Vector3 leftEnginePos;
    private Vector3 rightSeatPos;
    private Vector3 leftSeatPos;
    private GameObject playerInRightSeat;
    private GameObject playerInLeftSeat;
    private float fireR;
    private float fireL;
    
    
    void Start()
    {
        power = 30;
        coreEngine1 = false;
        coreEngine2 = false;
        rightEnginePos = new Vector3(0.45f,0,0);
        leftEnginePos = new Vector3(-0.45f,0,0);
        rightSeatPos = new Vector3(0.3f,0.2f,-0.1f);
        leftSeatPos = new Vector3(-0.3f,0.2f,-0.1f);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump1")){
            coreEngine1 = !coreEngine1;
        }
        if(Input.GetButtonDown("Jump2")){
            coreEngine2 = !coreEngine2;
        }
    }
    private void FixedUpdate() {
        var fireR = Input.GetAxisRaw("Fire1") + Input.GetAxisRaw("Fire2");
        var fireL = Input.GetAxisRaw("FireL1") + Input.GetAxisRaw("FireL2");
        
        forceR.x = rb.transform.up.x*(power/10*fireR);
        forceR.y = rb.transform.up.y*(power/10*fireR);
        forceL.x = rb.transform.up.x*(power/10*fireL);
        forceL.y = rb.transform.up.y*(power/10*fireL);
        
        rb.AddForceAtPosition(forceR,rb.transform.TransformPoint(rightEnginePos));
        rb.AddForceAtPosition(forceL,rb.transform.TransformPoint(leftEnginePos));
        float forceMultiplier = 0;
        if(coreEngine1 && coreEngine2){
            forceMultiplier = 1;
        } else if (coreEngine1 || coreEngine2){
            forceMultiplier = 0.95f;
        }
        rb.AddForce(rb.transform.up*(power*forceMultiplier));
        
    }

    public void interact(GameObject gameObject)
    {
        if(playerInRightSeat == null){
            playerInRightSeat = gameObject;
        } else {
            playerInLeftSeat = gameObject;
        }
    }
}
