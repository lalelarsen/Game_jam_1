using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3;
    public PlayerMovement MoveScript;

    private bool forceCD = false;
    private float forceCDTime = 0.5f;
    private float forceCurrTime = 0f;
    private bool dmgCD = false;
    private float dmgCDTime = 3f;
    private float dmgCurrTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        MoveScript = GetComponentInChildren<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(dmgCD){
            dmgCurrTime -= Time.deltaTime;
            if(dmgCurrTime < 0){
                dmgCD = false;
            }
        }
        if(forceCD){
            forceCurrTime -= Time.deltaTime;
            if(forceCurrTime < 0){
                forceCD = false;
                MoveScript.pushed = false;
            }
        }
    }

    public void takePlayerDamage(bool applyForce){
        if(!dmgCD){
            health--;
            dmgCurrTime = dmgCDTime;
            dmgCD = true;
        }
        if(applyForce){
            if(!forceCD){
                // MoveScript.currentJumpTime = MoveScript.maxJumpTime;
                MoveScript.rb.velocity = new Vector2(MoveScript.rb.velocity.x, 0);
                MoveScript.targetedVelocity.y = 1200;
                MoveScript.pushed = true;
                forceCurrTime = forceCDTime;
                forceCD = true;
            }
        }
        if(health == 0){
            //Ded
        }
    }
}
