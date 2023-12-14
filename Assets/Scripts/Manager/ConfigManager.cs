using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConfigManager : Singleton<ConfigManager>
{
    private string startGameFormConfigPath = "Assets/StreamingAssets/Config/StartGameFormConfig.json";

    public StartGameFormConfig startGameFormConfig;

    public override void Init()
    {
        LoadJson();
        PrintConfig();
        Debug.Log("Load Config done");
    }

    private void LoadJson()
    {
        startGameFormConfig = LoadCfgFromJSON<StartGameFormConfig>(startGameFormConfigPath);
    }

    private T LoadCfgFromJSON<T>(string path)
    {
        Debug.Log(path);
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(json);
    }

    private void PrintConfig()
    {
        startGameFormConfig.Print();
    }
}