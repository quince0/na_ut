using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton

    public static Inventory inventoryInstance;

    private void Awake()
    {
        if (inventoryInstance != null)
        {
            Debug.LogWarning("You can't create more than one inventory instance!");
            return;
        }
        inventoryInstance = this;
    }

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public List<Item> items = new List<Item>();
    public int inventorySpace = 20;

    public bool Add(Item item)
    {
        if (items.Count >= inventorySpace)
        {
            Debug.Log("Your inventory is full.");
            return false;
        }

        items.Add(item);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }

        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

}