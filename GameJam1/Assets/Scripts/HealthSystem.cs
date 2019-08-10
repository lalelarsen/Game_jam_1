using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int health = 100;
    public bool main = true;
    // Max checks for a parent
    public int parts = 5;
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
        Debug.Log("Hit enemy");
        if(!main){
            HealthSystem mainPart = getMainPart(gameObject, 2);
            mainPart.TakeDamage(damage);
        } else {
            health -= damage;
            //spawn dmg particals
            Debug.Log(health);
            if(health <= 0)
            {
                Debug.Log("Dead");
                Destroy(gameObject);
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

}
