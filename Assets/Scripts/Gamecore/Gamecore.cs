using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Gamecore : Singleton<Gamecore>
{
    public int currentRound;
    private int maxRound;
    private int eachRoundTime;
    private long nextRoundTime;

    // max num of monsters
    private int maxNum;

    // all monsters, 0-maxNum = AI, maxNum-2maxNum = Mine
    private List<MonsterBase> monsterList;

    public override void Init()
    {
        // init values
        currentRound = 0;
        monsterList = new List<MonsterBase>(2 * maxNum);

        // TODO config
        maxRound = 20;
        eachRoundTime = 60;
        maxNum = 4;
        SetNextRoundTime();
        InitAll();
    }

    private void InitAll()
    {
        InitAllMonsters(MonsterCampId.AI);
        InitAllMonsters(MonsterCampId.Mine);
    }

    private void InitAllMonsters(MonsterCampId id)
    {
        // TODO use config to init monsters, now directly write them
        for (int i = 0; i < maxNum; i++)
        {
            MonsterBase monster = new MonsterBase();
            monster.NewMonsterBase("lgsb" + i.ToString(), id, 100 * (i + 1), 100 * (i + 1), 100 * (i + 1));
            monsterList.Add(monster);
        }
    }

    public void TestAttack()
    {
        // test ai 2 attack mine 1
        MonsterBase ai = GetMonsterBase(MonsterCampId.AI, 1);
        MonsterBase mine = GetMonsterBase(MonsterCampId.Mine, 0);
        ai.Fight(ref mine);
    }

    public MonsterBase GetMonsterBase(MonsterCampId id, int index)
    {
        if (id == MonsterCampId.AI)
        {
            return monsterList[index];
        }
        if (id == MonsterCampId.Mine)
        {
            return monsterList[maxNum + index];
        }
        return null;
    }

    public void EndRound()
    {
        if (currentRound >= maxRound)
        {
            Debug.Log("round over");
            return;
        }

        // calc all status
        currentRound += 1;

        // turn to next round
        SetNextRoundTime();
        PrintGamecoreInfo();
    }

    private void SetNextRoundTime()
    {
        TimeSpan time = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
        nextRoundTime = Convert.ToInt64(time.TotalSeconds) + eachRoundTime;
    }

    private void PrintGamecoreInfo()
    {
        string monsterStatus = "";
        for (int i = 0; i < 2 * maxNum; i++)
        {
            monsterStatus += monsterList[i].PrintStatus();
        }
        Debug.Log("currentRound: " + currentRound + " nextRoundTime: " + nextRoundTime + " status: " + monsterStatus);
    }

    public void Update()
    {
        if (currentRound >= maxRound)
        {
            return;
        }
        TimeSpan time = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
        long now = Convert.ToInt64(time.TotalSeconds);
        if (now < nextRoundTime)
        {
            return;
        }
        // auto next round
        EndRound();
    }
}
