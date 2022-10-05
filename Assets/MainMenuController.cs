using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject loadLevelUI;
    public GameObject tutorialUI;
    public GameObject creditsUI;


    public string gameVersion = "1.0";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    public void PlayLevel()
    {
    }

    public void LoadEditor()
    {
    }

    public void HowToPlay()
    {
    }

    public void Credits()
    {
    }

    public void Exit()
    {
        Application.Quit();
    }


    public void LoadLevel()
    {
    }
}