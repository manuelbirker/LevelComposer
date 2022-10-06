using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isGrounded;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Remove(other.gameObject.name.Length-1) != "Floor")
        {
            return;
        }

        isGrounded = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Remove(other.gameObject.name.Length-1) != "Floor")
        {
            return;
        }


        isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;
    }
}