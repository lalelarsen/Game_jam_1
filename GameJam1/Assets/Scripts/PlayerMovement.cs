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
    public float maxJumpTime = 0.5f;
    public float currentJumpTime;
    private bool movementDisabled = false;
    public bool pushed = false;

    private PlayerController playerCon;
    private GrapplingHook grapplingHook;
    public Rigidbody2D rb;
    public Vector2 targetedVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        grapplingHook = GetComponent<GrapplingHook>();
        playerCon = GetComponentInParent<PlayerController>();
        playerNum = playerCon.playerNum;
        moveDir = playerCon.moveDir;
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = playerCon.moveDir;
        jumpPressed = (playerNum == 1 ? Input.GetButton("Jump1") : Input.GetButton("Jump2"));
    }

    void FixedUpdate()
    {
        currentJumpTime -= Time.fixedDeltaTime;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadiusCheck, whatIsGround);

        preventInfinateJump();

        if (pushed){
            rb.AddForce(new Vector2(targetedVelocity.x, targetedVelocity.y));
            pushed = false;
        }
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
            targetedVelocity.x = rb.velocity.x;
            targetedVelocity.y = 18;
            rb.AddForce(targetedVelocity);
        } else
        {
            // falling
        }

    }

    private void preventInfinateJump()
    {
        var colls = Physics2D.OverlapCircleAll(groundCheck.position, groundRadiusCheck, whatIsGround);
        foreach (Collider2D col in colls)
        {
            if (col.tag == "Player")
            {
                var pm = col.GetComponent<PlayerMovement>();
                if (pm.playerNum == this.playerNum && colls.Length <= 1)
                {
                    isGrounded = false;
                }
            }
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
        grapplingHook.EnableHook(enable);
    }

    public void enableHorizontalMovement(bool enable)
    {
        movementDisabled = !enable;
    }

}