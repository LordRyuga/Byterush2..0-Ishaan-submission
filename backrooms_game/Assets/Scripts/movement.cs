using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movement : MonoBehaviour
{
    //References to other game objects
    public Rigidbody player;
    public Transform orientation;
    public Camera mainCam;
    public LayerMask Ground;
    public Image staminaBar;

    //Movement variables
    public float movespeed;
    public float maxspeed;
    public float boostedSpeed;
    public float originalMaxSpeed;
    public float drag;
    public float jumpvel;
    bool readyToJump;
    bool refillStamina;
    public float stamina;
    public float maxStamina = 4.0f;
    public float currentspeed;

    //variables to check for ground using raycast to add drag
    public float PlayerHeight;
    bool grounded;

    //input variables
    float horizontalInput;
    float verticalInput;

    Vector3 MovementDirection;

    private void Start()
    {
        player = GetComponent<Rigidbody>();     //setup rigidbody
        player.freezeRotation = true;           
        readyToJump = true;                     //Initially ready to jump
        stamina = maxStamina;                   //Initally stamina bar is filled
        refillStamina = false;
        boostedSpeed = 1.5f * maxspeed;
        originalMaxSpeed = maxspeed;
    }

    private void FixedUpdate()
    {
        PlayerMovement();                       //call player movement every frame
    }
    private void Update()
    {
        currentspeed = player.velocity.magnitude;
        grounded = Physics.Raycast(transform.position, Vector3.down, PlayerHeight * 0.5f + 0.2f, Ground);
        staminaBar.fillAmount = stamina / maxStamina;
        if (grounded)
        {
            player.drag = drag;
        }
        else
        {
            player.drag = 0;
        }

        if (readyToJump && Input.GetKey(KeyCode.Space))
        {
            Jump();
        }

        LimitSpeed();
        Myinput();
        
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if(stamina > 0.1f)
            {
                maxspeed = boostedSpeed;
                refillStamina = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            maxspeed = originalMaxSpeed;
            mainCam.fieldOfView = 60f;
            refillStamina = true;
        }
        if (Input.GetKey(KeyCode.LeftControl) && stamina > 0f)
        {
            stamina -= Time.deltaTime;
            if (stamina < 0.05f)
            {
                refillStamina = true;
                stamina = 0f;
            }
            if(stamina > 0f)
            {
                mainCam.fieldOfView = 65f;
            }
        }
        if(refillStamina)
        {
            maxspeed = originalMaxSpeed;
            mainCam.fieldOfView = 60f;
            stamina += Time.deltaTime * 0.5f;
            if (stamina > maxStamina)
            {
                stamina = maxStamina;
                refillStamina = false;
            }
        }
    }

    private void Myinput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void PlayerMovement()
    {
        MovementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        player.AddForce(MovementDirection.normalized * movespeed * 10f, ForceMode.Force);
    }

    private void LimitSpeed()
    {
        Vector3 Vel = new Vector3(player.velocity.x, 0, player.velocity.z);

        if(Vel.magnitude > maxspeed)
        {
            Vector3 maxVel = Vel.normalized * maxspeed;
            player.velocity = new Vector3(maxVel.x, player.velocity.y, maxVel.z);
        }
    }
    private void Jump()
    {
        player.velocity = new Vector3(player.velocity.x * 1.3f, 0, player.velocity.z * 1.3f);
        player.AddForce(transform.up * jumpvel, ForceMode.Impulse);
        readyToJump = false;
        stamina -= 0.2f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "ground")
        {
            readyToJump = true;
            refillStamina = true;
        }
    }
}
