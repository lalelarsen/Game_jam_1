using System;
using UnityEngine;

public class SpaceShip : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update

    Rigidbody2D rb;
    public int playerNum;
    public float power;
    public bool coreEngine1;
    public bool coreEngine2;
    private Vector2 forceR;
    private Vector2 forceL;
    private Vector3 tempPos;
    private Vector3 rightEnginePos;
    private Vector3 leftEnginePos;
    private Vector3 rightSeatPos;
    private Vector3 leftSeatPos;
    public GameObject playerInRightSeat;
    public GameObject playerInLeftSeat;
    private float fireR;
    private float fireL;
    private Collider2D col;
    
    
    void Start()
    {
        power = 30;
        coreEngine1 = false;
        coreEngine2 = false;
        rightEnginePos = new Vector3(0.45f,0,0);
        leftEnginePos = new Vector3(-0.45f,0,0);
        rightSeatPos = new Vector3(0.3f,0.4f,0.1f);
        leftSeatPos = new Vector3(-0.3f,0.4f,0.1f);
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump1")){
            if(playerInRightSeat != null && playerInLeftSeat != null){
                coreEngine1 = !coreEngine1;
            } else if(playerInRightSeat != null || playerInLeftSeat != null){
                if(!coreEngine1){
                    coreEngine1 = !coreEngine1;
                } else {
                    coreEngine1 = false;
                    coreEngine2 = false;
                }
            }
        }
        if(Input.GetButtonDown("Jump2")){
            if(playerInRightSeat != null && playerInLeftSeat != null){
                coreEngine2 = !coreEngine2;
            } else if(playerInRightSeat != null || playerInLeftSeat != null){
                if(!coreEngine1){
                    coreEngine1 = !coreEngine1;
                } else {
                    coreEngine1 = false;
                    coreEngine2 = false;
                }
            }
        }
    }
    private void FixedUpdate() {
        float fireR = 0;
        float fireL = 0;
        if(playerInRightSeat != null && playerInLeftSeat != null){
            string rightSeatName = playerInRightSeat.transform.parent.name.Substring(playerInRightSeat.transform.parent.name.Length-1,1);
            string leftSeatName = playerInLeftSeat.transform.parent.name.Substring(playerInLeftSeat.transform.parent.name.Length-1,1);
            fireR = Input.GetAxisRaw("Fire" + rightSeatName);
            fireL = Input.GetAxisRaw("Fire" + leftSeatName);
        } else if(playerInRightSeat != null || playerInLeftSeat != null){
            fireR = Input.GetAxisRaw("Fire1") + Input.GetAxisRaw("Fire2");
            fireL = Input.GetAxisRaw("FireL1") + Input.GetAxisRaw("FireL2");
        }
        
        forceR.x = rb.transform.up.x*(power/10*fireR);
        forceR.y = rb.transform.up.y*(power/10*fireR);
        forceL.x = rb.transform.up.x*(power/10*fireL);
        forceL.y = rb.transform.up.y*(power/10*fireL);
        
        rb.AddForceAtPosition(forceR,rb.transform.TransformPoint(rightEnginePos));
        rb.AddForceAtPosition(forceL,rb.transform.TransformPoint(leftEnginePos));
        float forceMultiplier = 0;
        if(coreEngine1 && coreEngine2){
            forceMultiplier = 1;
        } else if (coreEngine1 || coreEngine2){
            forceMultiplier = 0.95f;
        }
        rb.AddForce(rb.transform.up*(power*forceMultiplier));
        
        if(playerInRightSeat != null){
            playerInRightSeat.transform.position = gameObject.transform.TransformPoint(rightSeatPos);
            playerInRightSeat.transform.rotation = gameObject.transform.rotation;
        }
        if(playerInLeftSeat != null){
            playerInLeftSeat.transform.position = gameObject.transform.TransformPoint(leftSeatPos);
            playerInLeftSeat.transform.rotation = gameObject.transform.rotation;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player"){
            Physics2D.IgnoreCollision(other.collider,col);
        }
    }
    public void interact(GameObject interacter)
    {
        if(playerInRightSeat == interacter){
            interacter.GetComponent<PlayerMovement1>().enableMovement(true);
            playerInRightSeat = null;
            if(playerInLeftSeat == null){
                coreEngine1 = false;
                coreEngine2 = false;
            }
        } else if(playerInLeftSeat == interacter){
            interacter.GetComponent<PlayerMovement1>().enableMovement(true);
            playerInLeftSeat = null;
            if(playerInRightSeat == null){
                coreEngine1 = false;
                coreEngine2 = false;
            }
        } else {
            interacter.GetComponent<PlayerMovement1>().enableMovement(false);
            if(playerInRightSeat == null){
                playerInRightSeat = interacter;

            } else {
                playerInLeftSeat = interacter;
            }
        }
    }
}
