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
    // �¸��غ���˭�ж�
    public CampId nextRoundState;

    public Dictionary<CampId, Player> playerDict;

    public Dictionary<CampId, CampId> roundSeqDict;

    public override void Init()
    {
        // init values
        currentRound = 0;

        // TODO config
        maxRound = 20;
        eachRoundTime = 6000;
        playerDict = new Dictionary<CampId, Player>();
        roundSeqDict = new Dictionary<CampId, CampId>();

        GameFsm.Instance.AddFsm(GameStatus.OnGoing, StartGame);
        GameFsm.Instance.AddFsm(GameStatus.Lose, EndGame);
        GameFsm.Instance.AddFsm(GameStatus.Win, EndGame);

        roundSeqDict.Add(CampId.Myself, CampId.AI);
        roundSeqDict.Add(CampId.AI, CampId.Myself);
    }

    public override void Uninit()
    {
        base.Uninit();
        GameFsm.Instance.RemoveFsm(GameStatus.OnGoing, StartGame);
        GameFsm.Instance.RemoveFsm(GameStatus.Lose, EndGame);
        GameFsm.Instance.RemoveFsm(GameStatus.Win, EndGame);
    }

    private void StartGame()
    {
        currentRound = 1;
        gameStatus = GameStatus.Invalid;
        nextRoundState = CampId.Myself;
        InitAllPlayers();
        InitAllMonsters();
        InitGameStartCard();

        StartRound();
    }

    private void EndGame()
    {
        if (gameStatus == GameStatus.Win)
        {
            
        }
        else
        {

        }

        // �嵥������
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
        GetPlayer(CampId.AI).EachRoundDrawCard();
        GetPlayer(CampId.Myself).EachRoundDrawCard();
    }

    // ������
    public void FightFace(CampId source, CampId target, int damage)
    {
        if (source == target)
        {
            Debug.LogError("camp same, can not fight face");
            return;
        }
        MonsterBase sourceMonster = GetPlayer(CampId.Myself).face;
        foreach (var player in playerDict)
        {
            if (player.Value.campId != source)
            {
                MonsterFight(sourceMonster, player.Value.face);
            }
        }
    }

    // �����˺�
    public int CalcDamage(MonsterBase sourceMonster, MonsterBase targetMonster)
    {
        // mine.hp = mine.hp - (target.attack - mine.defense)
        int damage = targetMonster.attack - sourceMonster.defense;
        if (damage < 0)
        {
            Debug.Log("this " + sourceMonster.monsterName + " attack target" + targetMonster.monsterName);
            Debug.Log("can not damage, target.attack" + targetMonster.attack + " this.defense " + sourceMonster.defense);
            damage = 0;
        }
        return damage;
    }

    public void MonsterFight(MonsterBase sourceMonster, MonsterBase targetMonster)
    {
        // �����ж�����
        bool canAct = sourceMonster.CostActionNum();
        if (!canAct)
        {
            return;
        }

        int damage = CalcDamage(sourceMonster, targetMonster);
        targetMonster.Hurt(damage);
        damage = CalcDamage(targetMonster, sourceMonster);
        sourceMonster.Hurt(damage);
    }

    // ����
    public void Fight(CampId source, int sourceMonsterIndex, CampId target, int targetMonsterIndex)
    {
        MonsterBase monster1 = GetPlayer(source).GetMonster(sourceMonsterIndex);
        MonsterBase monster2 = GetPlayer(target).GetMonster(targetMonsterIndex);
        MonsterFight(monster1, monster2);

        CalcStatus();
    }

    // ʹ���ٻ����￨
    public void UseMonsterCard(CampId id, CardBase card)
    {
        if (card.useCardType != UseCardType.MonsterCard)
        {
            return;
        }

        Player player = GetPlayer(id);
        MonsterCard monsterCard = new MonsterCard(card);
        monsterCard.Init();
        player.UseMonsterCard(monsterCard);
    }

    public List<int> GetAllMonsterIndex(CampId id)
    {
        Player player = GetPlayer(id);
        List<int> result = player.GetAllMonsterIndex();
        return result;
    }

    public void UseAttributeCard(CampId source, CampId target, CardBase card, List<int> targetMonsterIndex)
    {
        if (card.useCardType != UseCardType.AttributeCard)
        {
            return;
        }

        Player sourcePlayer = GetPlayer(source);
        Player targetPlayer = GetPlayer(target);
        AttributeCard attributeCard = new AttributeCard(card);
        attributeCard.Init();
        if (attributeCard.useNum != targetMonsterIndex.Count)
        {
            Debug.LogError("not same use num, can not use card");
            return;
        }

        if (attributeCard.attributeCardUseTargetType == AttributeCardUseTargetType.MySelf)
        {
            // ��һ�����Լ���Ч�Ŀ�Ƭ
            sourcePlayer.UseAttributeCard(attributeCard, targetMonsterIndex);
        }
        else if (attributeCard.attributeCardUseTargetType == AttributeCardUseTargetType.Other)
        {
            // ��һ���ԶԷ���Ч�Ŀ�Ƭ
            targetPlayer.UseAttributeCard(attributeCard, targetMonsterIndex);
        }
        else if (attributeCard.attributeCardUseTargetType == AttributeCardUseTargetType.All)
        {
            // ��ȫ��ʹ�����Կ�Ƭ
            if (attributeCard.useNum != 0)
            {
                Debug.LogError("not same use num, can not use card");
                return;
            }
            foreach (var player in playerDict)
            {
                player.Value.UseAttributeCard(attributeCard, GetAllMonsterIndex(player.Value.campId));
            }
        }
    }

    // ʹ�ÿ�Ƭ��source�Ƿ��𷽣�target��Ŀ�귽��targetMonsterIndex��Ŀ�����id
    public void UseCard(CampId source, int cardId, CampId target = CampId.Invalid, List<int> targetMonsterIndex = null)
    {
        CardBase card = GetPlayer(source).GetCard(cardId);

        // ʹ�ÿ�
        if (card.useCardType == UseCardType.MonsterCard)
        {
            // �ٻ����￨ֻ����Լ��ã�����source��targetҪһ��
            if (target != CampId.Invalid)
            {
                Debug.LogError("not same camp, can not use card");
                return;
            }
            UseMonsterCard(source, card);
        }
        else if (card.useCardType == UseCardType.AttributeCard)
        {
            // ʹ�����Կ�Ŀ�����index�����ǿ�
            if (targetMonsterIndex != null)
            {
                Debug.LogError("targetMonsterIndex null, can not use card");
                return;
            }
            UseAttributeCard(source, target, card, targetMonsterIndex);
        }

        // ��������
        foreach (var cardAbility in card.cardAbility)
        {
            if (cardAbility.ability == Ability.SummonMonster)
            {
                CardAbilitySummonMonster cardAbilitySummonMonster = new CardAbilitySummonMonster(cardAbility);
                // �ٻ������Ĺ���Ӧ������������ĺ���
                UseMonsterCard(source, cardAbilitySummonMonster.monsterCard);
            }
            else if (cardAbility.ability == Ability.HurtMonster)
            {
                CardAbilityHurtMonster cardAbilityHurtMonster = new CardAbilityHurtMonster(cardAbility);
                UseAttributeCard(source, target, cardAbilityHurtMonster.attributeCard, targetMonsterIndex);
            }
        }

        // ���㳡��״̬
        CalcStatus();

        // ȥ������
        GetPlayer(source).RemoveCard(cardId);
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
                    GameFsm.Instance.SetGameStatus(GameStatus.Lose);
                    return;
                }
                deadPlayer.Add(player.Value);
            }
        }

        // ���û�ң������˶�����
        if (deadPlayer.Count == playerDict.Count - 1)
        {
            GameFsm.Instance.SetGameStatus(GameStatus.Win);
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

    // ��ʼ�غϣ�ʵ����Ҳ�ǻغϽ��㺯��
    public void StartRound()
    {
        // �����¸��غ�
        currentRound += 1;

        if (currentRound >= maxRound)
        {
            Debug.Log("round over");
            return;
        }

        // ����������˭�����¸��غ���˭��
        gameRoundState = nextRoundState;
        nextRoundState = roundSeqDict[gameRoundState];

        // �鿨
        GetPlayer(gameRoundState).EachRoundDrawCard();

        // �����¸��غ�ʱ��
        SetNextRoundTime();
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
        // TODO �Ż� realtimeSinceStartup
        TimeSpan time = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
        long now = Convert.ToInt64(time.TotalSeconds);
        if (now < nextRoundTime)
        {
            return;
        }

        if (gameStatus != GameStatus.OnGoing)
        {
            return;
        }

        // ��ʼ����غ�
        StartRound();
    }

    // ��ǰ״̬
    public override string ToString()
    {
        string text = "";
        foreach (var player in playerDict)
        {
            text += player.Value.ToString();
            text += "\n";
        }
        return text;
    }
}
