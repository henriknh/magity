using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{

    public static StorageManager instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void Save()
    {
        Save saveData = new Save();
        saveData.highScore = GameManager.instance.highscore;

        string jsonData = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("Data", jsonData);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        string jsonData = PlayerPrefs.GetString("Data");
        if(jsonData != "")
        {
            Save loadedData = JsonUtility.FromJson<Save>(jsonData);

            GameManager.instance.highscore = loadedData.highScore;
        }
        
    }
}