using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum QuestType
{
    Kill
}

[System.Serializable]
[CreateAssetMenu(fileName ="QuestData" , menuName ="����Ʈ ������",order = int.MinValue+2)]
public class Quest : ScriptableObject
{
    public QuestType type;
    public int Questid;
    public string Questname;
    [TextArea]
    public string Questdescription;
    public SoldierData soldierDatas;
    public int MonsterRequirement;
    public UseItemData Reward;
    
    
    

    
}
