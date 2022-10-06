using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isGrounded;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Remove(other.gameObject.name.Length-1) != "Floor" && other.gameObject.name != "TrampolineUp"&& other.gameObject.name != "TrampolineDown")
        {
            return;
        }

        isGrounded = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Remove(other.gameObject.name.Length-1) != "Floor" && other.gameObject.name != "TrampolineUp"&& other.gameObject.name != "TrampolineDown")
        {
            return;
        }


        isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Remove(collision.gameObject.name.Length-1) != "Floor" && collision.gameObject.name != "TrampolineUp"&& collision.gameObject.name != "TrampolineDown")
        {
            return;
        }

        isGrounded = true;   
    }


    private void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.name.Remove(collisionInfo.gameObject.name.Length-1) != "Floor" && collisionInfo.gameObject.name != "TrampolineUp"&& collisionInfo.gameObject.name != "TrampolineDown")
        {
            return;
        }

        isGrounded = true;   
    }


    private void OnCollisionExit(Collision other)
    {
        isGrounded = false; 
    }
}