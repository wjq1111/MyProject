using System;
using UnityEngine;

public class AutoSingletonAttribute : Attribute
{
    public bool bAutoCreate;

    public AutoSingletonAttribute(bool bCreate)
    {
        bAutoCreate = bCreate;
    }
}

[AutoSingleton(true)]
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T s_instance;
    private static bool s_destroyed;
    public static T Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = GetInstance();
            }
            return s_instance;
        }
    }

    protected static T GetInstance()
    {
        if (s_instance == null && !s_destroyed)
        {
            s_instance = (T)FindObjectOfType(typeof(T));

            if (s_instance == null)
            {
                object[] attributes = typeof(T).GetCustomAttributes(typeof(AutoSingletonAttribute), true);
                if (attributes.Length > 0)
                {
                    var bAutoCreate = ((AutoSingletonAttribute)attributes[0]).bAutoCreate;
                    if (!bAutoCreate)
                    {
                        return null;
                    }
                }

                GameObject singleton = new GameObject("[Singleton]" + typeof(T).Name);
                if (singleton != null)
                {
                    s_instance = singleton.AddComponent<T>();
                    s_instance.Init();
                }

                GameObject bootObject = GameObject.Find("Boot");
                if (bootObject != null)
                {
                    singleton.transform.SetParent(bootObject.transform);
                }
            }
        }
        return s_instance;
    }

    public static void DestroyInstance()
    {
        if (s_instance != null)
        {
            Destroy(s_instance.gameObject);
        }

        s_destroyed = true;
        s_instance = null;
    }

    protected virtual void Awake()
    {
        if (s_instance != null && s_instance.gameObject != gameObject)
        {
            if (Application.isPlaying)
            {
                Destroy(gameObject);
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }
        else if (s_instance == null)
        {
            s_instance = GetComponent<T>();
        }

        DontDestroyOnLoad(gameObject.transform.root);

        Init();
    }

    protected virtual void OnDestroy()
    {
        if (s_instance != null && s_instance.gameObject == gameObject)
        {
            s_instance = null;
        }
    }

    protected virtual void Init()
    {

    }

    protected virtual void Uninit()
    {

    }

    protected virtual void OnApplicationQuit()
    {
        Uninit();
        s_instance = null;
    }
}
