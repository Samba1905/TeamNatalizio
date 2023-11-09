using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 10;
    private int currentHealth;

    public int HealtsPoint 
    { 
        get
        {
            return currentHealth;
        }

        private set
        {
            if (value > maxHealth)
            {
                currentHealth = maxHealth;
            }
            else
            {
                currentHealth = value;
            }
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        // Creare logica per la gestione della morte del giocatore
        Debug.Log("Il giocatore è morto.");
        // Aggiungere qui altre azioni da eseguire quando il giocatore muore (tipo resurrezione)
    }
}
