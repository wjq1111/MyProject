using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

// monster base logic
public class MonsterBase
{
    // ����Ψһid���ͽ��������Ŀ�Ƭ��Ψһid��ͬ
    public int id;
    // ��������
    public string monsterName;
    // ����������Ӫ
    public CampId monsterCampId;

    // ���﹥��
    private int attack;
    // �������
    private int defense;
    // ����Ѫ��
    private int hp;
    // �����ж�����
    private int actionNum;
    // ���﹥��������סʱ��ɵ�����˺�
    private int minDamage;

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

    // ���﹥��
    public void Fight(ref MonsterBase targetMonster)
    {
        bool canAct = CalcActionNum();
        if (!canAct)
        {
            return;
        }

        CalcThisHp(ref targetMonster);
        CalcTargetHp(ref targetMonster);
    }

    // �����ж�����
    public bool CalcActionNum()
    {
        if (actionNum >= 1)
        {
            actionNum -= 1;
        }
        Debug.Log("can not action: " + actionNum);
        return false;
    }

    // ���㷢��Ѫ��
    public void CalcThisHp(ref MonsterBase targetMonster)
    {
        // mine.hp = mine.hp - (target.attack - mine.defense)
        int damage = targetMonster.attack - this.defense;
        if (damage < 0)
        {
            Debug.Log("this " + this.monsterName + " attack target" + targetMonster.monsterName);
            Debug.Log("can not damage, target.attack" + targetMonster.attack + " this.defense " + this.defense);
            damage = 0;
        }
        this.hp = this.hp - damage;
    }

    // ���㹥����Ѫ��
    public void CalcTargetHp(ref MonsterBase targetMonster)
    {
        // target.hp = target.hp - (mine.attack - target.defense)
        int damage = this.attack - targetMonster.defense;
        if (damage < 0)
        {
            Debug.Log("this " + this.monsterName + " attack target" + targetMonster.monsterName);
            Debug.Log("can not damage, this.attack: " + this.attack + " target.defense: " + targetMonster.defense);
            damage = this.minDamage;
        }
        targetMonster.hp = targetMonster.hp - damage;
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

    public string PrintMonster()
    {
        string result = "";
        result += "name: " + monsterName + "\n";
        result += "gid: " + id + "\n";
        result += "campId: " + monsterCampId + "\n";
        result += "hp: " + hp + "\n";
        result += "attack: " + attack + "\n";
        result += "defense: " + defense + "\n";
        return result;
    }
}
