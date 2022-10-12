using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class TutorialController : MonoBehaviour
{
    public List<string> tutText;
    public TMP_Text tmpText;

    public List<VideoClip> clips;
    public VideoPlayer player;

    public int currentSelected = 0;

    public GameObject buttonPrev;
    public GameObject buttonNext;

    private void Start()
    {
        Change();
    }


    private void Update()
    {
        if (currentSelected == 0)
        {
            buttonPrev.SetActive(false);
        }
        else
        {
            buttonPrev.SetActive(true);
        }

        if (currentSelected == tutText.Count - 1)
        {
            buttonNext.SetActive(false);
        }
        else
        {
            buttonNext.SetActive(true);
        }
    }

    public void Next()
    {
        if (currentSelected < clips.Count)
        {
            currentSelected += 1;
            Change();
        }
    }

    public void Previous()
    {
        if (currentSelected > 0)
        {
            currentSelected -= 1;
            Change();
        }
    }


    public void Change()
    {
        player.clip = clips[currentSelected];
        tmpText.text = tutText[currentSelected];
        player.Play();
    }
}