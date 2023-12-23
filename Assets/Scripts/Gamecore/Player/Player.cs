using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player
{
    // player camp
    public CampId campId;

    // player card bag
    public List<CardBase> cardBag;
    // game card deck
    public List<CardBase> handCardList;

    // max num of monsters
    public int maxMonsterNum;
    // max num of card
    public int maxCardNum;
    // face
    public MonsterBase face;
    // in-game monsters
    public List< MonsterBase> monsterList;
    // each round draw card number
    public int eachRoundDrawCardNum;

    public void Init()
    {
        handCardList = new List<CardBase>();
        // TODO config
        maxMonsterNum = 4;
        eachRoundDrawCardNum = 2;
        maxCardNum = 10;

        face = new MonsterBase();
        monsterList = new List<MonsterBase>();

        InitCardBag();
    }

    // ��ʼ����Ƭ����
    private void InitCardBag()
    {
        cardBag = new List<CardBase>();
        foreach (var playerCard in ConfigManager.Instance.playerCardBagMap)
        {
            CardBase card = new CardBase();
            card.Init(playerCard.Value.Id);
            cardBag.Add(card);
        }
    }

    public bool IsDead()
    {
        return face.IsDead();
    }

    // ��ȡ����
    public MonsterBase GetMonster(int index)
    {
        if (index >= monsterList.Count)
        {
            return null;
        }
        return monsterList[index];
    }

    // ��ȡ��Ƭ
    public CardBase GetCard(int cardId)
    {
        foreach (var card in handCardList)
        {
            if (cardId == card.gid)
            {
                return card;
            }
        }
        return null;
    }

    // ��ȡ�����ڳ������±�
    public List<int> GetAllMonsterIndex()
    {
        List<int> result = new List<int>();
        for (int i = 0; i < monsterList.Count; i++)
        {
            result.Add(i);
        }
        return result;
    }

    // �������״̬
    public void CalcStatus()
    {
        List<MonsterBase> removeMonster = new List<MonsterBase>();
        foreach (var monster in monsterList)
        {
            if (monster.IsDead())
            {
                removeMonster.Add(monster);
            }
        }
        foreach (var monster in removeMonster)
        {
            monsterList.Remove(monster);
        }

        // ֪ͨuiˢ��
        EventManager.Instance.DispatchEvent(EventId.FlushDebugStatus);
    }

    // ��ʼ����Ϸ����ʱ����
    public void InitGameStartMonsters()
    {
        // TODO use config to init monsters, now directly write them
        for (int i = 0; i < maxMonsterNum; i++)
        {
            MonsterCard monsterCard = new MonsterCard();
            monsterCard.Init();
            MonsterBase monster = new MonsterBase(monsterCard, campId);
            monsterList.Add(monster);
        }
    }

    // ʹ�ù��￨
    public void UseMonsterCard(MonsterCard monsterCard)
    {
        if (monsterList.Count >= maxMonsterNum)
        {
            Debug.LogError("index too large");
            return;
        }
        MonsterBase newMonster = new MonsterBase(monsterCard, campId);
        monsterList.Add(newMonster);

        // ֪ͨuiˢ��
        EventManager.Instance.DispatchEvent(EventId.FlushDebugStatus);
    }

    public AttributeCard AddAttributeCardBuff(AttributeCard card)
    {
        // TODO ��������
        return card;
    }

    // ʹ�����Կ�        
    public void UseAttributeCard(AttributeCard attributeCard, List<int> indexs)
    {
        foreach (int index in indexs)
        {
            GetMonster(index).AddAttribute(attributeCard);
        }

        // ֪ͨuiˢ��
        EventManager.Instance.DispatchEvent(EventId.FlushDebugStatus);
    }

    public void RemoveCard(int cardId)
    {
        CardBase card = GetCard(cardId);
        if (card == null)
        {
            return;
        }
        handCardList.Remove(card);

        // ֪ͨuiˢ��
        EventManager.Instance.DispatchEvent(EventId.FlushDebugStatus);
    }

    // ÿ�غϳ鿨
    public void EachRoundDrawCard()
    {
        List<CardBase> canDrawCard = CalcCanDrawCard();

        if (canDrawCard == null)
        {
            return;
        }
        if (canDrawCard.Count < eachRoundDrawCardNum)
        {
            Debug.Log("can draw less than each round draw" + canDrawCard.Count + " " + eachRoundDrawCardNum);
        }

        for (int i = 0; i < Mathf.Min(eachRoundDrawCardNum, canDrawCard.Count); i++)
        {
            handCardList.Add(canDrawCard[i]);
            Debug.Log("player: " + this.campId + " each round draw card:" + canDrawCard[i].cardName);
        }

        // ֪ͨuiˢ��
        EventManager.Instance.DispatchEvent(EventId.FlushDebugStatus);
    }

    // ������Գ鿨�ļ���
    private List<CardBase> CalcCanDrawCard()
    {
        List<CardBase> result = new List<CardBase>();
        foreach (var card in cardBag)
        {
            bool found = false;
            foreach (var monster in monsterList)
            {
                if (card.gid == monster.id)
                {
                    found = true;
                    break;
                }
            }
            if (found)
            {
                continue;
            }

            foreach (var handCard in handCardList)
            {
                if (card.gid == handCard.gid)
                {
                    found = true;
                    break;
                }
            }
            if (found)
            {
                continue;
            }

            result.Add(card);
        }

        if (result.Count == 0)
        {
            Debug.Log("no more card to draw");
            return null;
        }

        // shuffle result
        result = Shuffle(result);
        return result;
    }

    private List<T> Shuffle<T>(List<T> original)
    {
        System.Random random = new System.Random();
        T temp;
        for (int i = 0; i < original.Count; i++)
        {
            int index = random.Next(0, original.Count - 1);
            if (index != i)
            {
                temp = original[i];
                original[i] = original[index];
                original[index] = temp;
            }
        }
        return original;
    }

    private string CardBagToString()
    {
        string str = "";
        str += "��Ӫ" + this.campId + "����" + "\n";
        foreach (CardBase card in this.cardBag)
        {
            str += card.ToString();
        }
        str += '\n';
        return str;
    }

    public override string ToString()
    {
        string str = "";
        str += "��:" + "\n";
        str += face.ToString() + "\n";
        str += "��Ӫ��" + this.campId + "\n";
        str += "������:\n";
        foreach (CardBase card in this.handCardList)
        {
            str += card.ToString() + "\n";
        }
        str += "�ϳ�������:\n";
        foreach (var monster in this.monsterList)
        {
            str += monster.ToString() + "\n";
        }
        str += "\n";
        return str;
    }
}
