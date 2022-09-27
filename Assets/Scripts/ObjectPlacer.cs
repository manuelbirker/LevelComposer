using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectPlacer : MonoBehaviour
{
    public GameObject holding;
    public GameObject selected;
    public bool canPlace;
    public bool pointerOverUI;

    public bool snapToGrid = false;

    private void Start()
    {
        holding = null;
    }

    void Update()
    {
        CursorPosition();

        CheckForOverlap();

        pointerOverUI = EventSystem.current.IsPointerOverGameObject();

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (holding != null)
            {
                DeSpawn();
            }
        }


        ShowGhostObject();
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Destroy(selected);
        }


        if (pointerOverUI)
        {
            canPlace = false;
        }
        else
        {
            canPlace = true;
        }


        if (Input.GetKey(KeyCode.LeftControl))
        {
            snapToGrid = true;
        }
        else
        {
            snapToGrid = false;
        }
    }

    public void CheckForOverlap()
    {
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.CompareTag("Deadzone"))
            {
                canPlace = false;
            }
            else
            {
                canPlace = true;
                selected = null;
            }


            if (hit.transform.gameObject.CompareTag("Saveable"))
            {
                selected = hit.transform.gameObject;
            }
            else
            {
                selected = null;
            }
        }
    }


    public void CursorPosition()
    {
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);


        if (!snapToGrid)
        {
            transform.position = new Vector3(
                worldPosition.x,
                worldPosition.y,
                0
            );
        }
        else
        {
            Vector3 pos = new Vector3(Mathf.Round(worldPosition.x / 1) * 1,
                Mathf.Round(worldPosition.y / 1) * 1, 0);

            transform.position = pos;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        selected = col.gameObject;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        selected = collision.gameObject;
    }


    private void OnCollisionExit2D(Collision2D other)
    {
        selected = null;
    }


    private void ShowGhostObject()
    {
        if (!holding)
        {
            return;
        }

        holding.transform.position = this.transform.position;
    }


    public int idCounter;

    public void Spawn(GameObject gm)
    {
        DeSpawn();

        holding = Instantiate(gm, transform);
        int layer = LayerMask.NameToLayer("Ignore Raycast");
        holding.layer = layer;
        holding.GetComponent<ObjectController>().isHeld = true;
        holding.GetComponent<Collider>().enabled = false;
    }

    void Place(GameObject gm)
    {
        int layer = LayerMask.NameToLayer("Default");
        holding.layer = layer;
        holding.name = idCounter.ToString();
        holding.transform.parent = null;
        holding.GetComponent<ObjectController>().isHeld = false;
        holding.GetComponent<Collider>().enabled = true;
        idCounter++;
        holding = Instantiate(gm, transform);

        layer = LayerMask.NameToLayer("Ignore Raycast");
        holding.layer = layer;
    }


    public void DeSpawn()
    {
        Destroy(holding);
        holding = null;
    }
}