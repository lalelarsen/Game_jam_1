using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossFloor : MonoBehaviour
{
    private Vector3 upPosition;
    private Vector3 downPosition;
    private Vector3 shakePosition;

    public Vector3 shakeStrength = new Vector3 (0,0.5f,0);
    public bool up = true;
    public bool fallable = true;
    void Start()
    {
        upPosition = gameObject.transform.position;
        downPosition = new Vector2(gameObject.transform.position.x,gameObject.transform.position.y-3);
    }

    // Update is called once per frame
    void Update()
    {
        if(up){
            transform.DOMove(upPosition,2);
            fallable = true;
        } else {
            if(fallable){
                transform.DOShakePosition(1, shakeStrength, 0, 180, false, false).OnComplete(()=>{
                    fallable = false;
                    transform.DOMove(downPosition,4).SetEase(Ease.Linear).OnComplete(()=>{
                    });
                });
            }
        }
    }

    public void collapse(){
        up = false;
    }

}
