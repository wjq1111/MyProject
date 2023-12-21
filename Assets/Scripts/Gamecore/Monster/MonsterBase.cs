using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

// ����Ļ����߼�
public class MonsterBase
{
    // ����Ψһid���ͽ��������Ŀ�Ƭ��Ψһid��ͬ
    public int id;
    // ��������
    public string monsterName;
    // ����������Ӫ
    public CampId monsterCampId;

    // ���﹥��
    public int attack;
    // �������
    public int defense;
    // ����Ѫ��
    public int hp;
    // �����ж�����
    public int actionNum;
    // ���﹥��������סʱ��ɵ�����˺�
    public int minDamage;

    public void InitMonsterBase()
    {
        monsterName = "lg not init";
        monsterCampId = CampId.Invalid;
        attack = 0;
        defense = 0;
        hp = 1;
        actionNum = 0;
        minDamage = 0;
    }

    public MonsterBase()
    {
        InitMonsterBase();
    }

    public MonsterBase(MonsterCard card, CampId campId)
    {
        InitMonsterBase();
        this.id = card.gid;
        this.monsterCampId = campId;
        this.monsterName = card.outlookCardName;
        this.attack = card.attack;
        this.defense = card.defense;
        this.hp = card.hp;
        this.actionNum = card.actionNum;
        this.minDamage = card.minDamage;
    }

    public bool CostActionNum()
    {
        if (actionNum < 1)
        {
            Debug.LogError("actionNum < 1");
            return false;
        }
        actionNum -= 1;
        return true;
    }

    public void Hurt(int damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            hp = 0;
        }
    }

    // ��ĳ���ּ�����buff
    public void AddAttribute(AttributeCard attributeCard)
    {
        this.attack += attributeCard.addAttack;
        this.defense += attributeCard.addDefense;
        this.hp += attributeCard.addHp;
        this.actionNum += attributeCard.addActionNum;
        Debug.Log("add attribute id:" + attributeCard.gid);
    }

    // �жϹ����Ƿ�����
    public bool IsDead()
    {
        return this.hp <= 0;
    }

    public override string ToString()
    {
        string str = "";
        str += "��������: " + monsterName + "\n";
        str += "gid: " + id + "\n";
        str += "��Ӫ: " + monsterCampId + "\n";
        str += "Ѫ��: " + hp + "\n";
        str += "����: " + attack + "\n";
        str += "����: " + defense + "\n";
        return str;
    }
}
