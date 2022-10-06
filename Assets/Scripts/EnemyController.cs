using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public class EnemyController : MonoBehaviour
{
    public float boundariesMinX = 5.0f;
    public float boundariesMaxX = 5.0f;
    public int direction = -1;
    public float movingSpeed = 1f;

    public float _movingSpeed;
    public Vector3 startPos;
    public ObjectController oC;
    public bool activated = false;

    private void Awake()
    {
        oC = GetComponent<ObjectController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!activated)
        {
            if (oC.isHeld)
            {
                startPos = transform.position;
                activated = true;
                _movingSpeed = movingSpeed;
            }
        }


        if (GameManager.Instance._gameState != GameManager.GameState.PlayLevel &&
            GameManager.Instance._gameState != GameManager.GameState.PlayTest)
        {
            GetComponent<Rigidbody>().isKinematic = true;

            return;
        }

        GetComponent<Rigidbody>().isKinematic = false;

        switch (direction)
        {
            case -1:
                // Moving Left
                if (transform.position.x > boundariesMinX)
                {
                    GetComponent<Rigidbody>().velocity =
                        new Vector2(-movingSpeed, GetComponent<Rigidbody>().velocity.y);
                }
                else
                {
                    direction = 1;
                }

                break;
            case 1:
                //Moving Right
                if (transform.position.x < boundariesMaxX)
                {
                    GetComponent<Rigidbody>().velocity =
                        new Vector2(movingSpeed, GetComponent<Rigidbody>().velocity.y);
                }
                else
                {
                    direction = -1;
                }

                break;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy Col");
            if (!collision.transform.GetComponent<PlayerController>().isGrounded)
            {
                if (collision.gameObject.name.Remove(collision.gameObject.name.Length - 1) == "Enemy")
                {
                    Debug.Log("Enemy Kill");
                    this.gameObject.GetComponent<Collider>().enabled = false;
                    this.gameObject.GetComponent<Renderer>().enabled = false;
                    GameManager.Instance.score += 500;
                    movingSpeed = 0;
                    transform.position = startPos;
                }
            }
            else
            {
                Debug.Log("Death by enemy");
                collision.transform.GetComponent<PlayerController>().ReSpawn();
            }
        }
    }
}