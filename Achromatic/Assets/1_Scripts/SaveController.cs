using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string name;
    public float bestscore;

    private SaveData(SavaDataBuilder savaDataBuilder)
    {
        name = savaDataBuilder.name;
        bestscore = savaDataBuilder.bestscore;
    }

    public class SavaDataBuilder
    {
        public string name;
        public float bestscore;
        public SavaDataBuilder Name(string name)
        {
            this.name = name;
            return this;
        }
        public SavaDataBuilder Bestscore(float bestscore)
        {
            this.bestscore = bestscore;
            return this;
        }
        public SaveData Build()
        {
            return new SaveData(this);
        }
    }
}

public static class SaveSystem // save location -> C:\Users\{username}\AppData\LocalLow\{addition_location}
{
    public static string SavePath => Application.persistentDataPath + "/saves/";
    private static string encryptionCodeWord = "Achromatic";
    // private static bool useEncryption = true;


    public static void Save(SaveData saveData, string saveFileName, bool useEncryption)
    {
        if (!Directory.Exists(SavePath)) // 존재하지 않으면 새로 만들어준다.
        {
            Directory.CreateDirectory(SavePath);
        }

        string saveJson = JsonUtility.ToJson(saveData);
        if (useEncryption)
        {
            saveJson = EncryptDecrypt(saveJson);
        }

        string saveFilePath = SavePath + saveFileName + ".json";

        File.WriteAllText(saveFilePath, saveJson);
        Debug.Log("Save Success: " + saveFilePath);
    }

    public static SaveData Load(string saveFileName, bool useEncryption)
    {
        string saveFilePath = SavePath + saveFileName + ".json";

        if (!File.Exists(saveFilePath))
        {
            Debug.LogError("No such saveFile exists");
            return null;
        }

        string saveFile = File.ReadAllText(saveFilePath);
        if (useEncryption)
        {
            saveFile = EncryptDecrypt(saveFile);
        }
        SaveData saveData = JsonUtility.FromJson<SaveData>(saveFile);
        return saveData;
    }

    private static string EncryptDecrypt(string saveJson)
    {
        string modified = "";
        for (int i = 0; i < saveJson.Length; i++)
        {
            modified += (char)(saveJson[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }

        return modified;
    }
}

public class SaveController : MonoBehaviour
{
    [Header("Notice: If doesn't work, change encryption mode")]
    [SerializeField]
    private bool useEncryption = true;
    void Start()
    {
        if (!File.Exists(SaveSystem.SavePath + "playerinfo" + ".json"))
        {
            Save();
        }
        Load();
    }

    /*
    void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            Save();
        }
        if (Input.GetKeyDown("l"))
        {
            Load();
        }
    }
    */

    public void Save()
    {
        SaveData character = new SaveData.SavaDataBuilder()
            .Name(UserData.name)
            .Bestscore(UserData.bestscore)
            .Build();

        SaveSystem.Save(character, "playerinfo", useEncryption);
    }

    public void Load()
    {
        SaveData loadData = SaveSystem.Load("playerinfo", useEncryption);
        if (loadData != null)
        {
            Debug.Log(string.Format("LoadData Result => name : {0}, bestscore : {1} ", loadData.name, loadData.bestscore));
            UserData.name = loadData.name;
            UserData.bestscore = loadData.bestscore;
        }
    }
}