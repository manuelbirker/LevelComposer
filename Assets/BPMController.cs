using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;

public class BPMController : MonoBehaviour
{
    public float secPerBeat;
    public float beatSize = 1f;

    public float timeElapsed;
    public Vector3 startPosition;
    private Vector3 startValue;
    private Vector3 endValue;

    public int levelLength = 20;

    void Start()
    {
        secPerBeat = GameManager.Instance.fps / GameManager.Instance.bpm;

        startValue = transform.position;

        endValue =
            new Vector3(transform.position.x + beatSize, transform.position.y, 0);
    }

    private void Update()
    {
        // Loop
        if (transform.position.x > levelLength)
        {
            transform.position = startPosition;
        }
    }


    void Reset()
    {
        secPerBeat = GameManager.Instance.fps / GameManager.Instance.bpm;

        startValue = transform.position;
        endValue =
            new Vector3(transform.position.x + beatSize, transform.position.y, 0);
    }


    void FixedUpdate()
    {
        if (timeElapsed < secPerBeat)
        {
            transform.position = Vector3.Lerp(
                startValue,
                endValue,
                timeElapsed / secPerBeat);

            timeElapsed += Time.deltaTime;
        }
        else
        {
            startValue = transform.position;
            endValue =
                new Vector3(transform.position.x + beatSize, transform.position.y, 0);
            timeElapsed = 0;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("Play Sound");
        col.transform.gameObject.GetComponent<AudioSource>().Play();
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Play Sound");
        col.transform.gameObject.GetComponent<AudioSource>().Play();
    }
}