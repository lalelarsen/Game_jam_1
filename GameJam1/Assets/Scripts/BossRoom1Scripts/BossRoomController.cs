using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossRoomController : MonoBehaviour
{
    GameObject cam;
    public enum Phases { NotStarted, Phase1Trans, Phase1, Phase2Trans, Phase2, Phase3Trans, Phase3 };
    public Phases phase = Phases.NotStarted;
    private bool onGoingAction = false;
    private bool goToNextPhase = false;
    private int attackMove;

    public GameObject hookBall;
    public GameObject leftArm;
    public Animator animL;
    private Vector2 leftArmStartPos;
    public GameObject rightArm;
    public Animator animR;
    public GameObject timerObject;
    private Vector2 rightArmStartPos;
    private Vector2 tempArmPos;
    private Vector2 tempLeftArmPos;
    private Vector2 tempRightArmPos;

    private bool pickNewAttackMove = true;
    private float timeToWait;
    private bool timeStarted = false;
    private bool waitPlatform = false;
    public bool sidesUp = true;

    void Start()
    {
        cam = gameObject.transform.Find("Boss cam").gameObject;
        leftArmStartPos = leftArm.transform.position;
        rightArmStartPos = rightArm.transform.position;
    }


    void Update()
    {

        switch (phase)
        {
            case Phases.Phase1Trans:
                introduceBoss();
                break;

            case Phases.Phase1:
                if (attackMove == 0 && pickNewAttackMove)
                {
                    attackMove = Mathf.RoundToInt(Random.Range(0.5f, 4.5f));
                    // attackMove = 4;
                }
                switch (attackMove)
                {
                    case 1:
                        moveFromRightSide();
                        break;

                    case 2:
                        moveFromLeftSide();
                        break;

                    case 3:
                        hitFromRightSide();
                        break;

                    case 4:
                        hitFromLeftSide();
                        break;
                }
                //do SomeMethod that ends with "onGoingAction = false" and "...OnComplete( if(goToNextPhase) -> change phase))

                break;

            case Phases.Phase2Trans:
                Phase2Trans();
                break;

            case Phases.Phase2:
                if (attackMove == 0 && pickNewAttackMove)
                {
                    attackMove = Mathf.RoundToInt(Random.Range(0.5f, 4.5f));
                    // attackMove = 4;
                }
                switch (attackMove)
                {
                    case 1:
                        moveFromRightSide();
                        break;

                    case 2:
                        moveFromLeftSide();
                        break;

                    case 3:
                        hitFromRightSide();
                        break;

                    case 4:
                        hitFromLeftSide();
                        break;
                }
                break;

            case Phases.Phase3Trans:
                phase = Phases.Phase3;
                break;

            case Phases.Phase3:
                // Move floor
                if (!waitPlatform)
                {
                    waitPlatform = true;
                    if (sidesUp)
                    {
                        sidesUp = !sidesUp;
                        string[] parameters = new string[2];
                        parameters[0] = "Right,Left";
                        parameters[1] = "Down";
                        BroadcastMessage("floorMove", parameters);

                        parameters[0] = "Mid";
                        parameters[1] = "Up";
                        BroadcastMessage("floorMove", parameters);

                        timerObject.transform.DOMove(timerObject.transform.position, 10).OnComplete(() =>
                        {
                            waitPlatform = false;
                        });
                    }
                    else
                    {
                        sidesUp = !sidesUp;
                        string[] parameters = new string[2];
                        parameters[0] = "Right,Left";
                        parameters[1] = "Up";
                        BroadcastMessage("floorMove", parameters);

                        parameters[0] = "Mid";
                        parameters[1] = "Down";
                        BroadcastMessage("floorMove", parameters);

                        timerObject.transform.DOMove(timerObject.transform.position, 10).OnComplete(() =>
                        {
                            waitPlatform = false;
                        });
                    }
                }

                if (attackMove == 0 && pickNewAttackMove)
                {
                    attackMove = Mathf.RoundToInt(Random.Range(0.5f, 2.5f));
                    // attackMove = 4;
                }
                switch (attackMove)
                {
                    case 1:
                        hitFromSides();
                        break;

                    case 2:
                        moveFromSides();
                        break;

                }
                break;
        }
    }

    public void startBossFight()
    {
        cam.SetActive(true);
        phase = Phases.Phase1Trans;
    }

    private bool waitSeconds(float seconds)
    {
        if (!timeStarted)
        {
            timeToWait = seconds;
            timeStarted = true;
        }
        if (timeToWait < 0)
        {
            return true;
        }
        else
        {
            timeToWait -= Time.deltaTime;
            return false;
        }

    }

    private void doMoveWaitSeconds(float seconds)
    {
        timerObject.transform.DOMove(timerObject.transform.position, seconds);
    }

    private void resetTimer()
    {
        timeStarted = false;
    }

    public void hitMilestone(string[] parameters){
        switch(phase){
            case Phases.Phase1:
                phase = Phases.Phase2Trans;
            break;
            case Phases.Phase2:
                phase = Phases.Phase3Trans;
            break;
        }
    }

    #region phase1TransMoves
    public void introduceBoss()
    {
        if (waitSeconds(3))
        {
            tempArmPos.x = rightArmStartPos.x;
            tempArmPos.y = rightArmStartPos.y + 22;
            rightArm.transform.DOMove(tempArmPos, 4);

            tempArmPos.x = leftArmStartPos.x;
            tempArmPos.y = leftArmStartPos.y + 22;
            leftArm.transform.DOMove(tempArmPos, 4).OnComplete(() =>
            {
                resetTimer();
                // Maybe stop

                rightArm.transform.DOMove(rightArmStartPos, 4);
                leftArm.transform.DOMove(leftArmStartPos, 4).OnComplete(() =>
                {
                    phase = Phases.Phase1;
                });
            });

        }



    }
    #endregion

    #region phase1BossMoves
    public void hitFromRightSide()
    {
        pickNewAttackMove = false;
        tempArmPos.x = rightArmStartPos.x;
        tempArmPos.y = rightArmStartPos.y + 22;

        Sequence moveSequence = DOTween.Sequence();
        moveSequence.Append(rightArm.transform.DOMove(tempArmPos, 2));
        moveSequence.AppendCallback(() =>
        {
            animR.SetTrigger("rightSlam");
        });
        moveSequence.Append(rightArm.transform.DOMove(tempArmPos, 2));
        tempArmPos.y = tempArmPos.y - 22;
        moveSequence.Append(rightArm.transform.DOMove(tempArmPos, 2));
        moveSequence.AppendCallback(() =>
        {
            rightArm.transform.position = rightArmStartPos;
            pickNewAttackMove = true;
        });
        attackMove = 0;
    }

    public void moveFromRightSide()
    {
        pickNewAttackMove = false;
        tempArmPos.x = rightArmStartPos.x;
        tempArmPos.y = rightArmStartPos.y + 8;

        Sequence moveSequence = DOTween.Sequence();
        moveSequence.Append(rightArm.transform.DOMove(tempArmPos, 2));
        tempArmPos.x = leftArmStartPos.x;
        moveSequence.Append(rightArm.transform.DOMove(tempArmPos, 6));
        tempArmPos.y = leftArmStartPos.y;
        moveSequence.Append(rightArm.transform.DOMove(tempArmPos, 2));
        moveSequence.AppendCallback(() =>
        {
            rightArm.transform.position = rightArmStartPos;
            pickNewAttackMove = true;
        });
        attackMove = 0;
    }
    public void hitFromLeftSide()
    {
        pickNewAttackMove = false;
        tempArmPos.x = leftArmStartPos.x;
        tempArmPos.y = leftArmStartPos.y + 22;

        Sequence moveSequence = DOTween.Sequence();
        moveSequence.Append(leftArm.transform.DOMove(tempArmPos, 2));
        moveSequence.AppendCallback(() =>
        {
            animL.SetTrigger("leftSlam");
        });
        moveSequence.Append(leftArm.transform.DOMove(tempArmPos, 2));
        tempArmPos.y = tempArmPos.y - 22;
        moveSequence.Append(leftArm.transform.DOMove(tempArmPos, 2));
        moveSequence.AppendCallback(() =>
        {
            leftArm.transform.position = leftArmStartPos;
            pickNewAttackMove = true;
        });
        attackMove = 0;
    }
    public void moveFromLeftSide()
    {
        pickNewAttackMove = false;
        tempArmPos.x = leftArmStartPos.x;
        tempArmPos.y = leftArmStartPos.y + 8;

        Sequence moveSequence = DOTween.Sequence();
        moveSequence.Append(leftArm.transform.DOMove(tempArmPos, 2));
        tempArmPos.x = rightArmStartPos.x;
        moveSequence.Append(leftArm.transform.DOMove(tempArmPos, 6));
        tempArmPos.y = rightArmStartPos.y;
        moveSequence.Append(leftArm.transform.DOMove(tempArmPos, 2));
        moveSequence.AppendCallback(() =>
        {
            leftArm.transform.position = leftArmStartPos;
            pickNewAttackMove = true;
        });
        attackMove = 0;
    }

    #endregion

    #region phase2TransMoves
    private void Phase2Trans()
    {
        if(!hookBall.activeSelf){
            hookBall.SetActive(true);
            hookBall.transform.DOMove(new Vector2(hookBall.transform.position.x, hookBall.transform.position.y-15), 4);
        }
        string[] parameters = new string[2];
        parameters[0] = "Mid";
        parameters[1] = "Down";

        BroadcastMessage("floorMove", parameters);
        timerObject.transform.DOMove(timerObject.transform.position, 4).OnComplete(() =>
        {
            attackMove = 0;
            phase = Phases.Phase2;
        });
    }

    #endregion

    #region phase3Bossmoves
    public void hitFromSides()
    {
        pickNewAttackMove = false;

        // MOVE RIGHT ARM
        tempRightArmPos.x = rightArmStartPos.x;
        tempRightArmPos.y = rightArmStartPos.y + 22;
        rightArm.transform.DOMove(tempRightArmPos, 2).OnComplete(() =>
        {
            animR.SetTrigger("rightSlam");
            rightArm.transform.DOMove(tempRightArmPos, 2).OnComplete(() =>
            {
                tempRightArmPos.y = tempRightArmPos.y - 22;
                rightArm.transform.DOMove(tempRightArmPos, 2).OnComplete(() =>
                {
                    rightArm.transform.position = rightArmStartPos;
                    pickNewAttackMove = true;
                });
            });
        });

        // MOVE LEFT ARM
        tempLeftArmPos.x = leftArmStartPos.x;
        tempLeftArmPos.y = leftArmStartPos.y + 22;
        leftArm.transform.DOMove(tempLeftArmPos, 2).OnComplete(() =>
        {
            animL.SetTrigger("leftSlam");
            leftArm.transform.DOMove(tempLeftArmPos, 2).OnComplete(() =>
            {
                tempLeftArmPos.y = tempLeftArmPos.y - 22;
                leftArm.transform.DOMove(tempLeftArmPos, 2).OnComplete(() =>
                {
                    leftArm.transform.position = leftArmStartPos;
                });
            });
        });

        attackMove = 0;
    }

    public void moveFromSides()
    {
        pickNewAttackMove = false;
        tempRightArmPos.x = rightArmStartPos.x;
        tempRightArmPos.y = rightArmStartPos.y + 8;

        rightArm.transform.DOMove(tempRightArmPos, 2).OnComplete(() =>
        {
            tempRightArmPos.x = leftArmStartPos.x;
            rightArm.transform.DOMove(tempRightArmPos, 6).OnComplete(() =>
            {
                tempRightArmPos.y = leftArmStartPos.y;
                rightArm.transform.DOMove(tempRightArmPos, 2).OnComplete(() =>
                {
                    rightArm.transform.position = rightArmStartPos;
                    pickNewAttackMove = true;
                });
            });
        });

        tempLeftArmPos.x = leftArmStartPos.x;
        tempLeftArmPos.y = leftArmStartPos.y + 8;

        leftArm.transform.DOMove(tempLeftArmPos, 2).OnComplete(() =>
        {
            tempLeftArmPos.x = rightArmStartPos.x;
            leftArm.transform.DOMove(tempLeftArmPos, 6).OnComplete(() =>
            {
                tempLeftArmPos.y = rightArmStartPos.y;
                leftArm.transform.DOMove(tempLeftArmPos, 2).OnComplete(() =>
                {
                    leftArm.transform.position = leftArmStartPos;

                });
            });
        });

        attackMove = 0;
    }

    #endregion
}
