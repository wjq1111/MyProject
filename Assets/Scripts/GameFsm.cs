using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
public enum GameFsmState
{
    Invalid,
    StartGame,
}

public class GameFsm
{
    private static GameFsm instance;

    public static GameFsm Instance { 
        get
        {
            if (instance == null)
            {
                instance = new GameFsm();
            }
            return instance;
        }
    }
    
    public GameFsmState state;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void AcceptEvent()
    {
        //Debug.Log("base accept event");
    }

    public virtual void DoSomething()
    {
        System.Threading.Thread.Sleep(2000);
        Debug.Log("sleep finish");
    }

    public virtual void GenerateEvent()
    {

    }
}
