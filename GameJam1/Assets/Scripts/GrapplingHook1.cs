using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook1 : MonoBehaviour
{
    public int playerNum = 1;
    public float aimX;
    public float aimY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // aimX = (playerNum == 1 ? Input.GetAxisRaw("AimX") : Input.GetAxisRaw("Horizontal2"));
        // aimY = (playerNum == 1 ? Input.GetAxisRaw("AimY") : Input.GetAxisRaw("Horizontal2"));   
    }

    void FixedUpdate() {
        // Debug.Log(aimX);
        // Debug.Log(aimY);
    }
}
