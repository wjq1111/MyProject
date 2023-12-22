// ���Կ��߼�

using UnityEngine;

public class AttributeCard : CardBase
{
    // ����������
    public int addAttack;
    // ����������
    public int addDefense;
    // ����Ѫ��
    public int addHp;
    // �����ж�����
    public int addActionNum;
    // ���Կ����õ�λ�����������ȫ�����ã�useNumΪ0
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
