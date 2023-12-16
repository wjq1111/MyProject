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

    protected override void Awake()
    {
        base.Awake();

        AllocGid.CreateInstance();
        ConfigManager.CreateInstance();
        EventManager.CreateInstance();
        FormManager.GetInstance();

        GameObject obj = GameObject.Find("MainCamera");
        gameCamera = obj.GetComponent<Camera>();
    }

    private void Start()
    {

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        FormManager.DestroyInstance();
        EventManager.DestroyInstance();
        ConfigManager.DestroyInstance();
        AllocGid.DestroyInstance();
    }
}
