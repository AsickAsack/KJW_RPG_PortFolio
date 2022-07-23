using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum QuestType
{
    Kill
}

[System.Serializable]
[CreateAssetMenu(fileName ="QuestData" , menuName ="퀘스트 데이터",order = int.MinValue+2)]
public class Quest : ScriptableObject
{
    public QuestType type;
    public int Questid;
    public string Questname;
    [TextArea]
    public string Questdescription;
    public SoldierData soldierDatas;
    public int MonsterRequirement;
    public ItemData Reward;
    
    
    

    
}
