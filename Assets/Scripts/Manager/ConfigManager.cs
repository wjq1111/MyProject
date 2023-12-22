using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Config
{
    public int Id;
}

public class ConfigManager : Singleton<ConfigManager>
{
    // Â·¾¶
    private static string streamingAssetsPath = "Assets/StreamingAssets/Config/";

    // form
    private string startGameFormConfigPath = streamingAssetsPath + "StartGameFormConfig.json";

    // database
    private string playerCardBagDatabasePath = streamingAssetsPath + "PlayerCardBagDataBase.json";
    private string cardManualDatabasePath = streamingAssetsPath + "CardManualDataBase.json";
    private string attributeCardManualDatabasePath = streamingAssetsPath + "AttributeCardManualDatabase.json";
    private string monsterCardManualDatabasePath = streamingAssetsPath + "MonsterCardManualDatabase.json";

    // database
    private PlayerCardBagDatabase playerCardBagDatabase;
    private CardManualDatabase cardManualDatabase;
    private AttributeCardManualDatabase attributeCardManualDatabase;
    private MonsterCardManualDatabase monsterCardManualDatabase;


    // ÅÆµÄÍ¼¼ø
    public Dictionary<int, CardManual> cardManualMap;
    // ³õÊ¼ÅÆ¿â
    public Dictionary<int, PlayerCardBag> playerCardBagMap;
    // ÊôÐÔ¿¨Í¼¼ø
    public Dictionary<int, AttributeCardManual> attributeCardManualMap;
    // ¹ÖÎï¿¨Í¼¼ø
    public Dictionary<int, MonsterCardManual> monsterCardManualMap;

    public override void Init()
    {
        LoadJson();
        // Debug.Log("Load Config done");
    }

    private void LoadJson()
    {
        playerCardBagDatabase = LoadCfgFromJSON<PlayerCardBagDatabase>(playerCardBagDatabasePath);
        cardManualDatabase = LoadCfgFromJSON<CardManualDatabase>(cardManualDatabasePath);
        attributeCardManualDatabase = LoadCfgFromJSON<AttributeCardManualDatabase>(attributeCardManualDatabasePath);
        monsterCardManualDatabase = LoadCfgFromJSON<MonsterCardManualDatabase>(monsterCardManualDatabasePath);

        cardManualMap = Convert(cardManualDatabase.info);
        playerCardBagMap = Convert(playerCardBagDatabase.info);
        attributeCardManualMap = Convert(attributeCardManualDatabase.info);
        monsterCardManualMap = Convert(monsterCardManualDatabase.info);
    }

    private T LoadCfgFromJSON<T>(string path)
    {
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(json);
    }

    private Dictionary<int, T> Convert<T>(List<T> database) where T : Config
    {
        Dictionary<int, T> dict = new Dictionary<int, T>();
        foreach (var data in database)
        {
            dict.Add(data.Id, data);
        }
        return dict;
    }
}