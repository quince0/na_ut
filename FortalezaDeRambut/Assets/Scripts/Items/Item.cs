using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null; 


    public virtual void Use()
    {
        // Something will happen depending of kind of item
        Debug.Log("Using " + name);
    }

    public void RemoveFromInventory()
    {
        Inventory.inventoryInstance.Remove(this);
    }
}