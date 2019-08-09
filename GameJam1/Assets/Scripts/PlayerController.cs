using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int playerNum = 1;
    public float moveDir = 0;

    private IWeapon[] inventory;

    private int inventoryIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponentsInChildren<IWeapon>();
        SetupInventory();
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = (playerNum == 1 ? Input.GetAxisRaw("Horizontal1") : Input.GetAxisRaw("Horizontal2"));
        bool switchPressed = (playerNum == 1 ? Input.GetButtonDown("Switch1") : Input.GetButtonDown("Switch2"));
        if(switchPressed)
        {
            inventory[inventoryIndex].EnableWeapon(false);
            inventory[inventoryIndex].DisposeWeapon();
            inventoryIndex = (inventoryIndex + 1 >= inventory.Length) ? 0 : inventoryIndex + 1;
            inventory[inventoryIndex].EnableWeapon(true);
        }
    }

    private void SetupInventory()
    {
        foreach (var wep in inventory)
        {
            wep.EnableWeapon(false);
        }
        if (inventory.Length > 0)
        {
            inventory[0].EnableWeapon(true);
        }
    }

}
