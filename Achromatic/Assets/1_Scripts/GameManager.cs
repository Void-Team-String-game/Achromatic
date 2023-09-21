using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager GetInstance()
    {
        return instance;
    }

    void Init()
    {
        if (instance == null)
        {
            GameObject go = GameObject.Find("GameSystem");
            if (go == null)
            {
                go = new GameObject { name = "GameSystem"};
            }

            if (go.GetComponent<GameManager>() == null)
            {
                go.AddComponent<GameManager>();
            }
            DontDestroyOnLoad(go);
            instance = go.GetComponent<GameManager>();
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SaveByJson<T>(string filePath, string fileName, T obj)
    {
        var fileStream = new FileStream($"{filePath}/{fileName}", FileMode.Create);
        var jsonData = JsonUtility.ToJson(obj);
        var data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }
}
