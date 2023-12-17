using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    Player player;
    PlayerMovement playerMovement;
    [SerializeField]
    int currentHealth, maxHealth;
    [SerializeField]
    float timerImmunity, maxTimerImmunity;
    bool canTakeDamage;
    [SerializeField] TMP_Text currentHp; 

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
        playerMovement = GetComponent<PlayerMovement>();
        currentHealth = maxHealth;
        canTakeDamage = true;
        currentHp.text = HealtPoints.ToString();
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
            currentHp.text = HealtPoints.ToString();
        }
    }

    public void TakeDamage(int damageAmount, Vector3 dir, int KickBackForce)
    {
        if (canTakeDamage)
        {
            playerMovement.anim.SetTrigger("Damage");
            player.rb.AddForce(new Vector3(0, 1, 0) * KickBackForce, ForceMode.Impulse);
            HealtPoints -= damageAmount;
            Immunity();
            timerImmunity = 0f;
            if (HealtPoints <= 0)
            {
                Die();
            }
            currentHp.text = HealtPoints.ToString();
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
