using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    public int playerNum = 1;
    public int acceleration = 300;
    public float drag = 0.985f;

    private float moveDir = 0;
    private float maxVelocity = 10;
    private Rigidbody2D rb;
    private Vector2 targetedVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = (playerNum == 1 ? Input.GetAxisRaw("Horizontal1") : Input.GetAxisRaw("Horizontal2"));
        
    }

    void FixedUpdate()
    {
        if(rb.velocity.x * moveDir > maxVelocity){
            targetedVelocity.x = maxVelocity * moveDir;
            targetedVelocity.y = rb.velocity.y;
            rb.velocity = targetedVelocity;
        } else {
            targetedVelocity.x = moveDir * acceleration;
            targetedVelocity.y = rb.velocity.y;
            rb.AddForce(targetedVelocity);
        }
        if(moveDir == 0){
            targetedVelocity.x = rb.velocity.x * drag;
            targetedVelocity.y = rb.velocity.y;
            rb.velocity = targetedVelocity;
        }

        // Debug.Log(rb.velocity.x);
        // Debug.Log(Time.fixedDeltaTime);
    }
}
