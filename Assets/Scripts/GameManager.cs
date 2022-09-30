using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public int bpm = 128;
    public float fps = 60;
    public int levelLength = 20;
    public int beatSize = 4;

    // Settings
    public string levelName;
    public TMP_InputField levelNameInput;
    public TMP_InputField levelLengthInput;
    public GameObject settingsUI;
    public bool settingsOpen;

    public GameObject editorUI;

    public BPMController bpmController;

    private static GameManager _instance;

    public static GameManager Instance
    {
        get { return _instance; }
    }


    public enum GameState
    {
        MainMenu,
        Editor,
        PlayTest,
        LevelSelect,
        PlayLevel,
        Tutorial,
        Controls,
        Credits
    }

    public GameState _gameState;


    public GameObject playTestButton;
    public GameObject stopPlayTestButton;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(gameObject);

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = (int)Mathf.Round(fps);
    }


    public void PlayTest()
    {
        _gameState = GameState.PlayTest;
        bpmController.Reset();
        playTestButton.SetActive(false);
        stopPlayTestButton.SetActive(true);


        // TODO Change Play Button To Stop Button
        // TODO Deactivate Editor Interface and functions
        // TODO Stop Playtest when esc is pressed
        // TODO Reset Player Position
    }


    public void StopPlayTest()
    {
        _gameState = GameState.Editor;
        bpmController.Reset();
        playTestButton.SetActive(true);
        stopPlayTestButton.SetActive(false);
        bpmController.Reset();

        // TODO Change Play Button back to Play Button
        // TODO Activate Editor Interface and functions
        // TODO Reset Player Position
    }


    private void Start()
    {
        if (_gameState == GameState.Editor)
        {
            levelName = "NewLevel_" + Random.Range(0, 9999);
            levelNameInput.text = levelName;
        }

        bpmController.levelLength = levelLength;
        bpmController.Reset();
    }

    public void OpenSettings()
    {
        settingsOpen = true;
        settingsUI.SetActive(true);
        editorUI.SetActive(false);

        levelLengthInput.text = levelLength.ToString();
        levelNameInput.text = levelName;
    }

    public void CancelSettings()
    {
        settingsOpen = false;
        settingsUI.SetActive(false);
        editorUI.SetActive(true);
    }

    public void SaveSettings()
    {
        settingsOpen = false;
        levelName = levelNameInput.text;
        levelLength = int.Parse(levelLengthInput.text);
        bpmController.levelLength = levelLength;
        settingsUI.SetActive(false);
        editorUI.SetActive(true);
        bpmController.Reset();
    }
}