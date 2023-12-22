// 卡片基类和各种卡片的实现

// 卡片使用效果类型
using System.Collections.Generic;
using UnityEngine;

public enum UseCardType
{
    Invalid,
    MonsterCard,
    AttributeCard,
    LGCard,
}

// 卡片能力
public class CardAbility
{
    public Ability ability;
    public int abilityCardId;
}

// 卡片召唤怪物能力
public class CardAbilitySummonMonster : CardAbility
{
    public int summonCount;
    public MonsterCard monsterCard;

    public CardAbilitySummonMonster()
    {
        ability = Ability.Invalid;
    }

    public CardAbilitySummonMonster(CardAbility ability)
    {
        this.ability = ability.ability;
        // 初始化这张怪物卡
        this.monsterCard.id = this.abilityCardId;
        this.monsterCard.Init();
    }
}

// 卡片修改怪物属性能力
public class CardAbilityModifyMonsterAttribute : CardAbility
{
    public int hurtCount;
    public AttributeCard attributeCard;

    public CardAbilityModifyMonsterAttribute()
    {
        ability = Ability.Invalid;
    }

    public CardAbilityModifyMonsterAttribute(CardAbility ability)
    {
        this.ability = ability.ability;
        // 初始化这张属性卡
        this.attributeCard.id = this.abilityCardId;
        this.attributeCard.Init();
    }
}

public class CardBase
{
    // 配置表对应的id
    public int id;
    // 全局唯一的卡片id
    public int gid;
    // 卡片使用效果类型
    public UseCardType useCardType;
    // 卡片名称
    public string cardName;
    // 卡片外显名称
    public string outlookCardName;
    // 卡片能力
    public List<CardAbility> cardAbility;

    public CardBase()
    {
        this.gid = AllocGid.Instance.Alloc();
        cardAbility = new List<CardAbility>();
    }

    public void Init(int cardId)
    {
        this.id = cardId;
        if (!ConfigManager.Instance.cardManualMap.ContainsKey(cardId))
        {
            Debug.LogError("card id not in card manual map" + cardId.ToString());
            return;
        }
        var cfgCard = ConfigManager.Instance.cardManualMap[cardId];
        this.cardName = cfgCard.CardName;
        this.useCardType = (UseCardType)cfgCard.UseCardType;

        // 初始化卡的能力
        string abilityStr = cfgCard.Ability;
        if (abilityStr.Length != 0)
        {
            string[] subAbilityArray = abilityStr.Split(";");
            foreach (string subAbilityStr in subAbilityArray)
            {
                string[] ability = subAbilityStr.Split(",");
                if (ability.Length != 2)
                {
                    Debug.LogError("ability length not 2");
                    return;
                }
                Ability abilityType = (Ability)int.Parse(ability[0]);
                int abilityCardId = int.Parse(ability[1]);
                CardAbility newCardAbility = new CardAbility();
                newCardAbility.ability = abilityType;
                newCardAbility.abilityCardId = abilityCardId;
            }
        }
    }

    public override string ToString()
    {
        string str = "";
        str += "卡片名称: " + cardName + "\n";
        str += "gid: " + gid + "\n";
        str += "卡片使用类型: " + useCardType + "\n";
        str += "卡片能力:\n";
        foreach (var ability in cardAbility)
        {
            str += ability + " ";
        }
        return str;
    }
}

