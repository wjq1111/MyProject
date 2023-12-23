using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFramework : MonoSingleton<GameFramework>
{
    private Camera gameCamera;
    public Camera MainCamera
    {
        get { return gameCamera; }
    }

    private GameConf gameConf;

    public GameConf GameConf
    {
        get { return gameConf; }
    }

    protected override void Awake()
    {
        base.Awake();
        gameConf = Resources.Load<GameConf>("GameConf");

        AllocGid.CreateInstance();
        GameFsm.CreateInstance();
        ConfigManager.CreateInstance();
        EventManager.CreateInstance();
        PoolManager.GetInstance();
        FormManager.GetInstance();
        Gamecore.CreateInstance();

        GameObject obj = GameObject.Find("MainCamera");
        gameCamera = obj.GetComponent<Camera>();
    }

    private void Start()
    {

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Gamecore.DestroyInstance();
        FormManager.DestroyInstance();
        PoolManager.DestroyInstance();
        EventManager.DestroyInstance();
        ConfigManager.DestroyInstance();
        GameFsm.DestroyInstance();
        AllocGid.DestroyInstance();
    }
}
