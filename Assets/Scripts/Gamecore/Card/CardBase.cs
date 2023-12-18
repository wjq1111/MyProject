// ��Ƭ����͸��ֿ�Ƭ��ʵ��

// ��Ƭʹ��Ч������
public enum UseCardType
{
    MonsterCard,
    AttributeCard,
    LGCard,
}

public class CardBase
{
    // ȫ��Ψһ�Ŀ�Ƭid
    public int gid;
    // ��Ƭʹ��Ч������
    public UseCardType useCardType;
    // ��Ƭ����
    public string cardName;
    // ��Ƭ��������
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

