public class MonsterCard : CardBase
{
    // ���￨����
    public int attack;
    // ���￨����
    public int defense;
    // ���￨Ѫ��
    public int hp;
    // ���￨�ж�����
    public int actionNum;
    // ���￨����ʱ�����������ס����ɵ�����˺�
    public int minDamage;

    public MonsterCard() { }

    public MonsterCard(CardBase card)
    {
        this.gid = card.gid;
        this.useCardType = card.useCardType;
        this.cardName = card.cardName;
        this.outlookCardName = card.outlookCardName;
    }

    // ��ʼ������
    public void Init()
    {
        // config, id -> cfg
        this.attack = 100;
        this.defense = 100;
        this.hp = 100;
        this.actionNum = 1;
        this.minDamage = 1;
    }
}