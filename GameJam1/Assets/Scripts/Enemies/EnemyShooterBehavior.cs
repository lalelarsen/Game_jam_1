using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooterBehavior : MonoBehaviour
{
    public GameObject bulletPrefab;
    
    private readonly float shootCD = 2;
    private float lastShot = 0;
    private Vector2 aim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        lastShot += Time.deltaTime;
        //Debug.Log(lastShot);

    }

    private void shootAtPlayer(Vector3 playerPos)
    {

        Debug.Log("shoot");
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

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("found player");
            if(lastShot > shootCD)
            {
                shootAtPlayer(other.transform.position);
            }
        }
    }

}
