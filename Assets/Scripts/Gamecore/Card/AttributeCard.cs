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
    }
}
