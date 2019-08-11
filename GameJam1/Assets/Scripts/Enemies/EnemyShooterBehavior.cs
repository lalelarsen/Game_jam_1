using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooterBehavior : MonoBehaviour
{
    public GameObject bulletPrefab;
    public LayerMask layerMask;

    private readonly float detectionRadius = 100f;
    private readonly float shootCD = 2;
    private float lastShot = 0;
    private Vector2 aim;

    // Update is called once per frame
    void Update()
    {
        if (lastShot > shootCD)
        {
            shootPlayerIfInRange(Physics2D.OverlapCircleAll(transform.position, detectionRadius, layerMask));
        }
        else
        {
            lastShot += Time.deltaTime;
        }

    }

    private void shootPlayerIfInRange(Collider2D[] colliders)
    {
        if(colliders.Length > 0)
        {
            shootAtPlayer(colliders[0].transform.position);
        }
    }

    private void shootAtPlayer(Vector3 playerPos)
    {
        lastShot = 0;
        // find bullet vector
        Vector3 bulletTrejectory = playerPos - transform.position;

        // find bullet rotation
        //if (Mathf.Abs(aimVec.x) < 0.1 && Mathf.Abs(aimVec.y) < 0.1)
        //{
        //    tempAim.x = 0;
        //    tempAim.y = 1;
        //}
        float rot_z = Mathf.Atan2(bulletTrejectory.y, bulletTrejectory.x) * Mathf.Rad2Deg;

        // instantite bullet
        Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, rot_z));

        // init bullet
    }

}
