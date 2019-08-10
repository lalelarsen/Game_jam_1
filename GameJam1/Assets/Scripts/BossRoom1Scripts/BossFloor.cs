using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossFloor : MonoBehaviour
{
    public enum Placements { Left, Mid, Right };
    public Placements placement;
    private Vector3 upPosition;
    private Vector3 downPosition;
    private Vector3 shakePosition;

    private Vector3 shakeStrength;
    public bool up = true;
    public bool moving = false;
    void Start()
    {
        shakeStrength = new Vector3(0, 0.1f, 0);
        upPosition = gameObject.transform.position;
        downPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 3);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void floorMove(string[] parameters)
    {
        if (parameters[0].Contains(this.placement.ToString()) && !moving)
        {
            moving = true;
            switch (parameters[1])
            {
                case "Down":
                    transform.DOShakePosition(2, shakeStrength, 10, 180, false, false).OnComplete(() =>
                    {
                        transform.DOMove(downPosition, 2.5f).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            moving = false;
                        });
                    });
                    break;
                case "Up":
                    transform.DOMove(upPosition, 1.5f).SetEase(Ease.Linear).OnComplete(() =>
                       {
                           moving = false;
                       });
                    break;
            }
        }
    }

}
