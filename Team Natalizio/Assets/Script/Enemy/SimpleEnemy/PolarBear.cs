using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PolarBear : MonoBehaviour
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
    float rayDist;

    [SerializeField]
    bool isWalking, isRunning, isAttacking;
    [SerializeField]
    bool directionR, restTime;

    bool playerHitted;

    [SerializeField]
    Transform[] waypoints;
    int nWay;
    [SerializeField]
    float distance, idleTimer, distancePlayer, restTimer;

    enum PBearState
    {
        normal,
        attack
    }
    PBearState state;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        playerHP = FindAnyObjectByType<PlayerHealth>();
    }

    void Update()
    {
        if(state == PBearState.normal)
        {
            Rotation();
        }
        else if(state == PBearState.attack)
        {

        }

        AnimUpdate();
        DetectPlayer();
        DetectRotation();

    }

    private void FixedUpdate()
    {
        if (state == PBearState.normal)
        {
            Movement();
        }
        else if(state == PBearState.attack)
        {

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerHitted = true;
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
            if (idleTimer > 3f)
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
        else if (!directionR)
        {
            rb.velocity = new Vector3(-moveSpeed * Time.fixedDeltaTime, rb.velocity.y, rb.velocity.z);
        }
    }
    void AnimUpdate()
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
                Debug.Log("PLAYER");
                state = PBearState.attack;
            }
        }
    }
    void AttackMode()
    {
        isWalking = false;
        isRunning = true;
        moveSpeed = runSpeed;
        isAttacking = true;

        if (isAttacking)
        {
            if (distancePlayer >= 5f)
            {
                rb.velocity = new Vector3(moveSpeed * Time.fixedDeltaTime, rb.velocity.y, rb.velocity.z);
            }
            else if (distancePlayer <= -5f)
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

    void DetectPlayerForDamage()
    {
        if (playerHitted) //playerHP.TakeDamage(1);
            Debug.Log("PRESO");
    }

    bool DetectRotation()
    {
        if (transform.localEulerAngles.y > 180f) return directionR = false;
        return directionR = true;
    }
}
