using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;

public class LevelManager : MonoBehaviour
{
    public string saveableAssetTag = "Saveable";
    public GameObject[] assetsToSave;
    public string[] assetNames;
    public Vector3[] assetPositions;
    public GameObject[] possibleObjects;

    public string loadLevelName;
    public TMP_InputField loadLevelInputField;

    public ToolTipController ttip;

    private void Update()
    {
        if (loadLevelInputField.text.Length > 0)
        {
            loadLevelName = loadLevelInputField.text;
        }
    }

    public void FindSaveableAssets()
    {
        if (assetsToSave != null)
        {
            for (int i = 0; i < assetsToSave.Length; i++)
            {
                Destroy(assetsToSave[i]);
            }
        }


        assetsToSave = GameObject.FindGameObjectsWithTag(saveableAssetTag);

        assetNames = new string[assetsToSave.Length];
        assetPositions = new Vector3[assetsToSave.Length];

        for (int j = 0; j < assetsToSave.Length; j++)
        {
            assetNames[j] = assetsToSave[j].name;
            assetPositions[j] = assetsToSave[j].transform.position;
        }

        SaveToFile();
    }


    public void SaveToFile()
    {
        string filePath = "Assets/Levels/" + GameManager.Instance.levelName + ".txt";
        StreamWriter writer = new StreamWriter(filePath, false);


        string info2 = "levelName:" + "#/#" + GameManager.Instance.levelName;
        writer.WriteLine(info2);

        info2 = "levelLength:" + "#/#" + GameManager.Instance.levelLength;
        writer.WriteLine(info2);

        info2 = "ambiance:" + "#/#" + GameManager.Instance.ambianceID;
        writer.WriteLine(info2);


        for (int i = 0; i < assetsToSave.Length; i++)
        {
            string info = "object:" + "#/#" + assetNames[i] + "#/#" + assetPositions[i].x.ToString() + "#/#" +
                          assetPositions[i].y.ToString() +
                          "#/#" +
                          assetPositions[i].z.ToString();

            writer.WriteLine(info);
        }


        writer.Close();


        ttip.ChangeToolTip("Level saved!");
    }


    public void LoadLevel()
    {
        foreach (GameObject saveableObject in GameObject.FindGameObjectsWithTag("Saveable"))
        {
            Destroy(saveableObject);
        }

        string filePath = "Assets/Levels/" + loadLevelName + ".txt";

  
        StreamReader reader = new StreamReader(filePath);
        
        
        
        
        int numOfLine = 0;

        while (reader.ReadLine() != null)
        {
            numOfLine++;
        }

        assetNames = new string[0];
        assetPositions = new Vector3[0];

        assetNames = new string[numOfLine];
        assetPositions = new Vector3[numOfLine];

        reader.Close();

        StreamReader reader2 = new StreamReader(filePath);

        while (!reader2.EndOfStream)
        {
            for (int i = 0; i < numOfLine; i++)
            {
                string[] data = reader2.ReadLine().Split("#/#");

                if (data[0] == "levelName:")
                {
                    GameManager.Instance.levelName = data[1];
                }

                if (data[0] == "levelLength:")
                {
                    GameManager.Instance.levelLength = int.Parse(data[1]);
                }

                if (data[0] == "ambiance:")
                {
                    GameManager.Instance.ambianceID = int.Parse(data[1]);
                }


                if (data[0] == "object:")
                {
                    assetNames[i] = data[1];
                    assetPositions[i].x = float.Parse(data[2]);
                    assetPositions[i].y = float.Parse(data[3]);
                    assetPositions[i].z = float.Parse(data[4]);
                }
            }
        }

        reader.Close();

        CreateAssets();
    }


    public void CreateAssets()
    {
        for (int i = 0; i < assetNames.Length; i++)
        {
            for (int j = 0; j < possibleObjects.Length; j++)
            {
                if (possibleObjects[j].name == assetNames[i])
                {
                    GameObject spawn = Instantiate(possibleObjects[j], assetPositions[i], Quaternion.identity);

                    spawn.name = spawn.name.Replace("(Clone)", "").Trim();
                }
            }
        }


        GameManager.Instance.SaveSettingsAfterLoad();


        if (GameObject.Find("Start"))
        {
            GameManager.Instance.startCount = 1;
        }
        else
        {
            GameManager.Instance.startCount = 1;
        }

        if (GameObject.Find("Goal"))
        {
            GameManager.Instance.goalCount = 0;
        }
        else
        {
            GameManager.Instance.goalCount = 0;
        }


        ttip.ChangeToolTip("Level loaded!");
    }
}