using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomController : MonoBehaviour
{
    GameObject cam;
    public enum Phases {Phase1, Phase2Trans, Phase2, Phase3Trans, Phase3};
    public Phases phase = Phases.Phase1;
    private bool onGoingAction = false;
    private bool goToNextPhase = false;
    void Start()
    {
        cam = gameObject.transform.Find("Boss cam").gameObject;
    }

    void Update()
    {
        
        switch(phase){
            case Phases.Phase1:
                if(!onGoingAction){
                    onGoingAction = true;
                    int attackMove = Mathf.RoundToInt(Random.Range(0.5f, 4.5f));
                    switch(attackMove){
                        case 1:
                            hitFromRightSide();
                        break;

                        case 2:
                            shootFromRightSide();
                        
                        break;
                        
                        case 3:
                            hitFromLeftSide();
                        
                        break;
                        
                        case 4:
                            shootFromLeftSide();
                        
                        break;
                    }

                    //do SomeMethod that ends with "onGoingAction = false" and "...OnComplete( if(goToNextPhase) -> change phase))
                }
            break;

            case Phases.Phase2Trans:

            break;

            case Phases.Phase2:

            break;

            case Phases.Phase3Trans:

            break;

            case Phases.Phase3:

            break;
        }
    }

    public void startBossFight(){
        cam.SetActive(true);
    }

    #region phase1BossMoves
    public void hitFromRightSide(){

    }
    public void shootFromRightSide(){

    }
    public void hitFromLeftSide(){

    }
    public void shootFromLeftSide(){

    }
    #endregion
    
}
