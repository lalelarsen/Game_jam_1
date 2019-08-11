using System.Collections;
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
    private Vector3 moveDir;

    private Vector3 aimVec;
    private Vector3 gunVec;
    private Vector3 indicatorVec;

    // Start is called before the first frame update
    void Start()
    {
        playerCon = GetComponentInParent<PlayerController>();
        playerNum = playerCon.playerNum;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerCon.moveDir < -0.1)
        {
            moveDir = Vector3.left;
        } else if (playerCon.moveDir > 0.1)
        {
            moveDir = Vector3.right;
        }

        var aimY = (playerNum == 1) ? Input.GetAxisRaw("AimY1") : Input.GetAxisRaw("AimY2");
        var aimX = (playerNum == 1) ? Input.GetAxisRaw("AimX1") : Input.GetAxisRaw("AimX2");
        aimVec.x = aimX * 1;
        aimVec.y = aimY * -1;
        //Debug.Log("aimRaw: " + aimX + "," + aimY);
        //Debug.Log("aimVec: " + aimVec);

        var fireGunTrigger = (playerNum == 1) ? Input.GetAxisRaw("Fire1") : Input.GetAxisRaw("Fire2");

        UpdateGunAim();

        if(shootCDActual < 0)
        {
            if (fireGunTrigger > .2f)
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

        float rot_z = Mathf.Atan2(tempAim.y, tempAim.x) * Mathf.Rad2Deg;

        // instantite bullet
        Instantiate(bulletPrefab, bulletSpawnPos.position, Quaternion.Euler(0f, 0f, rot_z));

        // set cooldown
        shootCDActual = shootCDMax;
    }

    private void UpdateGunAim()
    {
        transform.position = playerPos.transform.position;

        //Debug.Log("UpdateGunGFXAim");

        if (aimVec == Vector3.zero)
        {
            //Debug.Log("Aim is zero, use moveDir: " + moveDir);
            gunVec = moveDir;

        }
        else
        {
            if (Vector3.Angle(Vector3.left+Vector3.up, aimVec) < 22.5 ||
                Vector3.Angle(aimVec, Vector3.left + Vector3.up) < 22.5)
            {
                //Debug.Log("left up");
                gunVec.x = -1;
                gunVec.y = 1;
            }
            else if (Vector3.Angle(Vector3.right + Vector3.up, aimVec) < 22.5 || 
                Vector3.Angle(aimVec, Vector3.right + Vector3.up ) < 22.5)
            {
                //Debug.Log("right up");
                gunVec.x = 1;
                gunVec.y = 1;
            }
            else if (Vector3.Angle(Vector3.up, aimVec) <= 22.5 ||
                Vector3.Angle(aimVec, Vector3.up) <= 22.5)
            {
                //Debug.Log("up");
                gunVec.x = 0;
                gunVec.y = 1;
            }
            else if (Vector3.Angle(Vector3.left, aimVec) <= 22.5 ||
                Vector3.Angle(aimVec, Vector3.left) <= 22.5)
            {
                //Debug.Log("left");
                gunVec.x = -1;
                gunVec.y = 0;
            }
            else if (Vector3.Angle(Vector3.right, aimVec) <= 22.5 ||
                Vector3.Angle(aimVec, Vector3.right) <= 22.5)
            {
                //Debug.Log("right");
                gunVec.x = 1;
                gunVec.y = 0;
            }
            else
            {
                gunVec = moveDir;
                //Debug.Log("aim is useless, use moveDir: " + moveDir);
            }
            //Debug.Log("Aim is not zero, use aimVec: " + gunVec);
        }

        // calculate gun gfx rotation
        //Debug.Log("gunvec: " + gunVec);
        float heading = Mathf.Atan2(gunVec.x, gunVec.y);
        //Debug.Log("aggle: " + heading * Mathf.Rad2Deg * -1);
        transform.rotation = Quaternion.Euler(0f, 0f, heading * Mathf.Rad2Deg * -1);
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
