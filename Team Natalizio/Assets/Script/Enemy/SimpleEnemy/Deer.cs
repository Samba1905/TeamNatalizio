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
    Transform [] waypoints;
    int nWay;
    [SerializeField]
    float distance, idleTimer;

    enum DeerState
    {
        normal,
        attack
    }

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
            Movement();
        }
        else if(state == DeerState.attack)
        {
            AttackMode();
        }
        animUpdate();
        DetectPlayer();
    }

    void Movement()
    {
        if (transform.localEulerAngles.y > 180f)
        {
            rb.velocity = new Vector3(-moveSpeed * Time.deltaTime, rb.velocity.y, rb.velocity.z);
        }
        else
        {
            rb.velocity = new Vector3(moveSpeed * Time.deltaTime, rb.velocity.y, rb.velocity.z);
        }

        distance = waypoints[nWay].transform.position.x - transform.position.x;

        if (distance < 0) transform.localEulerAngles = new Vector3(0f, 270f, 0f);
        else if (distance > 0) transform.localEulerAngles = new Vector3(0f, 90f, 0f);

        if(distance < 0.25f && distance > -0.25f)
        {
            moveSpeed = 0f;
            isWalking = false;
            idleTimer += Time.deltaTime;
            if(idleTimer > 4.5f)
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
                Debug.Log("Player");
            }
        }
    }

    void AttackMode()
    {
        isWalking = false;
        isRunning = true;
        moveSpeed = runSpeed;

        if(!takePosPlayer)
        {
            playerPosAttack = player.transform.position + new Vector3(-7f, 0f, 0f);
            isAttacking = true;
            takePosPlayer = true;
        }

        if(isAttacking)
        {
            float distance = playerPosAttack.x - transform.position.x;
            Debug.Log(distance);
            if(distance < 0.5f && distance > -0.5f) rb.velocity = new Vector3(-moveSpeed * Time.deltaTime, rb.velocity.y, rb.velocity.z);
        }
    }
}
