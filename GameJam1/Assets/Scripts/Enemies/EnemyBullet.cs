using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    public int travelSpeed = 8;

    private readonly int lifeTime = 5;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyBullet", lifeTime);
    }

    void FixedUpdate()
    {
        transform.Translate(Vector2.right * travelSpeed * Time.fixedDeltaTime, Space.Self);
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
