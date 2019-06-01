using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int travelSpeed = 150;
    public int damage = 10;

    private int liveTime = 5;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyBullet", liveTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector2.right * travelSpeed * Time.fixedDeltaTime, Space.Self);
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<HealthSystem>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

}
