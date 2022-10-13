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

    public const string BaseFolder = "Levels";


    public static string GetBasePath()
    {
        Debug.Log(Application.persistentDataPath);
        string path = Application.dataPath + $"/{BaseFolder}/";
        string path1 = Application.dataPath + $"/{BaseFolder}";
        if (!Directory.Exists(path1)) Directory.CreateDirectory(path1);
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        Debug.Log(path);
        return path;
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
        string filePath = GetBasePath();

        foreach (string file in System.IO.Directory.GetFiles(filePath))
        {
            string[] _file = file.Split("/");

            string __file = _file[_file.Length - 1];

            string ___file = __file.Replace(".txt", "");


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
        PlayerPrefs.DeleteAll();
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