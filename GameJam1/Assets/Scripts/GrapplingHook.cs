using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public int playerNum = 1;

    private int indicatorRange = 25;
    private Vector3 aim;

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
        aim.x = aimX * indicatorRange;
        aim.y = aimY * indicatorRange * -1;
        Debug.Log($"aimx: {aimX}, aimy: {aimY}");

        aimIndicator.transform.position = transform.position + aim;
        

    }
}
