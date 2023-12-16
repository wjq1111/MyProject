using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
using UnityEngine;

public class Gamecore : Singleton<Gamecore>
{
    public int currentRound;
    private int maxRound;
    private int eachRoundTime;
    private long nextRoundTime;

    public Player player;
    public Player aiPlayer;

    public CampId gameRoundState;

    public override void Init()
    {
        // init values
        currentRound = 0;

        // TODO config
        maxRound = 20;
        eachRoundTime = 6000;
        InitAll();
    }

    private void InitAll()
    {
        InitAllPlayers();
        InitAllMonsters();
        InitGameStartCard();

        SetNextRoundTime();
    }

    private void InitAllPlayers()
    {
        player = new Player();
        player.campId = CampId.Mine;
        player.Init();
        aiPlayer = new Player();
        aiPlayer.campId = CampId.AI;
        aiPlayer.Init();
    }

    private void InitAllMonsters()
    {
        // aiPlayer.InitGameStartMonsters();
        // player.InitGameStartMonsters();
    }

    private void InitGameStartCard()
    {
        // TODO config
        int firstDrawCardNum = 4;
        aiPlayer.EachRoundDrawCard(firstDrawCardNum);
        player.EachRoundDrawCard(firstDrawCardNum);
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

        // draw card
        // if round is odd and ai not action first, or round is even and ai action first, should be ai round
        if ((currentRound % 2 == 1 && !aiPlayer.isActionFirst) || 
            (currentRound % 2 == 0 && aiPlayer.isActionFirst))
        {
            gameRoundState = CampId.AI;
            aiPlayer.EachRoundDrawCard(aiPlayer.eachRoundDrawCardNum);
        }
        else
        {
            gameRoundState = CampId.Mine;
            player.EachRoundDrawCard(player.eachRoundDrawCardNum);
        }

        // turn to next round
        SetNextRoundTime();
    }

    private void SetNextRoundTime()
    {
        TimeSpan time = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
        nextRoundTime = Convert.ToInt64(time.TotalSeconds) + eachRoundTime;
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
