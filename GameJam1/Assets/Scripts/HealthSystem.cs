using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int health = 100;
    public bool main = true;
    // Max checks for a parent
    public int parentChecks = 5;
    public int[] milestones;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {   
        if(!main){
            HealthSystem mainPart = getMainPart(gameObject, parentChecks);
            mainPart.TakeDamage(damage);
        } else {
            health -= damage;
            //spawn dmg particals
            if(health <= 0)
            {
                Destroy(gameObject);
            } else if(onMilestone(health)){
                string[] parameters = new string[0];
                BroadcastMessage("hitMilestone", parameters);
            }
        }
    }

    public HealthSystem getMainPart(GameObject gObject, int amountOfChecks){
        if(amountOfChecks == 0){
            throw new MissingComponentException();
        }
        Transform parent = gObject.transform.parent;
        HealthSystem comp = parent.gameObject.GetComponent<HealthSystem>();
        if(comp == null){
            comp = getMainPart(parent.gameObject, amountOfChecks-1);
        }
        return comp;
    }

    public bool onMilestone(int health){
        foreach (int i in milestones)
        {
            if(health == i){
                return true;
            }
        }
        return false;
    }
}
