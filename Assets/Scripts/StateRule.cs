using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRule
{
    public static StateRule instance;

    public static StateRule Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new StateRule();
            }
            return instance;
        }
    }
    // 某一个fsm1，接到某个event后，变为fsm2
    public Dictionary<GameFsmState, Dictionary<EventId, GameFsmState>> acceptStateRule = new Dictionary<GameFsmState, Dictionary<EventId, GameFsmState>>();
    // 某一个fsm，接到某个event1后，输出event2
    public Dictionary<GameFsmState, Dictionary<EventId, EventId>> sendStateRule = new Dictionary<GameFsmState, Dictionary<EventId, EventId>>();

    public void Init()
    {
        Dictionary<EventId, GameFsmState> dic1 = new Dictionary<EventId, GameFsmState>();
        dic1.Add(EventId.StartGame, GameFsmState.StartGame);
        acceptStateRule.Add(GameFsmState.Invalid, dic1);

        Dictionary<EventId, EventId> dic2 = new Dictionary<EventId, EventId>();
        dic2.Add(EventId.StartGame, EventId.LoadGame);
        sendStateRule.Add(GameFsmState.Invalid, dic2);
    }

}
