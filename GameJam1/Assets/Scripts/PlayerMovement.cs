using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int playerNum = 1;
    public int speed = 10;

    private float moveDir = 0;

    private Rigidbody2D rb;

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
        Vector2 movement = new Vector2(moveDir * speed, rb.velocity.y);

        rb.AddForce(movement);
    }
}
