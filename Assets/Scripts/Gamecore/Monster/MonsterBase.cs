using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

// monster base logic
public class MonsterBase
{
    // 怪物唯一id，和将怪物打出的卡片的唯一id相同
    public int id;
    // 怪物名称
    public string monsterName;
    // 怪物所属阵营
    public CampId monsterCampId;

    // 怪物攻击
    private int attack;
    // 怪物防御
    private int defense;
    // 怪物血量
    private int hp;
    // 怪物行动次数
    private int actionNum;
    // 怪物攻击被防御住时造成的最低伤害
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

    // 怪物攻击
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

    // 计算行动次数
    public bool CalcActionNum()
    {
        if (actionNum >= 1)
        {
            actionNum -= 1;
        }
        Debug.Log("can not action: " + actionNum);
        return false;
    }

    // 计算发起方血量
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

    // 计算攻击方血量
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
