using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    int players;
    BossRoomController controller;
    GameObject gate;
    int timeBeforeFall = 3;
    float timePassed = 0;
    private void Start() {
        controller = gameObject.GetComponentInParent<BossRoomController>();
        gate = gameObject.transform.Find("Gate").gameObject;
    }

    private void Update() {
        if(players == 2){
            timePassed += Time.fixedDeltaTime;
        }
        if(timePassed > timeBeforeFall){
            gate.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.transform.parent.tag == "Player"){
            players++;
        }
        if(players == 2){
            controller.startBossFight();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.transform.parent.tag == "Player"){
            players--;
        }
    }
}
