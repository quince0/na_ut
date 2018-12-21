using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The current problem we have is the NPC cant have equipment <-- this will be fixed in the next version, only if we want :)
public class EquipmentManager : MonoBehaviour
{
    #region Singleton

    KeyCode UnequipAllKey = KeyCode.U;

    public static EquipmentManager equipmentManagerInstance;

    public void Awake()
    {
        equipmentManagerInstance = this;
    }

    #endregion

    Equipment[] currentEquipment;
    Inventory inventory;

    public delegate void OnEquipmentChanged(Equipment newEquipment, Equipment oldEquipment);
    public OnEquipmentChanged onEquipmentChanged;

    void Start()
    {
        inventory = Inventory.inventoryInstance;

        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
    }

    void Update()
    {
        if (Input.GetKeyDown(UnequipAllKey))
        {
            UnequipAll();
        }
    }

    public void Equip(Equipment newEquipment)
    {
        int slotIndex = (int)newEquipment.equipSlot;

        Equipment oldEquipment = null;

        // If the current slot has something we want to swap the inventory item with the equipment item
        if (currentEquipment[slotIndex] != null)
        {
            oldEquipment = currentEquipment[slotIndex];
            inventory.Add(oldEquipment);
        }

        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newEquipment, oldEquipment);
        }

        currentEquipment[slotIndex] = newEquipment;
    }

    public void Unequip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            Equipment oldEquipment = currentEquipment[slotIndex];
            inventory.Add(oldEquipment);

            currentEquipment[slotIndex] = null;

            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldEquipment);
            }

        }
    }

    public void UnequipAll()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i);
        }
    }

}