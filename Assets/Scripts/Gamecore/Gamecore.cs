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
    // 下个回合是谁行动
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

        // 清单局数据
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

    // 攻击脸
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

    // 计算伤害
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
        // 计算行动次数
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

    // 攻击
    public void Fight(CampId source, int sourceMonsterIndex, CampId target, int targetMonsterIndex)
    {
        MonsterBase monster1 = GetPlayer(source).GetMonster(sourceMonsterIndex);
        MonsterBase monster2 = GetPlayer(target).GetMonster(targetMonsterIndex);
        MonsterFight(monster1, monster2);

        CalcStatus();
    }

    // 使用召唤怪物卡
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
            // 用一个对自己生效的卡片
            sourcePlayer.UseAttributeCard(attributeCard, targetMonsterIndex);
        }
        else if (attributeCard.attributeCardUseTargetType == AttributeCardUseTargetType.Other)
        {
            // 用一个对对方生效的卡片
            targetPlayer.UseAttributeCard(attributeCard, targetMonsterIndex);
        }
        else if (attributeCard.attributeCardUseTargetType == AttributeCardUseTargetType.All)
        {
            // 对全体使用属性卡片
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

    // 使用卡片，source是发起方，target是目标方，targetMonsterIndex是目标怪物id
    public void UseCard(CampId source, int cardId, CampId target = CampId.Invalid, List<int> targetMonsterIndex = null)
    {
        CardBase card = GetPlayer(source).GetCard(cardId);

        // 使用卡
        if (card.useCardType == UseCardType.MonsterCard)
        {
            // 召唤怪物卡只会给自己用，所以source和target要一致
            if (target != CampId.Invalid)
            {
                Debug.LogError("not same camp, can not use card");
                return;
            }
            UseMonsterCard(source, card);
        }
        else if (card.useCardType == UseCardType.AttributeCard)
        {
            // 使用属性卡目标怪物index不能是空
            if (targetMonsterIndex != null)
            {
                Debug.LogError("targetMonsterIndex null, can not use card");
                return;
            }
            UseAttributeCard(source, target, card, targetMonsterIndex);
        }

        // 卡的能力
        foreach (var cardAbility in card.cardAbility)
        {
            if (cardAbility.ability == Ability.SummonMonster)
            {
                CardAbilitySummonMonster cardAbilitySummonMonster = new CardAbilitySummonMonster(cardAbility);
                // 召唤出来的怪物应该在这个索引的后面
                UseMonsterCard(source, cardAbilitySummonMonster.monsterCard);
            }
            else if (cardAbility.ability == Ability.HurtMonster)
            {
                CardAbilityHurtMonster cardAbilityHurtMonster = new CardAbilityHurtMonster(cardAbility);
                UseAttributeCard(source, target, cardAbilityHurtMonster.attributeCard, targetMonsterIndex);
            }
        }

        // 计算场面状态
        CalcStatus();

        // 去掉手牌
        GetPlayer(source).RemoveCard(cardId);
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
                    GameFsm.Instance.SetGameStatus(GameStatus.Lose);
                    return;
                }
                deadPlayer.Add(player.Value);
            }
        }

        // 玩家没挂，其他人都挂了
        if (deadPlayer.Count == playerDict.Count - 1)
        {
            GameFsm.Instance.SetGameStatus(GameStatus.Win);
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

    // 开始回合，实际上也是回合结算函数
    public void StartRound()
    {
        // 到了下个回合
        currentRound += 1;

        if (currentRound >= maxRound)
        {
            Debug.Log("round over");
            return;
        }

        // 计算现在是谁动，下个回合是谁动
        gameRoundState = nextRoundState;
        nextRoundState = roundSeqDict[gameRoundState];

        // 抽卡
        GetPlayer(gameRoundState).EachRoundDrawCard();

        // 设置下个回合时间
        SetNextRoundTime();
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
        // TODO 优化 realtimeSinceStartup
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

        // 开始这个回合
        StartRound();
    }

    // 当前状态
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
