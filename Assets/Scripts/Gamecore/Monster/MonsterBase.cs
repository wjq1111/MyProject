using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public enum MonsterCampId
{
    Invalid,
    Mine,
    AI,
}

// monster base logic
public class MonsterBase : Singleton<MonsterBase>
{
    public string monsterName;
    private MonsterCampId monsterCampId;

    // attributes
    private int attack;
    private int defense;
    private int hp;
    private int actionNum;
    private int minDamage;

    public override void Init()
    {
        monsterName = "lgsb";
        monsterCampId = MonsterCampId.Invalid;
        actionNum = 1;
        minDamage = 1;
    }

    public void NewMonsterBase(string monsterName, MonsterCampId monsterCampId, int attack, int defense, int hp)
    {
        this.monsterName = monsterName;
        this.monsterCampId = monsterCampId;
        this.attack = attack;
        this.defense = defense;
        this.hp = hp;
    }

    public void Fight(ref MonsterBase targetMonster)
    {
        // CalcStatus
        bool canAct = CalcActionNum();
        if (!canAct)
        {
            return;
        }

        CalcThisHp(ref targetMonster);
        CalcTargetHp(ref targetMonster);
    }

    public bool CalcActionNum()
    {
        if (actionNum >= 1)
        {
            actionNum -= 1;
        }
        Debug.Log("can not action: " + actionNum);
        return false;
    }

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

    public string PrintStatus()
    {
        return "name: " + monsterName + "campId: " + monsterCampId + " hp: " + hp + " attack: " + attack + " defense: " + defense;
    }
}
