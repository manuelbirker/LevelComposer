using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Net.Mime;
using UnityEngine.SceneManagement;

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
        DontDestroyOnLoad(gameObject);
    }


    public void ChangeSelectedLevel(string lvl)
    {
        selectedLevel = lvl;
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


    public void LoadLevel()
    {
        whichOption = "PlayLevel";
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