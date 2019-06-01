using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 5;
    public bool interact = false;

    private int playerNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerNum = GetComponentInParent<PlayerController>().playerNum;
    }

    // Update is called once per frame
    void Update()
    {
        interact = (playerNum == 1 ? Input.GetButtonDown("Interact1") : Input.GetButtonDown("Interact2"));
        if(interact){
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactionRange);
            foreach (Collider2D collider in colliders)
            {
                var p = collider.gameObject.GetComponent<IInteractable>();
                if(p != null){
                    p.interact(gameObject);
                }
            }
        }
    }

    private void FixedUpdate() {

    }
}
