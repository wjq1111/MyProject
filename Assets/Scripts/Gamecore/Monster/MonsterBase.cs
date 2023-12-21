using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

// 怪物的基础逻辑
public class MonsterBase
{
    // 怪物唯一id，和将怪物打出的卡片的唯一id相同
    public int id;
    // 怪物名称
    public string monsterName;
    // 怪物所属阵营
    public CampId monsterCampId;

    // 怪物攻击
    public int attack;
    // 怪物防御
    public int defense;
    // 怪物血量
    public int hp;
    // 怪物行动次数
    public int actionNum;
    // 怪物攻击被防御住时造成的最低伤害
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

    // 给某个怪加属性buff
    public void AddAttribute(AttributeCard attributeCard)
    {
        this.attack += attributeCard.addAttack;
        this.defense += attributeCard.addDefense;
        this.hp += attributeCard.addHp;
        this.actionNum += attributeCard.addActionNum;
        Debug.Log("add attribute id:" + attributeCard.gid);
    }

    // 判断怪物是否死亡
    public bool IsDead()
    {
        return this.hp <= 0;
    }

    public override string ToString()
    {
        string str = "";
        str += "怪物名称: " + monsterName + "\n";
        str += "gid: " + id + "\n";
        str += "阵营: " + monsterCampId + "\n";
        str += "血量: " + hp + "\n";
        str += "攻击: " + attack + "\n";
        str += "防御: " + defense + "\n";
        return str;
    }
}
