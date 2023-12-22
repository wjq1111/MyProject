// 属性卡逻辑

using UnityEngine;

public class AttributeCard : CardBase
{
    // 增幅攻击力
    public int addAttack;
    // 增幅防御力
    public int addDefense;
    // 增幅血量
    public int addHp;
    // 增幅行动次数
    public int addActionNum;
    // 属性卡作用单位数量，如果是全体作用，useNum为0
    public int useNum;

    public AttributeCard() { }

    public AttributeCard(CardBase card)
    {
        this.gid = card.gid;
        this.useCardType = card.useCardType;
        this.cardName = card.cardName;
        this.outlookCardName = card.outlookCardName;
    }

    public void Init()
    {
        if (!ConfigManager.Instance.attributeCardManualMap.ContainsKey(this.id))
        {
            Debug.LogError("card id not in attributeCardManualMap" + this.id);
            return;
        }
        var cfgCard = ConfigManager.Instance.attributeCardManualMap[this.id];
        this.addAttack = cfgCard.Attack;
        this.addDefense = cfgCard.Defense;
        this.addHp = cfgCard.Hp;
        this.addActionNum = cfgCard.ActionNum;
        this.useNum = 1;
    }
}
