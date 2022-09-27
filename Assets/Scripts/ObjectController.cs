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


    public void Wobble()
    {
        InvokeRepeating("WobbleTarget", 0, 0.5f);
    }

    private void Start()
    {
        startAngle = this.transform.rotation.eulerAngles;
    }

    private void Update()
    {
        if (canWobble)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetAngle, Time.deltaTime);
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
            Wobble();
        }
    }
}