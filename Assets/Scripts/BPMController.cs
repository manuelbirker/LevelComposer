using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;

public class BPMController : MonoBehaviour
{
    private float secPerBeat;
    private float beatSize;

    private float timeElapsed;
    public Vector3 startPosition;
    private Vector3 startValue;
    private Vector3 endValue;

    public GameObject levelEndMarker;

    public int levelLength;

    void Start()
    {
        secPerBeat = GameManager.Instance.fps / GameManager.Instance.bpm;
        Debug.Log(secPerBeat);

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

        levelLength = GameManager.Instance.levelLength;
        beatSize = GameManager.Instance.beatSize;
        levelEndMarker.transform.position = new Vector3(levelLength, 0, 0);
    }


    void Reset()
    {
        secPerBeat = GameManager.Instance.fps / GameManager.Instance.bpm;

        startValue = transform.position;
        endValue =
            new Vector3(transform.position.x + beatSize, transform.position.y, 0);

        levelEndMarker.transform.position = new Vector3(levelLength, 0, 0);
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
    
        if (col.transform.gameObject.GetComponent<ObjectController>() == null)
        {
            return;
        }

        if (!col.transform.gameObject.CompareTag("Saveable"))
        {
            return;
        }

        if (col.transform.gameObject.GetComponent<ObjectController>().isHeld)
        {
            return;
        }


        if (col.transform.gameObject.GetComponent<ObjectController>().playOnTick != null)
        {
            AudioClip playOnTick = col.transform.gameObject.GetComponent<ObjectController>().playOnTick;
            col.transform.gameObject.GetComponent<AudioSource>().clip = playOnTick;
            col.transform.gameObject.GetComponent<AudioSource>().Play();
            Debug.Log("Play Sound");
        }
    }
}