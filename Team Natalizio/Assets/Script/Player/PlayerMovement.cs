using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerMovement : MonoBehaviour
{
    #region Componets
    Transform player;
    Camera mainCamera;
    Rigidbody rb;
    #endregion

    float horizontalSpeed;

    float t;

    [SerializeField, Header("Movement")]
    float speed;
    [SerializeField]
    float walkSpeed, runSpeed;
    bool isRunning;

    [SerializeField, Header("Jump")]
    float rayLenght;
    [SerializeField]
    float timerJump, maxTimerJump, jumpForce;
    bool isJumping;

    private void Start()
    {
        player = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        InputComands();
    }

    private void FixedUpdate()
    {
        Movement();
        Jump();
        CameraMovement();
    }

    void InputComands()
    {
        //Input per movimento
        horizontalSpeed = Input.GetAxis("Horizontal");

        //Funzione da sistemare per la corsa
        if (Input.GetButton("Run"))
        {
            isRunning = true;
            speed = runSpeed;
        }
        else
        {
            isRunning = false;
            speed = walkSpeed;
        }
    }

    void Movement()//Funzione per il movimento del personaggio
    {
        speed *= Time.fixedDeltaTime;

        rb.velocity = new Vector3(horizontalSpeed * speed, rb.velocity.y, rb.velocity.z); //Funzione per far muovere il giocatore

        //Rotazione del giocatore in base alla direzione
        if (horizontalSpeed < 0f) player.transform.localEulerAngles = new Vector3(0f, -90f, 0f);
        else if( horizontalSpeed > 0f) player.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
    }

    void Jump() //Funzione per saltare
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);

        Debug.DrawRay(transform.position, Vector3.down * rayLenght, Color.red); //Disegno per info in scena

        if (Physics.Raycast(ray, out hit, rayLenght)) //Condizione se colpisce qualcosa
        {
            if (hit.collider.CompareTag("Terrain")) //Se colpisce il terreno, sara' da mofidicare in futuro
            {
                if (Input.GetButtonDown("Jump"))
                {
                    isJumping = true;
                    timerJump = 0f;
                }
            }
        }

        if (Input.GetButtonUp("Jump") || timerJump > maxTimerJump) isJumping = false; //Smette di saltare con le condizioni

        if (isJumping) //Movimento continuo di salto
        {
            rb.velocity = new Vector3 (rb.velocity.x, jumpForce, rb.velocity.y);
            timerJump += Time.deltaTime;
        } 
    }

    void CameraMovement() //Funzione per far seguire il Player dalla telecamera con un movimento pi� leggero
    {
        //Interpolazione per la telecamera che si adatta al movimento
        if (isRunning) t = 7.5f;
        else t = 5f;

        float cameraPosX = Mathf.Lerp(mainCamera.transform.position.x, player.transform.position.x - 1.5f, t * Time.deltaTime); //Posizione X telecamera giocatore
        float cameraPosY = Mathf.Lerp(mainCamera.transform.position.y, player.transform.position.y + 3f, t * Time.deltaTime); //Posizione Y telecamera giocatore
        mainCamera.transform.position = new Vector3(cameraPosX, cameraPosY, mainCamera.transform.position.z); //Movimento telecamera
    }

}
