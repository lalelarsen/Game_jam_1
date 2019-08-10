using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRollerBehavior : MonoBehaviour
{

    public int moveSpeed = 8;
    public int jumpForce = 12;

    private int moveDir = 1;
    
    private readonly float jumpCD = 2f;
    private float lastJump = 0;

    private Vector2 moveVec;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        moveVec = new Vector2(moveDir * moveSpeed, rb.velocity.y);
    }
    
    void FixedUpdate()
    {
        moveVec.x = moveDir * moveSpeed;

        if(lastJump > jumpCD)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            lastJump = 0;
        } else
        {
            lastJump += Time.fixedDeltaTime;
        }

        moveVec.y = rb.velocity.y;
        rb.velocity = moveVec;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            moveDir *= -1;
        }
    }
    
}
