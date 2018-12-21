using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
    #region Singleton

    public static Player playerInstance;

    void Start()
    {
        playerInstance = this;
        EquipmentManager.equipmentManagerInstance.onEquipmentChanged += OnEquipmentChanged;
    }

    #endregion

    void OnEquipmentChanged(Equipment newEquipment, Equipment oldEquipment)
    {
        if (oldEquipment != null)
        {
            removeAttackModifier(oldEquipment.attackModifier);
            removeDefenseModifier(oldEquipment.defenseModifier);
        }

        if (newEquipment != null)
        {
            addAttackModifier(newEquipment.attackModifier);
            addDefenseModifier(newEquipment.defenseModifier);
        }
    }

    public void InteractWithInteractable(Interactable interactable, Transform playerTransform)
    {
        interactable.Interact(playerTransform);
    }

    public void KillPlayer()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
