using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Deer : MonoBehaviour
{
    Transform player;
    Rigidbody rb;
    [SerializeField, Header("Speed")]
    float moveSpeed;
    [SerializeField]
    float walkSpeed, runSpeed;
    [SerializeField]
    float rayDist;
    [SerializeField]
    bool isWalking, isRunning, isAttacking;

    [SerializeField]
    Transform [] waypoints;
    int nWay;
    [SerializeField]
    float distance, idleTimer;





    private void Start()
    {
        player = FindAnyObjectByType<Player>().GetComponentInParent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Movement();
    }



    void Movement()
    {
        if (transform.rotation.y == 90) rb.velocity = new Vector3(-moveSpeed * Time.deltaTime, rb.velocity.y, rb.velocity.z);
        else rb.velocity = new Vector3(moveSpeed * Time.deltaTime, rb.velocity.y, rb.velocity.z);

        distance = waypoints[nWay].transform.position.x - transform.position.x;

        if (distance < 0) transform.localEulerAngles = new Vector3(0f, -90f, 0f);
        else if (distance > 0) transform.localEulerAngles = new Vector3(0f, 90f, 0f);

        if(distance < 0.25)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
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
        }
    }

    void DetectPlayer()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDist))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
        }
    }
}
