using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public int acceleration = 300;
    public float drag = 0.87f;
    public bool isGrounded = false;

    public LayerMask whatIsGround;
    public Transform groundCheck;

    private int playerNum;
    private float moveDir = 0;
    private bool jumpPressed = false;
    private int jumpForce = 5;
    private float maxVelocity = 10;
    private float groundRadiusCheck = .1f;
    private float maxJumpTime = 0.5f;
    private float currentJumpTime;
    private bool movementDisabled = false;

    private GrapplingHook grapplingHook;
    private Rigidbody2D rb;
    private Vector2 targetedVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        grapplingHook = GetComponent<GrapplingHook>();
        playerNum = GetComponentInParent<PlayerController>().playerNum;
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = (playerNum == 1 ? Input.GetAxisRaw("Horizontal1") : Input.GetAxisRaw("Horizontal2"));
        jumpPressed = (playerNum == 1 ? Input.GetButton("Jump1") : Input.GetButton("Jump2"));
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadiusCheck, whatIsGround);
        if (!movementDisabled)
        {
            if (rb.velocity.x * moveDir > maxVelocity)
            {
                targetedVelocity.x = maxVelocity * moveDir;
                targetedVelocity.y = rb.velocity.y;
                rb.velocity = targetedVelocity;
            }
            else
            {
                targetedVelocity.x = moveDir * acceleration;
                targetedVelocity.y = 0;
                rb.AddForce(targetedVelocity);
            }

            if (moveDir == 0)
            {
                targetedVelocity.x = rb.velocity.x * drag;
                targetedVelocity.y = rb.velocity.y;
                rb.velocity = targetedVelocity;
            }
        }

        if (isGrounded && jumpPressed && !movementDisabled)
        {
            targetedVelocity.x = rb.velocity.x;
            targetedVelocity.y = 12;
            rb.velocity = targetedVelocity;
            currentJumpTime = maxJumpTime;
        }
        else if (!isGrounded && jumpPressed && currentJumpTime > 0 && !movementDisabled)
        {
            currentJumpTime -= Time.fixedDeltaTime;
            targetedVelocity.x = rb.velocity.x;
            targetedVelocity.y = 18;
            rb.AddForce(targetedVelocity);
        } else
        {
            // falling
        }

    }

    public void enableDrag(bool enable)
    {
        drag = (enable) ? 0.87f : 1;
    }

    public void enableMovement(bool enable){
        rb.mass = enable ? 1 : 0;
        rb.angularDrag = enable ? 0.05f : 0;
        rb.gravityScale = enable ? 3 : 0;
        enableHorizontalMovement(enable);
        grapplingHook.enableHook(enable);
    }

    public void enableHorizontalMovement(bool enable)
    {
        movementDisabled = !enable;
    }

}