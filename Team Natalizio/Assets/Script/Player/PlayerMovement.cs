using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerMovement : MonoBehaviour
{
    #region Componets
    Transform playerT;
    Camera mainCamera;
    Rigidbody rb;
    public Animator anim;
    Player player;
    #endregion

    float t; //Valore per movimento Camera

    [SerializeField, Header("Movement")]
    float speed;
    [SerializeField]
    float walkSpeed, runSpeed;
    float horizontalSpeed;
    [SerializeField]
    float smashSpeed;
    bool isRunning, isWalking, canMove;

    [SerializeField, Header("Jump")]
    float rayLenght;
    [SerializeField]
    float timerJump, maxTimerJump, jumpForce;
    [SerializeField]
    bool isJumping, smashAttack;
    bool onGround;
    Ray ray;

    public bool SmashAttack { get { return smashAttack; } }
    public bool OnGround { get { return onGround; } }


    private void Start()
    {
        playerT = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        mainCamera = Camera.main;
        player = GetComponent<Player>();
    }

    private void Update()
    {
        InputComands();
        JumpDetect();
        Animation();
    }

    private void FixedUpdate()
    {
        Movement();
        CameraMovement();
    }

    void InputComands()
    {
        //Input per movimento
        if (canMove)
        {
            horizontalSpeed = Input.GetAxis("Horizontal");

            //Funzione da sistemare per la corsa
            if (Input.GetButton("Run"))
            {
                isRunning = true;
                isWalking = false;
                speed = runSpeed;
            }
            else
            {
                isRunning = false;
                isWalking = true;
                speed = walkSpeed;
            }

            if (horizontalSpeed == 0f) isWalking = false;
        }

        if(!onGround) //Condizione per poter schiacciare il nemico
        {
            if(Input.GetButtonDown("Vertical"))
            {
                canMove = false;
                smashAttack = true;
            }
        }
    }

    void Movement()//Funzione per il movimento del personaggio
    {
         //Funzione per far muovere il giocatore
        RaycastHit hit;
        if (horizontalSpeed < 0f)
        {
            playerT.transform.localEulerAngles = new Vector3(0f, -90f, 0f);
            ray = new Ray(transform.position, new Vector3(-0.5f, 0, 0));
        }
        else if (horizontalSpeed > 0f) 
        {
            playerT.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
            ray = new Ray(transform.position, new Vector3(0.5f, 0, 0));

        }

        Debug.DrawRay(transform.position, new Vector3(0.5f, 0, 0), Color.blue);
        if (Physics.Raycast(ray, out hit, 0.5f)) //Condizione se colpisce qualcosa
        {
            if (hit.collider.CompareTag("Limit")) //Se colpisce il terreno, sara' da mofidicare in futuro
            {

                rb.velocity = new Vector3(horizontalSpeed * 0 * Time.deltaTime, rb.velocity.y, rb.velocity.z);
            }
            else
            {
                rb.velocity = new Vector3(horizontalSpeed * speed * Time.deltaTime, rb.velocity.y, rb.velocity.z);
            }

        }
        else
        {
            rb.velocity = new Vector3(horizontalSpeed * speed * Time.deltaTime, rb.velocity.y, rb.velocity.z);
        }
        if (horizontalSpeed < 0f) playerT.transform.localEulerAngles = new Vector3(0f, -90f, 0f);
        else if (horizontalSpeed > 0f) playerT.transform.localEulerAngles = new Vector3(0f, 90f, 0f);

        //Rotazione del giocatore in base alla direzione


        if (isJumping) //Movimento continuo di salto
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.y);
            timerJump += Time.deltaTime;
        }

        //Condizione per fare la schiacciata
        if (smashAttack)
        {
            player.canDamage = true;
            rb.AddForce(Vector3.down * smashSpeed * Time.fixedDeltaTime, ForceMode.Impulse);
        }

        if(player.BounceBack)
        {
            smashAttack = false;
        }
    }

    void JumpDetect() //Funzione per saltare
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);

        Debug.DrawRay(transform.position, Vector3.down * rayLenght, Color.red); //Disegno per info in scena

        if (Physics.Raycast(ray, out hit, rayLenght)) //Condizione se colpisce qualcosa
        {
            if (hit.collider.CompareTag("Terrain")) //Se colpisce il terreno, sara' da mofidicare in futuro
            {
                onGround = true;
                canMove = true;
                smashAttack = false;
                if (Input.GetButtonDown("Jump"))
                {
                    
                    isJumping = true;
                    timerJump = 0f;
                }
            }
        }
        else onGround = false;

        //Smette di saltare con le condizioni
        if (Input.GetButtonUp("Jump") || timerJump > maxTimerJump) isJumping = false;
    }

    void CameraMovement() //Funzione per far seguire il Player dalla telecamera con un movimento pi� leggero
    {
        //Interpolazione per la telecamera che si adatta al movimento
        if (isRunning) t = 7.5f;
        else t = 5f;

        float cameraPosX = Mathf.Lerp(mainCamera.transform.position.x, playerT.transform.position.x - 1.5f, t * Time.deltaTime); //Posizione X telecamera giocatore
        float cameraPosY = Mathf.Lerp(mainCamera.transform.position.y, playerT.transform.position.y + 3f, t * Time.deltaTime); //Posizione Y telecamera giocatore
        mainCamera.transform.position = new Vector3(cameraPosX, cameraPosY, mainCamera.transform.position.z); //Movimento telecamera
    }

    void Animation()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isFloating", !onGround);
        anim.SetBool("isSmashing", smashAttack);
        anim.SetBool("isRunning", isRunning);
    }

}
