using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class icebergMove : MonoBehaviour
{
    public GameObject player;
    private Rigidbody player_rb;
    private PlayerController playerScript;
    //private PlayerController script;

    public float speed;
    public float x_min;
    public float x_max;
    public float z_min;
    public float z_max;
    public bool move_in_x_dir;

    bool step_on_ice = false;
    //bool move_rhs = false;
    // Start is called before the first frame update
    void Start()
    {
        //speed = speed * Time.fixedDeltaTime;
        player_rb = player.GetComponent<Rigidbody>();
        speed = speed * Time.fixedDeltaTime;
        playerScript = player.GetComponent<PlayerController>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            step_on_ice = true;
            //Debug.Log("player steped on ice");
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            step_on_ice = false;
            //Debug.Log("player not on ice");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (move_in_x_dir)
        {
            float pos_x = transform.position.x;
            if (pos_x >= x_max || pos_x <= x_min)
            {
                speed = -1 * speed;
            }
            pos_x = pos_x + speed;
            if (step_on_ice && !playerScript.gamePause)
            {
                Vector3 player_pos = player.transform.position;
                player_rb.MovePosition(new Vector3 (player_pos.x+speed, player_pos.y, player_pos.z));
            }
            transform.position = new Vector3(pos_x, transform.position.y, transform.position.z);
        }
        else {
            float pos_z = transform.position.z;
            if (pos_z >= z_max || pos_z <= z_min)
            {
                speed = -1 * speed;
            }
            pos_z = pos_z + speed;
            if (step_on_ice && !playerScript.gamePause)
            {
                Vector3 player_pos = player_rb.position;
                player_rb.MovePosition(new Vector3(player_pos.x, player_pos.y, player_pos.z + speed));
            }
            transform.position = new Vector3(transform.position.x, transform.position.y, pos_z);
        }
        
    }
}
