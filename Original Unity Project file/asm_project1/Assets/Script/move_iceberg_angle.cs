using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_iceberg_angle : MonoBehaviour
{
    public float speed;
    public float x_start;
    public float x_end;
    public float z_start;
    public float z_end;
    bool move_to_end = false;
    bool step_on_ice = false;
    Vector3 start_pos;
    Vector3 end_pos;

    public GameObject player;
    private Rigidbody player_rb;
    private PlayerController playerScript;
    // Start is called before the first frame update
    void Start()
    {
        speed = speed * Time.fixedDeltaTime;
        start_pos = new Vector3(x_start, transform.position.y, z_start);
        end_pos = new Vector3(x_end, transform.position.y, z_end);
        player_rb = player.GetComponent<Rigidbody>();
        playerScript = player.GetComponent<PlayerController>(); 

        //debugPosition();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 next_pos;
        //move to end point if not reach
        if (!move_to_end)
        {
            next_pos = Vector3.MoveTowards(transform.position, end_pos, speed);
            if (!Is_within_range(x_start, x_end, next_pos.x) || !Is_within_range(z_start, z_end, next_pos.z))
            {
                move_to_end = true;
                next_pos = end_pos;
                //Debug.Log("hits end point");
                //debugPosition();
            }
        }
        else
        {
            next_pos = Vector3.MoveTowards(transform.position, start_pos, speed);
            if (!Is_within_range(x_start, x_end, next_pos.x) || !Is_within_range(z_start, z_end, next_pos.z))
            {
                move_to_end = false;
                next_pos = start_pos;
                //Debug.Log("hits start point");
                //debugPosition();
            }
        }
        Vector3 step = transform.position - next_pos;
        if (step_on_ice && !playerScript.gamePause)
        {
            player_rb.AddForce(player.transform.position - step);
        }
        transform.position = next_pos;
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
    void debugPosition()
    {
        Debug.Log("x:" + transform.position.x + ", y:" + transform.position.y + ", z:" + transform.position.z);
    }

    bool Is_within_range(float start, float end, float num)
    {
        if (start > end)
        {
            if (num >= start)
            { return false; }
            else if (num <= end)
            { return false; }
            else
            { return true; }
        }
        else
        {
            if (num <= start)
            { return false; }
            else if (num >= end)
            { return false; }
            else
            { return true; }
        }
    }
}
