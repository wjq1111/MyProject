using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameFsm : GameFsm
{
    // Start is called before the first frame update
    void Start()
    {
        state = GameFsmState.StartGame;
    }

    // Update is called once per frame
    public void Update()
    {
        AcceptEvent();
    }

    public override void AcceptEvent()
    {
        base.AcceptEvent();
        Debug.Log("start game accept event");
    }
}
