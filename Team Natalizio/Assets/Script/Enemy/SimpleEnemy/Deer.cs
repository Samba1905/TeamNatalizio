using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Deer : MonoBehaviour
{
    Transform playerPos;
    PlayerHealth playerHP;
    EnemyHealt myHealth;
    Rigidbody rb;
    Animator anim;
    [SerializeField, Header("Speed")]
    float moveSpeed;
    [SerializeField]
    float walkSpeed, runSpeed;
    [SerializeField]
    float rayDist, distDmg;
    [SerializeField]
    bool isWalking, isRunning, isAttacking;
    [SerializeField]
    bool takePosPlayer;
    Vector3 playerPosAttack;
    [SerializeField]
    bool directionR, restTime;

    [SerializeField]
    Transform [] waypoints;
    int nWay;
    [SerializeField]
    float distance, idleTimer, distancePlayer, restTimer;

    enum DeerState
    {
        normal,
        attack
    }

    [SerializeField]
    DeerState state;

    private void Start()
    {
        playerHP = FindAnyObjectByType<PlayerHealth>();
        playerPos = FindAnyObjectByType<Player>().GetComponentInParent<Transform>();
        myHealth = GetComponent<EnemyHealt>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        state = DeerState.normal;
    }

    private void Update()
    {
        if (state == DeerState.normal)
        {
            Rotation();
        }
        else if (state == DeerState.attack)
        {
            ChargeMode();
            RestTimer();
            ChangeState();
        }
        animUpdate();
        DetectPlayer();
        DetectRotation();
        DeadState();
    }

    private void FixedUpdate()
    {
        if (state == DeerState.normal)
        {
            Movement();
        }
        else if (state == DeerState.attack)
        {
            AttackMode();
        }
    }

    void DeadState()
    {
        if (myHealth.IsDeath)
        {
            isAttacking = false;
            isRunning = false;
            isWalking = false;
            moveSpeed = 0;
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);
            enabled = false;
        }
    }

    void Rotation()
    {
        distance = waypoints[nWay].transform.position.x - transform.position.x;

        if (distance < 0) transform.localEulerAngles = new Vector3(0f, 270f, 0f);
        else if (distance > 0) transform.localEulerAngles = new Vector3(0f, 90f, 0f);

        if (distance < 0.25f && distance > -0.25f)
        {
            moveSpeed = 0f;
            isWalking = false;
            idleTimer += Time.deltaTime;
            if (idleTimer > 4.5f)
            {
                nWay = (nWay + 1) % waypoints.Length;
                idleTimer = 0f;
            }
        }
        else
        {
            moveSpeed = walkSpeed;
            isWalking = true;
        }
    }

    void Movement()
    {
        if (directionR)
        {
            rb.velocity = new Vector3(moveSpeed * Time.fixedDeltaTime, rb.velocity.y, rb.velocity.z);
        }
        else if(!directionR)
        {
            rb.velocity = new Vector3(-moveSpeed * Time.fixedDeltaTime, rb.velocity.y, rb.velocity.z);
        }    
    }

    void animUpdate()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isRunning", isRunning);
    }

    void DetectPlayer()
    {
        Ray ray = new Ray(transform.position + Vector3.up, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDist))
        {
            Debug.DrawRay(transform.position + Vector3.up, transform.forward * hit.distance, Color.red);
            if (hit.collider.CompareTag("Player"))
            {
                state = DeerState.attack;
            }
        }

        if (Physics.Raycast(ray, out hit, distDmg))
        {
            Debug.DrawRay(transform.position + Vector3.up, transform.forward * hit.distance, Color.blue);
            if (hit.collider.CompareTag("Player"))
            {
                playerHP.TakeDamage(1);
            }
        }
    }

    void AttackMode()
    {
        isWalking = false;
        isRunning = true;
        moveSpeed = runSpeed;

        if(!takePosPlayer && !directionR)
        {
            playerPosAttack = playerPos.transform.position + new Vector3(-5f, 0f, 0f);
            isAttacking = true;
            takePosPlayer = true;
        }
        else if (!takePosPlayer && directionR)
        {
            playerPosAttack = playerPos.transform.position + new Vector3(5f, 0f, 0f);
            isAttacking = true;
            takePosPlayer = true;
            
        }

        if (isAttacking)
        {        
            if (distancePlayer >= 5f)
            {
                rb.velocity = new Vector3(moveSpeed * Time.fixedDeltaTime, rb.velocity.y, rb.velocity.z);
            }
            else if(distancePlayer <= -5f)
            {
                rb.velocity = new Vector3(-moveSpeed * Time.fixedDeltaTime, rb.velocity.y, rb.velocity.z);
            }
            else
            {
                isRunning = false;
                restTime = true;
            }
        }
    }

    void ChangeState()
    {
        if (restTimer > 3.5f)
        {
            state = DeerState.normal;
            takePosPlayer = false;
            isAttacking = false;
            isRunning = false;
            isWalking = true;
            restTime = false;
        }
    }

    float ChargeMode()
    {
        return distancePlayer = playerPosAttack.x - transform.position.x;
    }

    float RestTimer()
    {
        if(restTime) return restTimer += Time.deltaTime;
        return restTimer = 0f;
    }

    bool DetectRotation()
    {
        if (transform.localEulerAngles.y > 180f) return directionR = false;           
        return directionR = true;
    }
}
