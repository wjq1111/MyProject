using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Unity.Collections;
using UnityEngine;

public class Gamecore : Singleton<Gamecore>
{
    // ��ǰ�غ���
    public int currentRound;
    // ���غ���
    private int maxRound;
    // ÿ�غ�ʱ��
    private int eachRoundTime;
    // �»غϸ���ʱ��
    private long nextRoundTime;
    // ��Ϸ״̬
    public GameStatus gameStatus;

    // ��ǰ�غ���˭�ж�
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

    // ������
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

    // ����
    public void Fight(CampId source, int sourceMonsterIndex, CampId target, int targetMonsterIndex)
    {
        MonsterBase monster1 = GetPlayer(source).GetMonster(sourceMonsterIndex);
        MonsterBase monster2 = GetPlayer(target).GetMonster(targetMonsterIndex);
        monster1.Fight(ref monster2);

        CalcStatus();
    }

    // ʹ�ÿ�Ƭ��source�Ƿ��𷽣�target��Ŀ�귽��targetMonsterIndex��Ŀ�����id
    public void UseCard(CampId source, int cardId, CampId target, List<int> targetMonsterIndex)
    {
        CardBase card = GetPlayer(source).GetCard(cardId);
        if (card.useCardType == UseCardType.MonsterCard)
        {
            // �ٻ����￨ֻ����Լ��ã�����source��targetҪһ��
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
            // ���Կ����ܸ��Լ����߱����ã�Ҫ�ֱ����۲�ͬ�����Կ�
            AttributeCard attributeCard = new AttributeCard(card);
            if (attributeCard.attributeCardUseTargetType == AttributeCardUseTargetType.MySelf)
            {
                // source��һ�����Լ���Ч�Ŀ�Ƭ
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
                // source��һ���ԶԷ���Ч�Ŀ�Ƭ
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
                // source��ȫ��ʹ�����Կ�Ƭ
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

    // ���㳡��״̬
    public void CalcStatus()
    {
        // ���ж�����ǲ��ǹ���
        List<Player> deadPlayer = new List<Player>();
        foreach (var player in playerDict)
        {
            // �Լ����ˣ����ֽ���������Ҫ��update��
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

        // ���û�ң������˶�����
        if (deadPlayer.Count == playerDict.Count - 1)
        {
            SetGameStatus(GameStatus.Win);
            return;
        }

        // ���㳡�����״̬
        foreach (var player in playerDict)
        {
            if (player.Value.IsDead())
            {
                continue;
            }
            player.Value.CalcStatus();
        }
    }

    // ��ʼ�غ�
    public void StartRound()
    {
        if (currentRound >= maxRound)
        {
            Debug.Log("round over");
            return;
        }

        // ����״̬
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

    // ������һ���غ�ʱ��
    private void SetNextRoundTime()
    {
        TimeSpan time = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
        nextRoundTime = Convert.ToInt64(time.TotalSeconds) + eachRoundTime;
    }
    
    // ����������
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

        // �����¸��غ�ʱ��
        SetNextRoundTime();

        // ��ʼ����غ�
        StartRound();

        // AI���ƣ����ڻ�û��ai�����߼�������aiֻ�鿨������


        // �����¸��غ�
        currentRound += 1;
    }
}
