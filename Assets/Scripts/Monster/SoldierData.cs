using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Soldier_Data",menuName = "솔저 데이터",order = int.MinValue)]
public class SoldierData : ScriptableObject
{
    public string SoldierName;
    public float MaxHP;
    public float HP;
    public float ATK;
    public float DEF;
    public float Speed;
    public float AttackDelay;
    public float EXP;

}
