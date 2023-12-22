using UnityEngine;

public class MonsterCard : CardBase
{
    // 怪物卡攻击
    public int attack;
    // 怪物卡防御
    public int defense;
    // 怪物卡血量
    public int hp;
    // 怪物卡行动次数
    public int actionNum;
    // 怪物卡攻击时，如果被防御住，造成的最低伤害
    public int minDamage;

    public MonsterCard() { }

    public MonsterCard(CardBase card)
    {
        this.gid = card.gid;
        this.useCardType = card.useCardType;
        this.cardName = card.cardName;
        this.outlookCardName = card.outlookCardName;
    }

    // 初始化怪物
    public void Init()
    {
        if (!ConfigManager.Instance.monsterCardManualMap.ContainsKey(this.id))
        {
            Debug.LogError("card id not in monster card manual map");
            return;
        }
        var cfgCard = ConfigManager.Instance.monsterCardManualMap[this.id];
        this.attack = cfgCard.Attack;
        this.defense = cfgCard.Defense;
        this.hp = cfgCard.Hp;
        this.actionNum = cfgCard.ActionNum;
        this.minDamage = 1;
    }
}