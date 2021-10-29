using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform player;
    public Transform player_x;
    public float moveSpeed = 1;
    public float turnSpeed = 1;
    void Start()
    {
        if (player == null) player = transform;
    }

    
    void Update()
    {
        Move();
        Turn();
    }

    private void Move()
    {
        if (player == null) return;

        if (Input.GetKey(KeyCode.W))
        {
            Vector3 pos = player.position;
            pos = player.forward * moveSpeed + pos;
            player.position = pos;

        }
        else if(Input.GetKey(KeyCode.S)){

            Vector3 pos = player.position;
            pos = player.forward * -moveSpeed + pos;
            player.position = pos;
        }
    }

    private void Turn()
    {
        if (player == null) return;


        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        if (Input.GetMouseButton(1))
        {
            player.Rotate(Vector3.up,h*turnSpeed );//
        }

        if (Input.GetMouseButton(1))
        {
            player_x.Rotate(Vector3.right, -v * turnSpeed);//
        }

    }
}
