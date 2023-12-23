using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameConf", menuName = "GameConf")]
public class GameConf : ScriptableObject
{
    [Tooltip("玩家手牌区按钮")]
    public GameObject PlayerCardButton;

    [Tooltip("场上怪物按钮")]
    public GameObject MonsterButton;
}
