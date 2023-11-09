using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public class EnemyMovementAndTouchDmg : MonoBehaviour
{
    public float speed = 5f;
    public int damageAmount = 10;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Movimento del nemico verso il giocatore
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Se il nemico entra in collisione con il giocatore
        if (collision.gameObject.CompareTag("Player"))
        {
            // Cercare un componente di salute sul giocatore e infliggere danni se presente
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}