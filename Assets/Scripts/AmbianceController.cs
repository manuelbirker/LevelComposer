using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbianceController : MonoBehaviour
{
    public List<AudioClip> ambianceSounds;


    public void Play()
    {
        this.GetComponent<AudioSource>().Play();
    }


    public void Stop()
    {
        this.GetComponent<AudioSource>().Stop();
    }


    public void Change(int id)
    {
        this.GetComponent<AudioSource>().clip = ambianceSounds[id];
    }
}