using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Deer : MonoBehaviour
{
    Transform player;
    Rigidbody rb;
    Animator anim;
    [SerializeField, Header("Speed")]
    float moveSpeed;
    [SerializeField]
    float walkSpeed, runSpeed;
    [SerializeField]
    float rayDist;
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
        player = FindAnyObjectByType<Player>().GetComponentInParent<Transform>();
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
        //anim.SetBool("isRunning", isRunning); DA SISTEMARE
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
    }

    void AttackMode()
    {
        isWalking = false;
        isRunning = true;
        moveSpeed = runSpeed;

        if(!takePosPlayer && !directionR)
        {
            playerPosAttack = player.transform.position + new Vector3(-5f, 0f, 0f);
            isAttacking = true;
            takePosPlayer = true;
        }
        else if (!takePosPlayer && directionR)
        {
            playerPosAttack = player.transform.position + new Vector3(5f, 0f, 0f);
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
