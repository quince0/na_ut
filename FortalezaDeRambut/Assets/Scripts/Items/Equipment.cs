using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipSlot;
    
    public uint attackModifier;
    public uint defenseModifier;

    public override void Use()
    {
        base.Use();
        EquipmentManager.equipmentManagerInstance.Equip(this);
        RemoveFromInventory();
    }

}

public enum EquipmentSlot { Weapon, Shield, Head, Chest, Legs, Feet }