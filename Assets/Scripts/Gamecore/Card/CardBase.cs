// 卡片基类和各种卡片的实现

// 卡片使用效果类型
public enum UseCardType
{
    MonsterCard,
    AttributeCard,
    LGCard,
}

public class CardBase
{
    // 全局唯一的卡片id
    public int gid;
    // 卡片使用效果类型
    public UseCardType useCardType;
    // 卡片名称
    public string cardName;
    // 卡片外显名称
    public string outlookCardName;

    public CardBase()
    {
        this.gid = AllocGid.Instance.Alloc();
    }

    public override string ToString()
    {
        string result = "";
        result += "card: " + cardName + "\n";
        result += "gid: " + gid + "\n";
        result += "useCardType" + useCardType + "\n";
        return result;
    }
}

