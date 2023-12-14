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
        ConfigManager.CreateInstance();
        EventManager.CreateInstance();
        FormManager.GetInstance();

        GameObject obj = GameObject.Find("MainCamera");
        gameCamera = obj.GetComponent<Camera>();
    }

    private void Start()
    {
        Debug.Log("GameFrameWork start");
    }

    protected override void OnDestroy()
    {
        Debug.Log("GameFrameWork destroy");
        base.OnDestroy();
        FormManager.DestroyInstance();
        EventManager.DestroyInstance();
        ConfigManager.DestroyInstance();
    }
}
