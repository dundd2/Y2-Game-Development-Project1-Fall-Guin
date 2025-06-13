using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Rigidbody of the player.
    private Rigidbody rb;
    public GameObject player_model;
    //public GameObject obj_rigidbody;

    public GameObject spawnNode;

    // Movement along X and Y axes.
    private float movementX;
    private float movementY;

    // Speed at which the player moves.
    public float speed;
    public float jump_speed;

    // box collider used to detect whether it is in touch with the ground
    public float maxDistance;
    public Vector3 boxSize;
    public LayerMask layerMask;
    public LayerMask layerMaskIce;
    public Animator m_Animator;
    public GameObject deathAnimator;

    public float turnSpeed;
    private bool hasHorizontalInput;
    private bool hasVerticalInput;
    private bool isWalking;

    //private Quaternion rotation = Quaternion.identity;
    private Vector3 movement;


    // sound
    public GameObject footstep;
    public AudioSource jumpSound;

    //scores
    public int score = 0;
    public TMP_Text scoreText;
    public int win_score;

    public bool gamePause = false;

    //lifes
    public int lifes_max;
    private int lifes;
    public TMP_Text lifeText;

    //UI
    public GameObject controller;
    private UI_Controller UI_ControlScript;

    // Start is called before the first frame update.
    void Start()
    {
        // Get and store the Rigidbody component attached to the player.
        rb = GetComponent<Rigidbody>();
        m_Animator = player_model.GetComponent<Animator>();
        footstep.SetActive(false);
        UI_ControlScript = controller.GetComponent<UI_Controller>();
        lifes = lifes_max;
        lifeText.text = "Lifes: " + lifes.ToString();
    }

    public void Initialize(GameObject controller)
    {
       
    }

    // This function is called when a move input is detected.
    void OnMove(InputValue movementValue)
    {
        // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();

        // Store the X and Y components of the movement.
        movementX = 1.0f * movementVector.x;
        movementY = 1.0f * movementVector.y;
        movement.Set(movementX, 0f, movementY);
        movement.Normalize();

    }

    void OnJump()
    {
        if (GroundCheck() && !gamePause)
        {
            rb.AddForce(0.0f, jump_speed, 0.0f);
            //jumpSound.Play();
        }
    }

    // FixedUpdate is called once per fixed frame-rate frame.
    private void FixedUpdate()
    {
        Vector3 currentEulerAngles = new Vector3(0, 0, 0);
        // Create a 3D movement vector using the X and Y inputs.

        hasHorizontalInput = !Mathf.Approximately(movementX, 0f);
        hasVerticalInput = !Mathf.Approximately(movementY, 0f);
        isWalking = (hasHorizontalInput || hasVerticalInput) && GroundCheck();
        m_Animator.SetBool("IsWalking", isWalking);

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, movement, turnSpeed * Time.deltaTime, 0f);

        Vector3 reverseForward = new Vector3(-1 * movement.x, 0, -1 * movement.z);
        Vector3 desiredBackward = Vector3.RotateTowards(transform.forward, reverseForward, turnSpeed * Time.deltaTime, 0f);
        Quaternion rotationForward = Quaternion.LookRotation(desiredBackward);


        if (!gamePause)
        {
            Vector3 new_pos = rb.position + movement * speed * desiredForward.magnitude;
            if (new_pos.x < -18 ) {
                new_pos.x = -18;
            }
            if (new_pos.x > 34 ) {
                new_pos.x = 34;
            }
            if (new_pos.z < -100)
            {
                new_pos.z = -100;
            }
            if (new_pos.z > 100)
            {
                new_pos.z = 100;
            }
            rb.MovePosition(new_pos);

            //Vector3 m_EulerAngleVelocity = new Vector3(0, desiredForward.y, 0);
            //Quaternion deltaRotation = Quaternion.Euler(turnSpeed * m_EulerAngleVelocity * Time.fixedDeltaTime);
            rb.MoveRotation(rotationForward);

        }
        if (isWalking)
        {
            footstep.SetActive(true);
        } else
        {
            footstep.SetActive(false);
        }

    }
    // check if the ball is on ground
    bool GroundCheck()
    {
        return Physics.CheckBox(transform.position - Vector3.down * maxDistance, boxSize, transform.rotation, layerMask);
    }

    public bool GroundCheck_ice()
    {
        return Physics.CheckBox(transform.position - Vector3.down * maxDistance, boxSize, transform.rotation, layerMaskIce);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("pickups"))
        {
            //Debug.Log("collide with pickup");
            //controlScript.points++;
            //controlScript.SetPoints();
            score++;
            collider.gameObject.SetActive(false);
            scoreText.text = "points: " + score.ToString();
            //if(score == win_score)
            //{
            //    winGame();
            //}
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("goal"))
        {
            winGame();
        } else if (collision.gameObject.CompareTag("water")) {
            if (lifes > 0)
            {
                killPlayer();
            }
            else
            {
                endGame();
            }
        }
    }

    void killPlayer()
    {
        gamePause = true;
        UI_ControlScript.gamePause = true;
        player_model.SetActive(false);
        deathAnimator.SetActive(true);
        lifes--;
        lifeText.text = "Lifes: " + lifes.ToString();

        StartCoroutine(deathAnimation());
    }
    IEnumerator deathAnimation()
    {
        yield return new WaitForSeconds(1.50f);
        deathAnimator.SetActive(false);
        player_model.SetActive(true);
        gamePause = false;
        UI_ControlScript.gamePause = false;
        RespawnPlayer();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position - Vector3.down * maxDistance, boxSize);
    }

    void winGame()
    {
        gamePause = true;
        UI_ControlScript.winGame();
        footstep.SetActive(false);
        //t_pauseMenu.text = ;
    }

    void endGame()
    {
        gamePause = true;
        UI_ControlScript.loseGame();
        footstep.SetActive(false);
        //t_pauseMenu.text = ;
    }

    public void StartGame()
    {
        gamePause = false;
        GameObject[] pickups = GameObject.FindGameObjectsWithTag("pickups");
        foreach (GameObject pickup in pickups)
        {
            pickup.SetActive(true);
        }
        score = 0;
        lifes = lifes_max;
        scoreText.text = "points: 0";
        lifeText.text = "Lifes: " + lifes.ToString();
        RespawnPlayer();
    }

    void RespawnPlayer()
    {
        footstep.SetActive(false);
        rb.position = spawnNode.transform.position;
    }
}
