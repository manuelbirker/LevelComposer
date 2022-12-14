using System;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public CinemachineVirtualCamera vcam_game;
    public GameObject editorMover;


    public GameObject ingameUI;
    public int score = 0;
    public TMP_Text scoreText;
    public TMP_Text win_scoreText;
    public GameObject editorObjects;

    public GameObject deadZones;

    public LevelManager lm;


    public GameObject winUI;
    public GameObject loseUI;

    public void ClearLevel()
    {
        GameObject[] assetsToSave = GameObject.FindGameObjectsWithTag("Saveable");

        if (assetsToSave != null)
        {
            for (int i = 0; i < assetsToSave.Length; i++)
            {
                Destroy(assetsToSave[i]);
            }
        }


        levelLength = 15;
        ambianceID = 0;
        levelName = "NewLevel_" + Random.Range(0, 99999);

        ResetLevel();

        settingsOpen = false;
        settingsUI.SetActive(false);
        editorUI.SetActive(true);


        lm.ResetLevel();
    }


    public void PlayerWins()
    {
        winUI.SetActive(true);

        editorUI.SetActive(false);
        ingameUI.SetActive(false);

        settingsUI.SetActive(false);
    }

    public void PlayerLoses()
    {
        loseUI.SetActive(true);


        editorUI.SetActive(false);
        ingameUI.SetActive(false);

        settingsUI.SetActive(false);
    }


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


        if (start != null)
        {
            start.SetActive(false);
        }


        _player = Instantiate(playerPrefab);
        _player.name = "Player";
        _player.transform.position = start.transform.position;


        _player.transform.rotation = Quaternion.Euler(0, 90, 0);


        vcam.Follow = _player.transform;
        vcam_game.Follow = _player.transform;
        vcam.enabled = false;
        vcam_game.enabled = true;
        deadZones.SetActive(false);

        _gameState = GameState.PlayTest;
        bpmController.Reset();
        playTestButton.SetActive(false);
        stopPlayTestButton.SetActive(true);

        if (ambianceID != 0)
        {
            ambController.Play();
        }
    }

    private void Update()
    {
        scoreText.text = "Score: " + score.ToString();
        win_scoreText.text = "Score: " + score.ToString();
    }

    public void PlayMode()
    {
        editorUI.SetActive(false);
        settingsUI.SetActive(false);

        start = GameObject.Find("Start");
        goal = GameObject.Find("Goal");


        if (start != null)
        {
            start.SetActive(false);
        }


        _player = Instantiate(playerPrefab);
        _player.transform.position = start.transform.position;


        _player.transform.rotation = Quaternion.Euler(0, 90, 0);


        vcam.Follow = _player.transform;
        vcam_game.Follow = _player.transform;
        vcam.enabled = false;
        vcam_game.enabled = true;


        _gameState = GameState.PlayLevel;
        bpmController.Reset();


        editorObjects.SetActive(false);
        bpmController.transform.gameObject.GetComponent<Renderer>().enabled = false;


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
        vcam_game.Follow = editorMover.transform;
        vcam.enabled = true;
        vcam_game.enabled = false;


        _gameState = GameState.Editor;
        bpmController.Reset();
        playTestButton.SetActive(true);
        stopPlayTestButton.SetActive(false);
        bpmController.Reset();
        ambController.Stop();
        deadZones.SetActive(true);
        Destroy(_player);


        foreach (GameObject gameObj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (gameObj.name == "Coin")
            {
                gameObj.gameObject.GetComponent<Collider>().enabled = true;
                gameObj.gameObject.GetComponent<Renderer>().enabled = true;
            }

            if (gameObj.gameObject.name.Remove(gameObj.gameObject.name.Length - 1) == "Enemy")
            {
                gameObj.gameObject.GetComponent<EnemyController>().skin.SetActive(true);
                gameObj.gameObject.GetComponent<Collider>().enabled = true;
                gameObj.gameObject.GetComponent<EnemyController>().movingSpeed =
                    gameObj.gameObject.GetComponent<EnemyController>()._movingSpeed;
                gameObj.gameObject.GetComponent<EnemyController>().killed = false;
                gameObj.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                //gameObj.gameObject.transform.position = gameObj.gameObject.GetComponent<EnemyController>().startPos;
            }
        }
    }

    public void ResetLevel()
    {
        bpmController.Reset();
        playTestButton.SetActive(true);
        stopPlayTestButton.SetActive(false);
        bpmController.Reset();
        ambController.Stop();
        deadZones.SetActive(true);
        Destroy(_player);


        foreach (GameObject gameObj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (gameObj.name == "Coin")
            {
                gameObj.gameObject.GetComponent<Collider>().enabled = true;
                gameObj.gameObject.GetComponent<Renderer>().enabled = true;
            }

            if (gameObj.gameObject.name.Remove(gameObj.gameObject.name.Length - 1) == "Enemy")
            {
                gameObj.gameObject.GetComponent<EnemyController>().skin.SetActive(true);
                gameObj.gameObject.GetComponent<Collider>().enabled = true;
                gameObj.gameObject.GetComponent<EnemyController>().movingSpeed =
                    gameObj.gameObject.GetComponent<EnemyController>()._movingSpeed;
                gameObj.gameObject.GetComponent<EnemyController>().killed = false;
                gameObj.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObj.gameObject.transform.position = gameObj.gameObject.GetComponent<EnemyController>().startPos;
            }
        }
        
        
        Camera.main.GetComponent<AudioHighPassFilter>().enabled = false;
        Camera.main.GetComponent<AudioLowPassFilter>().enabled = false;
        
        
        
    }


    private void Start()
    {
        if (PlayerPrefs.GetString("whichOption") == "Editor")
        {
            _gameState = GameState.Editor;
        }

        else if (PlayerPrefs.GetString("whichOption") == "PlayLevel")
        {
            _gameState = GameState.PlayLevel;
        }
  

        if (_gameState == GameState.Editor)
        {
            levelName = "NewLevel_" + Random.Range(0, 99999);
            levelNameInput.text = levelName;
        }
        else if (_gameState == GameState.PlayLevel)
        {
            levelName = PlayerPrefs.GetString("whichLevel");
            lm.loadLevelName = levelName;


            lm.LoadLevel();

            PlayMode();
        }



        if (_gameState == GameState.PlayLevel)
        {
            ingameUI.SetActive(true);
            editorUI.SetActive(false);
        }

        if (_gameState == GameState.Editor)
        {
            ingameUI.SetActive(false);
            editorUI.SetActive(true);
        }
        
        

        score = 0;
        bpmController.levelLength = levelLength;
        bpmController.Reset();
    }

    public void ExitGame()
    {
        ClearLevel();


        SceneManager.LoadScene("MainMenu");
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