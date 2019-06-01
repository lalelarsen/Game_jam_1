using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomController : MonoBehaviour
{
    GameObject cam;
    void Start()
    {
        cam = gameObject.transform.Find("Boss cam").gameObject;
    }

    
    void Update()
    {
        
    }

    public void startBossFight(){
        cam.SetActive(true);
    }
}
