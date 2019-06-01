using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public int playerNum = 1;

    private float indicatorRange = 2.5f;
    private Vector3 aimVec;
    private Vector3 indicatorVec;

    public GameObject aimIndicator;

    private DistanceJoint2D joint;

    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<DistanceJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {

        var aimX = (playerNum == 1) ? Input.GetAxisRaw("AimX1") : Input.GetAxisRaw("AimX2");
        var aimY = (playerNum == 1) ? Input.GetAxisRaw("AimY1") : Input.GetAxisRaw("AimY2");
        aimVec.x = aimX;
        aimVec.y = aimY * -1;

        indicatorVec = transform.position + aimVec * indicatorRange;
        aimIndicator.transform.position =  indicatorVec;
        

    }
}
