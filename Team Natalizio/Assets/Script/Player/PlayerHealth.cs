using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour

{
    public int maxHealth = 10;
    private int currentHealth;
    public int damageAmount;

   // Start is called before the first frame update
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
