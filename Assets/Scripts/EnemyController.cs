using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float boundariesMinX = 5.0f;
    public float boundariesMaxX = 5.0f;
    public int direction = -1;
    public float movingSpeed = 1f;


    // Update is called once per frame
    void Update()
    {
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
}