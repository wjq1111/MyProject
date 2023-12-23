using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PoolManager : MonoSingleton<PoolManager>
{
    private GameObject poolObj = null;
    private Dictionary<ResourceType, List<GameObject>> pool;

    protected override void Awake()
    {
        pool = new Dictionary<ResourceType, List<GameObject>>();
    }

    protected override void OnDestroy()
    {

    }

    public GameObject GetObject(ResourceType type)
    {
        if (type == ResourceType.Invalid)
        {
            Debug.LogError("resource type invalid");
            return null;
        }
        GameObject obj = null;
        if (pool.ContainsKey(type) && pool[type].Count > 0)
        {
            obj = pool[type][0];
            pool[type].RemoveAt(0);
        }
        else
        {
            switch (type)
            {
                case ResourceType.PlayerCardButton:
                    obj = GameObject.Instantiate<GameObject>(GameFramework.Instance.GameConf.PlayerCardButton);
                    break;
                case ResourceType.MonsterButton:
                    obj = GameObject.Instantiate<GameObject>(GameFramework.Instance.GameConf.MonsterButton);
                    break;
            }
        }
        obj.SetActive(true);
        obj.transform.SetParent(null);
        return obj;
    }

    // ¹é»¹object
    public void PushObject(ResourceType type, GameObject obj)
    {
        if (poolObj == null)
        {
            poolObj = new GameObject("PoolObject");
        }

        if (pool.ContainsKey(type))
        {
            pool[type].Add(obj);
        }
        else
        {
            pool.Add(type, new List<GameObject>() { obj });
        }

        if (poolObj.transform.Find(type.ToString()) == null)
        {
            new GameObject(type.ToString()).transform.SetParent(poolObj.transform);
        }

        obj.SetActive(false);
        obj.transform.SetParent(poolObj.transform.Find(type.ToString()));
    }
}
