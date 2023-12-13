using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerMovement playerMove;
    Rigidbody rb;

    [SerializeField]
    float rayLenght;
    [SerializeField]
    bool bounceBack;
    [SerializeField]
    float bounceForce, timerBounce;

    public bool canDamage;

    public bool BounceBack { get { return bounceBack; } }

    int _present;

    public int Present { get { return _present; } }




    private void Start()
    {
        playerMove = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        SantaAttack();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            _present++;
            other.gameObject.SetActive(false);
        }    
    }

    void SantaAttack()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        
        Debug.DrawRay(transform.position, Vector3.down * rayLenght, Color.blue); //Disegno per info in scena

        timerBounce += Time.deltaTime;

        if (Physics.Raycast(ray, out hit, rayLenght)) //Condizione se colpisce qualcosa
        {
            if (playerMove.SmashAttack && hit.collider.gameObject.layer == 6)
            {
                if (canDamage)
                {
                    hit.collider.gameObject.GetComponentInParent<EnemyHealt>().TakeDamage(1);
                    bounceBack = true;
                    timerBounce = 0;
                }
            }
        }
        if(playerMove.OnGround || timerBounce > 0.3f) bounceBack = false;

        if(bounceBack) rb.AddForce(Vector3.up * bounceForce, ForceMode.Force);
    }
}
