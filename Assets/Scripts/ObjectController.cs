using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectController : MonoBehaviour
{
    public bool isHeld;
    private Quaternion targetAngle;
    public bool canWobble = true;
    private Vector3 startAngle;

    public AudioClip playOnTick;
    public AudioClip playOnTouch;

    public string toolTip = "";

    public float startVolume;
    public float endVolume;

    public void Wobble()
    {
        InvokeRepeating("WobbleTarget", 0, 0.5f);
    }

    private void Start()
    {
        startAngle = this.transform.rotation.eulerAngles;

        startVolume = GetComponent<AudioSource>().volume;
        endVolume = 0;
    }

    private void Update()
    {
        if (canWobble)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetAngle, Time.deltaTime);
        }


        if (isHeld)
        {
            transform.gameObject.tag = "notSaveable";
        }
        else
        {
            transform.gameObject.tag = "Saveable";
        }

        if (!GetComponent<AudioSource>().isPlaying)
        {
            CancelInvoke("WobbleTarget");
            transform.rotation = quaternion.Euler(startAngle);
        }
    }

    void WobbleTarget()
    {
        float intensity = Random.Range(0.1f, 15);
        float curve = Mathf.Sin((Random.Range(0, Mathf.PI * 2)));
        targetAngle = Quaternion.Euler(Vector3.forward * curve * intensity);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.CompareTag("Ticker"))
        {
            if (!isHeld)
            {
                Wobble();
            }
        }


        if (other.transform.gameObject.CompareTag("Player"))
        {
            if (playOnTouch != null)
            {
                GetComponent<AudioSource>().clip = playOnTouch;
                GetComponent<AudioSource>().Play();


                if (this.gameObject.name == "Coin")
                {
                    this.gameObject.GetComponent<Collider>().enabled = false;
                    this.gameObject.GetComponent<Renderer>().enabled = false;
                    GameManager.Instance.score += 100;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (this.gameObject.name == "Rain")
        {
            Camera.main.GetComponent<AudioEchoFilter>().enabled = true;
        }

        if (this.gameObject.name == "Water")
        {
            Camera.main.GetComponent<AudioReverbFilter>().enabled = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (this.gameObject.name == "Rain")
        {
            Camera.main.GetComponent<AudioEchoFilter>().enabled = false;
        }


        if (this.gameObject.name == "Water")
        {
            Camera.main.GetComponent<AudioReverbFilter>().enabled = false;
        }
    }
}