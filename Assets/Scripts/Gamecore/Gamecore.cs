using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Unity.Collections;
using UnityEngine;

public class Gamecore : Singleton<Gamecore>
{
    // 当前回合数
    public int currentRound;
    // 最大回合数
    private int maxRound;
    // 每回合时间
    private int eachRoundTime;
    // 下回合更新时间
    private long nextRoundTime;
    // 游戏状态
    public GameStatus gameStatus;

    // 当前回合是谁行动
    public CampId gameRoundState;

    public Dictionary<CampId, Player> playerDict;

    public override void Init()
    {
        // init values
        currentRound = 0;
        gameStatus = GameStatus.Invalid;

        // TODO config
        maxRound = 20;
        eachRoundTime = 6000;
        playerDict = new Dictionary<CampId, Player>();
    }

    private void StartGame()
    {
        currentRound = 1;
        InitAllPlayers();
        InitAllMonsters();
        InitGameStartCard();

        SetNextRoundTime();
    }

    public void SetGameStatus(GameStatus result)
    {
        gameStatus = result;
    }

    private void InitAllPlayers()
    {
        Player player = new Player();
        player.campId = CampId.Myself;
        player.Init();
        Player aiPlayer = new Player();
        aiPlayer.campId = CampId.AI;
        aiPlayer.Init();
        playerDict.Add(CampId.Myself, player);
        playerDict.Add(CampId.AI, aiPlayer);
    }

    public Player GetPlayer(CampId id)
    {
        if (playerDict.ContainsKey(id))
        {
            return playerDict[id];
        }
        return null;
    }

    private void InitAllMonsters()
    {
        
    }

    private void InitGameStartCard()
    {
        // TODO config
        int firstDrawCardNum = 4;
        GetPlayer(CampId.AI).EachRoundDrawCard(firstDrawCardNum);
        GetPlayer(CampId.Myself).EachRoundDrawCard(firstDrawCardNum);
    }

    // 攻击脸
    public void FightFace(CampId source, CampId target, int damage)
    {
        if (source == target)
        {
            Debug.LogError("camp same, can not fight face");
            return;
        }
        if (target != CampId.All)
        {
            GetPlayer(target).FightFace(damage);
        }
        else
        {
            foreach (var player in playerDict)
            {
                if (player.Value.campId != source)
                {
                    player.Value.FightFace(damage);
                }
            }
        }
    }

    // 攻击
    public void Fight(CampId source, int sourceMonsterIndex, CampId target, int targetMonsterIndex)
    {
        MonsterBase monster1 = GetPlayer(source).GetMonster(sourceMonsterIndex);
        MonsterBase monster2 = GetPlayer(target).GetMonster(targetMonsterIndex);
        monster1.Fight(ref monster2);

        CalcStatus();
    }

