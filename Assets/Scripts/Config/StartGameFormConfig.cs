using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StartGameFormItem
{
    public int number;
}

[System.Serializable]
public class StartGameFormConfig
{
    public StartGameFormItem item;

    public void Print()
    {
        Debug.Log("debug:" + item.number);
    }
}
