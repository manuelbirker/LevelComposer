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

    public bool snapToGrid = false;

    public int canPlaceCheck;
    public int pointerInDeadZone;
    public int pointerIsOverUI;

    public ToolTipController ttip;


    private void Start()
    {
        holding = null;
    }


    void InputControls()
    {
        if (GameManager.Instance.settingsOpen)
        {
            return;
        }


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

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (selected != null)
            {
                if (selected.name == "Start")
                {
                    GameManager.Instance.startCount = 0;
                }

                if (selected.name == "Goal")
                {
                    GameManager.Instance.goalCount = 0;
                }
            }


            Destroy(selected);
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


    void Update()
    {
        CursorPosition();

        CheckForOverlap();

        InputControls();

        ShowGhostObject();


        if (EventSystem.current.IsPointerOverGameObject())
        {
            pointerIsOverUI = 1;
        }
        else
        {
            pointerIsOverUI = 0;
        }


        canPlaceCheck = pointerIsOverUI + pointerInDeadZone;

        if (canPlaceCheck == 0)
        {
            canPlace = true;
        }
        else
        {
            canPlace = false;
        }
    }

    public void CheckForOverlap()
    {
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100000f, ~LayerMask.NameToLayer("Ignore Raycast")))
        {
            if (hit.transform.gameObject.CompareTag("Deadzone"))
            {
                pointerInDeadZone = 1;
            }
            else
            {
                pointerInDeadZone = 0;
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
        else
        {
            pointerInDeadZone = 0;
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
            Vector3 pos = new Vector3(Mathf.Round(worldPosition.x / 0.25f) * 0.25f,
                Mathf.Round(worldPosition.y / 0.25f) * 0.25f, 0);

            transform.position = pos;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        selected = col.gameObject;
    }

    private void OnCollisionStay(Collision collision)
    {
        selected = collision.gameObject;
    }


    private void OnCollisionExit(Collision other)
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


        if (holding.name == "Start(Clone)")
        {
            if (GameManager.Instance.startCount > 0)
            {
                ttip.ChangeToolTip("Start already in level!");
                DeSpawn();
                return;
            }
        }

        if (holding.name == "Goal(Clone)")
        {
            if (GameManager.Instance.goalCount > 0)
            {
                ttip.ChangeToolTip("Goal already in level!");
                DeSpawn();
                return;
            }
        }


        int layer = LayerMask.NameToLayer("Ignore Raycast");
        holding.layer = layer;
        holding.GetComponent<ObjectController>().isHeld = true;
        holding.GetComponent<Collider>().enabled = false;


        if (holding.GetComponent<ObjectController>().toolTip != "" &&
            holding.GetComponent<ObjectController>().toolTip != " " &&
            holding.GetComponent<ObjectController>().toolTip != null)
        {
            ttip.ChangeToolTip(holding.GetComponent<ObjectController>().toolTip);
        }
    }

    void Place(GameObject gm)
    {
        if (holding.name == "Start(Clone)")
        {
            if (GameManager.Instance.startCount > 0)
            {
                ttip.ChangeToolTip("Start already in level!");
                DeSpawn();
                return;
            }
            else
            {
                GameManager.Instance.startCount = 1;
            }
        }

        if (holding.name == "Goal(Clone)")
        {
            if (GameManager.Instance.goalCount > 0)
            {
                ttip.ChangeToolTip("Goal already in level!");
                DeSpawn();
                return;
            }
            else
            {
                GameManager.Instance.goalCount = 1;
            }
        }

        holding.name = holding.name.Replace("(Clone)", "").Trim();

        int layer = LayerMask.NameToLayer("Default");
        holding.layer = layer;
        holding.transform.parent = null;
        holding.GetComponent<ObjectController>().isHeld = false;
        holding.GetComponent<Collider>().enabled = true;
        idCounter++;
        holding = Instantiate(gm, transform);

        layer = LayerMask.NameToLayer("Ignore Raycast");
        holding.layer = layer;
        holding.GetComponent<ObjectController>().isHeld = true;
        holding.GetComponent<Collider>().enabled = false;
    }


    public void DeSpawn()
    {
        Destroy(holding);
        holding = null;
    }
}