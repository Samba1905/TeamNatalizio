using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    int currentHealth, maxHealth;
    [SerializeField]
    float timerImmunity, maxTimerImmunity;
    bool canTakeDamage;

    public int HealtPoints
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
        canTakeDamage = true;
    }

    void Update() 
    { 
        TimerImmunity();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == 8)
        {
            HealtPoints++;
            other.gameObject.SetActive(false);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (canTakeDamage)
        {
            HealtPoints -= damageAmount;
            Immunity();
            timerImmunity = 0f;
            if (HealtPoints <= 0)
            {
                Die();
            }
        }
    }

    bool Immunity()
    {
        return canTakeDamage = false;
    }

    void TimerImmunity()
    {
        timerImmunity += Time.deltaTime;
        if(timerImmunity >= maxTimerImmunity) canTakeDamage = true;
    }


    private void Die()
    {
        gameObject.SetActive(false);
        Debug.Log("Il giocatore è morto.");
        // Aggiungere qui altre azioni da eseguire quando il giocatore muore (tipo resurrezione)
    }
}
