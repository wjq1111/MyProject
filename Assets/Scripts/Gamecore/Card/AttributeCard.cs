// ���Կ��߼�

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
    // ���Կ�ʹ��Ŀ��
    public AttributeCardUseTargetType attributeCardUseTargetType;
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
        // config, id -> cfg
        this.addAttack = 1;
        this.addDefense = 1;
        this.addHp = 1;
        this.addActionNum = 1;
        this.attributeCardUseTargetType = AttributeCardUseTargetType.MySelf;
        this.useNum = 1;
    }
}
