using UnityEngine;

public class Character : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth { get; private set; }

    public uint attack = 0;
    public uint defense = 0;

    void Awake ()
    {
        currentHealth = maxHealth;
    }

    void Update ()
    {
        // This is for testing
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
    }

    public void addAttackModifier(uint attackModifier)
    {
        attack += attackModifier;
    }

    public void removeAttackModifier(uint attackModifier)
    {
        if (attack >= attackModifier)
        {
            attack -= attackModifier;
        }
        else
        {
            attack = 0;
        }
    }

    public void addDefenseModifier(uint defenseModifier)
    {
        defense += defenseModifier;
    }

    public void removeDefenseModifier(uint defenseModifier)
    {
        if (defense >= defenseModifier)
        {
            defense -= defenseModifier;
        } else
        {
            defense = 0;
        }
    }

    public void TakeDamage(float damage)
    {
        damage /= (defense + 1);
        damage = Mathf.Round(damage);

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Debug.Log(transform.name + " died.");
    }

}
