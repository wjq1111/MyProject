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
        // config, id -> cfg
        this.attack = 100;
        this.defense = 100;
        this.hp = 100;
        this.actionNum = 1;
        this.minDamage = 1;
    }
}