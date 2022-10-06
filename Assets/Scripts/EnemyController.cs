using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using Object = UnityEngine.Object;

public class EnemyController : MonoBehaviour
{
    public float movingSpeed = 1f;

    public float _movingSpeed;
    public Vector3 startPos;
    public ObjectController oC;
    public bool activated = false;
    public bool killed = false;
    public GameObject skin;

    public float distance;
    public float distanceActive;
    public Transform player;
    public bool effectActive;


    public AudioMixer mixer;

    private void Awake()
    {
        oC = GetComponent<ObjectController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!activated)
        {
            if (!oC.isHeld)
            {
                startPos = transform.position;
                _movingSpeed = movingSpeed;
            }

            activated = true;
        }


        if (GameManager.Instance._gameState != GameManager.GameState.PlayLevel &&
            GameManager.Instance._gameState != GameManager.GameState.PlayTest)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            if (killed)
            {
                GetComponent<Rigidbody>().isKinematic = true;
            }
            else
            {
                GetComponent<Rigidbody>().isKinematic = false;
            }
        }

        if (GameManager.Instance._player)
        {
            player = GameObject.Find("Player").transform;


            distance = Vector3.Distance(transform.position, player.position);


            if (killed)
            {
                return;
            }

            if (distance < distanceActive)
            {
                effectActive = true;
            }
            else
            {
                effectActive = false;
            }
        }

        if (this.gameObject.name == "Enemy1")
        {
            if (effectActive)
            {
                Camera.main.GetComponent<AudioHighPassFilter>().enabled = true;
            }
            else
            {
                Camera.main.GetComponent<AudioHighPassFilter>().enabled = false;
            }
        }

        if (this.gameObject.name == "Enemy2")
        {
            if (effectActive)
            {
                Camera.main.GetComponent<AudioLowPassFilter>().enabled = true;
            }
            else
            {
                Camera.main.GetComponent<AudioLowPassFilter>().enabled = false;
            }
        }

        // TODO Enemy Movement
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.CompareTag("Player"))
        {
            if (!collision.transform.GetComponent<PlayerController>().isGrounded)
            {
                if (gameObject.name.Remove(gameObject.name.Length - 1) == "Enemy")
                {
                    Debug.Log("Enemy Kill");
                    this.gameObject.GetComponent<Collider>().enabled = false;
                    GameManager.Instance.score += 500;
                    movingSpeed = 0;
                    transform.position = startPos;
                    GetComponent<Rigidbody>().isKinematic = true;
                    killed = true;
                    GetComponent<EnemyController>().skin.SetActive(false);
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