    // 使用卡片，source是发起方，target是目标方，targetMonsterIndex是目标怪物id
    public void UseCard(CampId source, int cardId, CampId target, List<int> targetMonsterIndex)
    {
        CardBase card = GetPlayer(source).GetCard(cardId);
        if (card.useCardType == UseCardType.MonsterCard)
        {
            // 召唤怪物卡只会给自己用，所以source和target要一致
            if (source != target)
            {
                Debug.LogError("not same camp, can not use card");
                return;
            }
            MonsterCard monsterCard = new MonsterCard(card);
            GetPlayer(target).UseMonsterCard(monsterCard);
        }
        else if (card.useCardType == UseCardType.AttributeCard)
        {
            // 属性卡可能给自己或者别人用，要分别讨论不同的属性卡
            AttributeCard attributeCard = new AttributeCard(card);
            if (attributeCard.attributeCardUseTargetType == AttributeCardUseTargetType.MySelf)
            {
                // source用一个对自己生效的卡片
                if (source != target)
                {
                    Debug.LogError("not same camp, can not use card");
                    return;
                }
                if (attributeCard.useNum != targetMonsterIndex.Count)
                {
                    Debug.LogError("not same use num, can not use card");
                    return;
                }
                GetPlayer(target).UseAttributeCard(attributeCard, targetMonsterIndex);
            }
            else if (attributeCard.attributeCardUseTargetType == AttributeCardUseTargetType.Other)
            {
                // source用一个对对方生效的卡片
                if (source == target)
                {
                    Debug.LogError("same camp, can not use card");
                    return;
                }
                if (attributeCard.useNum != targetMonsterIndex.Count)
                {
                    Debug.LogError("not same use num, can not use card");
                    return;
                }
                GetPlayer(target).UseAttributeCard(attributeCard, targetMonsterIndex);
            }
            else if (attributeCard.attributeCardUseTargetType == AttributeCardUseTargetType.All)
            {
                // source对全体使用属性卡片
                if (target != CampId.All)
                {
                    Debug.LogError("target camp not all, can not use card");
                    return;
                }
                if (attributeCard.useNum != 0)
                {
                    Debug.LogError("not same use num, can not use card");
                    return;
                }
                foreach (var player in playerDict)
                {
                    player.Value.UseAttributeCard(attributeCard, null);
                }
            }
        }

        CalcStatus();
    }

    // 计算场面状态
    public void CalcStatus()
    {
        // 先判断玩家是不是挂了
        List<Player> deadPlayer = new List<Player>();
        foreach (var player in playerDict)
        {
            // 自己挂了，本局结束，不需要再update了
            if (player.Value.IsDead())
            {
                if (player.Value.campId == CampId.Myself)
                {
                    SetGameStatus(GameStatus.Lose);
                    return;
                }
                deadPlayer.Add(player.Value);
            }
        }

        // 玩家没挂，其他人都挂了
        if (deadPlayer.Count == playerDict.Count - 1)
        {
            SetGameStatus(GameStatus.Win);
            return;
        }

        // 计算场面怪物状态
        foreach (var player in playerDict)
        {
            if (player.Value.IsDead())
            {
                continue;
            }
            player.Value.CalcStatus();
        }
    }

    // 开始回合
    public void StartRound()
    {
        if (currentRound >= maxRound)
        {
            Debug.Log("round over");
            return;
        }

        // 计算状态
        CalcStatus();

        // draw card
        // if round is odd and ai not action first, or round is even and ai action first, should be ai round
        if ((currentRound % 2 == 1 && !GetPlayer(CampId.Myself).isActionFirst) || 
            (currentRound % 2 == 0 && GetPlayer(CampId.AI).isActionFirst))
        {
            gameRoundState = CampId.AI;
            GetPlayer(CampId.AI).EachRoundDrawCard(GetPlayer(CampId.AI).eachRoundDrawCardNum);
        }
        else
        {
            gameRoundState = CampId.Myself;
            GetPlayer(CampId.Myself).EachRoundDrawCard(GetPlayer(CampId.Myself).eachRoundDrawCardNum);
        }
    }

    // 设置下一个回合时间
    private void SetNextRoundTime()
    {
        TimeSpan time = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
        nextRoundTime = Convert.ToInt64(time.TotalSeconds) + eachRoundTime;
    }
    
    // 主驱动函数
    public void Update()
    {
        if (currentRound >= maxRound)
        {
            Debug.Log("round max, end game");
            SetGameStatus(GameStatus.Lose);
            return;
        }

        if (currentRound == 0)
        {
            StartGame();
        }

        TimeSpan time = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
        long now = Convert.ToInt64(time.TotalSeconds);
        if (now < nextRoundTime)
        {
            return;
        }

        // 设置下个回合时间
        SetNextRoundTime();

        // 开始这个回合
        StartRound();

        // AI出牌，现在还没有ai出牌逻辑，于是ai只抽卡不出牌


        // 到了下个回合
        currentRound += 1;
    }
}
