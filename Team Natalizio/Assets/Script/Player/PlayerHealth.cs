using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    Player player;
    [SerializeField]
    int currentHealth, maxHealth;
    [SerializeField]
    float timerImmunity, maxTimerImmunity;
    public bool canTakeDamage, hitted;

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
        player = GetComponent<Player>();
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

    public void TakeDamage(int damageAmount, Vector3 dir, int KickBackForce)
    {
        if (canTakeDamage)
        {
            hitted = true;
            player.rb.AddForce(new Vector3(0, 1, 0) * KickBackForce, ForceMode.Impulse);
            Debug.Log(dir.normalized.x);
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
        if (timerImmunity >= maxTimerImmunity) 
        {
            canTakeDamage = true;
        } 
    }


    private void Die()
    {
        gameObject.SetActive(false);
        Debug.Log("Il giocatore è morto.");
        // Aggiungere qui altre azioni da eseguire quando il giocatore muore (tipo resurrezione)
    }
}
