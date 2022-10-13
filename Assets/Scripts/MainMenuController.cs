using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Net.Mime;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject loadLevelUI;
    public GameObject tutorialUI;
    public GameObject creditsUI;


    public string gameVersion = "1.0";
    public string selectedLevel;

    public TMP_Dropdown levels;

    public string whichOption;

    private void Awake()
    {
    }


    public void ChangeSelectedLevel(string lvl)
    {
        selectedLevel = levels.options[levels.value].text;
    }

    public void PlayLevel()
    {
        mainMenuUI.SetActive(false);

        loadLevelUI.SetActive(true);
        tutorialUI.SetActive(false);
        creditsUI.SetActive(false);

        levels.ClearOptions();
        string filePath = "Assets/Levels/";

        foreach (string file in System.IO.Directory.GetFiles(filePath))
        {
            string[] _file = file.Split("/");
            string[] __file = _file[2].Split(".");
            string ___file = __file[0];

            levels.options.Add(new TMP_Dropdown.OptionData(___file));
        }
    }


    public void LoadEditor()
    {
        whichOption = "Editor";
        selectedLevel = "";
        PlayerPrefs.SetString("whichOption", whichOption);
        PlayerPrefs.SetString("whichLevel", "");
        SceneManager.LoadScene("Editor");

        // TODO Load Editor
    }

    public void HowToPlay()
    {
        mainMenuUI.SetActive(false);
        loadLevelUI.SetActive(false);
        tutorialUI.SetActive(true);
        creditsUI.SetActive(false);
    }

    public void Credits()
    {
        mainMenuUI.SetActive(false);
        loadLevelUI.SetActive(false);
        tutorialUI.SetActive(false);
        creditsUI.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void Start()

    {
 
        PlayerPrefs.SetString("whichOption", "");
        PlayerPrefs.SetString("whichLevel", "");
    }

    public void LoadLevel()
    {
        whichOption = "PlayLevel";
        PlayerPrefs.SetString("whichOption", whichOption);
        PlayerPrefs.SetString("whichLevel", selectedLevel);
        SceneManager.LoadScene("Editor");
    }

    public void ExitToMenu()
    {
        loadLevelUI.SetActive(false);
        tutorialUI.SetActive(false);
        creditsUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }
}