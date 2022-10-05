using System;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public int bpm = 128;
    public float fps = 60;
    public int levelLength = 20;
    public int beatSize = 4;
    public int ambianceID = 0;

    // Settings
    public string levelName;
    public TMP_InputField levelNameInput;
    public TMP_InputField levelLengthInput;
    public GameObject settingsUI;
    public bool settingsOpen;
    public TMP_Dropdown ambiance;
    public AmbianceController ambController;

    public GameObject editorUI;

    public BPMController bpmController;

    private static GameManager _instance;

    public ToolTipController ttip;


    public int goalCount = 0;
    public int startCount = 0;


    public GameObject playerPrefab;
    public GameObject _player;

    public static GameManager Instance
    {
        get { return _instance; }
    }


    public enum GameState
    {
        Editor,
        PlayTest,
        PlayLevel,
    }

    public GameState _gameState;

    public GameObject playTestButton;
    public GameObject stopPlayTestButton;


    public GameObject start;
    public GameObject goal;

    public CinemachineVirtualCamera vcam;
    public GameObject editorMover;

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
        if (startCount == 0)
        {
            ttip.ChangeToolTip("There is no startposition in your Level!");
            return;
        }

        // editorUI.SetActive(false);
        start = GameObject.Find("Start");
        goal = GameObject.Find("Goal");


        if (goal != null)
        {
            goal.SetActive(false);
        }

        if (start != null)
        {
            start.SetActive(false);
        }


        _player = Instantiate(playerPrefab);
        _player.transform.position = start.transform.position;


        _player.transform.rotation = Quaternion.Euler(0, 90, 0);


        vcam.Follow = _player.transform;
        _gameState = GameState.PlayTest;
        bpmController.Reset();
        playTestButton.SetActive(false);
        stopPlayTestButton.SetActive(true);

        if (ambianceID != 0)
        {
            ambController.Play();
        }
    }


    public void StopPlayTest()
    {
        if (goal != null)
        {
            goal.SetActive(true);
            goal = null;
        }

        if (start != null)
        {
            start.SetActive(true);
            start = null;
        }

        // editorUI.SetActive(true);
        vcam.Follow = editorMover.transform;
        _gameState = GameState.Editor;
        bpmController.Reset();
        playTestButton.SetActive(true);
        stopPlayTestButton.SetActive(false);
        bpmController.Reset();
        ambController.Stop();

        Destroy(_player);


        foreach (GameObject gameObj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (gameObj.name == "Coin")
            {
                gameObj.gameObject.GetComponent<Collider>().enabled = true;
                gameObj.gameObject.GetComponent<Renderer>().enabled = true;
            }
        }
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
        ambiance.value = ambianceID;
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
        ambianceID = ambiance.value;
        ambController.Change(ambianceID);
    }


    public void SaveSettingsAfterLoad()
    {
        settingsOpen = false;
        ambController.Change(ambianceID);
        settingsUI.SetActive(false);
        editorUI.SetActive(true);
        bpmController.levelLength = levelLength;
        bpmController.Reset();
        editorMover.transform.position = new Vector3(0, 0, 0);
    }
}