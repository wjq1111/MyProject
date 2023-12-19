using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GameFsm : Singleton<GameFsm>
{
    public Dictionary<GameStatus, List<Action>> gameFsm = new Dictionary<GameStatus, List<Action>>();

    public override void Init()
    {

    }

    public void AddFsm(GameStatus gameStatus, Action action)
    {
        if (!gameFsm.ContainsKey(gameStatus))
        {
            List<Action> actionList = new List<Action>();
            gameFsm.Add(gameStatus, actionList);
        }
        gameFsm[gameStatus].Add(action);
    }

    public void RemoveFsm(GameStatus gameStatus, Action action)
    {
        if (gameFsm.ContainsKey(gameStatus))
        {
            int index = gameFsm[gameStatus].IndexOf(action);
            if (index > 0)
            {
                gameFsm[gameStatus].RemoveAt(index);
            }
        }
    }

    public void SetGameStatus(GameStatus result)
    {
        if (gameFsm.ContainsKey(result))
        {
            foreach (var action in gameFsm[result])
            {
                action();
            }
        }
    }
}
