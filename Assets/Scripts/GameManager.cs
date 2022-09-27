using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int bpm = 128;
    public float fps = 60;
    public int levelLength = 20;
    public int beatSize = 4;

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
        bpmController.Reset();
        playTestButton.SetActive(true);
        stopPlayTestButton.SetActive(false);
        
        // TODO Change Play Button back to Play Button
        // TODO Activate Editor Interface and functions
        // TODO Reset Player Position
    }
}