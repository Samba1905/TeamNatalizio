using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    Transform player;
    [SerializeField]
    Transform meshPlayer;
    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    float horizontalSpeed, verticalSpeed, rotationSpeed;

    float t;

    [SerializeField]
    float speed, walkSpeed, runSpeed, jumpForce;
    [SerializeField]
    bool isRunning, isJumping;

    private void Update()
    {
        InputComands();
    }

    private void FixedUpdate()
    {
        Movement();
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

        //Funzione da completare per il salto
        if(Input.GetButton("Jump"))
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }
    }

    void Movement()//Funzione per il movimento del personaggio
    {
        speed *= Time.fixedDeltaTime;

        rb.velocity = new Vector3(horizontalSpeed * speed, 0f, 0f); //Funzione per far muovere il giocatore

        if (isJumping) rb.AddForce(Vector3.up * Time.fixedDeltaTime * jumpForce, ForceMode.Impulse); //Al momento vola quasi come un JetPack

        //Rotazione del giocatore in base alla direzione
        if (horizontalSpeed < 0f) player.transform.localEulerAngles = new Vector3(0f, -90f, 0f);
        else if( horizontalSpeed > 0f) player.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
    }

    void CameraMovement() //Funzione per far seguire il Player dalla telecamera con un movimento più leggero
    {
        //Interpolazione per la telecamera che si adatta al movimento
        if (isRunning) t = 7.5f;
        else t = 5f;

        float cameraPosX = Mathf.Lerp(mainCamera.transform.position.x, player.transform.position.x - 1.5f, t * Time.deltaTime); //Posizione X telecamera giocatore
        float cameraPosY = Mathf.Lerp(mainCamera.transform.position.y, player.transform.position.y + 3f, t * Time.deltaTime); //Posizione Y telecamera giocatore
        mainCamera.transform.position = new Vector3(cameraPosX, cameraPosY, mainCamera.transform.position.z); //Movimento telecamera
    }

}
