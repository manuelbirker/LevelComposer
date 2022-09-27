using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMoveController : MonoBehaviour
{
    public float speed = 1f;

    private Rigidbody rb;


    public int minX = 0;
    public int maxX = 10;
    public int minY = 0;
    public int maxY = 10;

    public BPMController bpmcontroller;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        maxX = bpmcontroller.levelLength;
    }

    private void FixedUpdate()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed, 0);

        if (transform.position.x < minX)
        {
            transform.position = new Vector3(minX + speed, transform.position.y, 0);
            return;
        }

        if (transform.position.x > maxX)
        {
            transform.position = new Vector3(maxX - speed, transform.position.y, 0);
            return;
        }


        if (transform.position.y < minY)
        {
            transform.position = new Vector3(transform.position.x, minY + speed, 0);
            return;
        }

        if (transform.position.y > maxY)
        {
            transform.position = new Vector3(transform.position.x, maxY - speed, 0);
            return;
        }

        rb.velocity = input;
    }
}