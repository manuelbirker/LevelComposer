using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public GameObject holding;
    public GameObject selected;
    public bool canPlace;


    void Update()
    {
        CursorPosition();

        CheckForOverlap();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (canPlace)
            {
                if (holding != null)
                {
                    Place(holding);
                }
            }
        }

        ShowGhostObject();
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Destroy(selected);
        }
    }

    public void CheckForOverlap()
    {
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.CompareTag("Saveable"))
            {
                if (hit.transform.gameObject != holding.transform.gameObject)
                {
                    selected = hit.transform.gameObject;
                    canPlace = false;
                }
            } 
        }
        else
        {
            canPlace = true;
            selected = null;
        }
    }


    public void CursorPosition()
    {
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);


        transform.position = new Vector3(
            worldPosition.x,
            worldPosition.y,
            0
        );
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        selected = col.gameObject;
        canPlace = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        selected = collision.gameObject;
        canPlace = false;
    }


    private void OnCollisionExit2D(Collision2D other)
    {
        selected = null;
        canPlace = true;
    }


    private void ShowGhostObject()
    {
        if (!holding)
        {
            return;
        }

        holding.transform.position = this.transform.position;
    }


    void Spawn(GameObject gm)
    {
        if (holding)
        {
            holding = null;
        }

        holding = Instantiate(gm, transform);

        holding.GetComponent<ObjectController>().isHeld = true;
        holding.GetComponent<Collider>().enabled = false;
    }

    void Place(GameObject gm)
    {
        holding.transform.SetParent(null);
        holding.GetComponent<ObjectController>().isHeld = false;
        holding.GetComponent<Collider>().enabled = true;
        Spawn(gm);
    }
}