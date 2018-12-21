using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractable : Interactable
{
    public Item item;

    // This method return true if it was posible to interact with the item
    public override bool Interact(Transform playerTransform)
    {
        bool possibleInteract = base.Interact(playerTransform);
        if (possibleInteract)
        {
            PickUp();
        }

        return true;
    }

    void PickUp()
    {
        Debug.Log("Picking up " + item.name);
        bool enoughtSpace = Inventory.inventoryInstance.Add(item);
        if (enoughtSpace)
        {
            Destroy(gameObject);
        }
    }
}
