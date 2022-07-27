using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equip,Use,Etc,Gold
}


public abstract class ItemData : ScriptableObject
{
    public abstract void UseItem(int ItemCode);

    public int ItemCode;
    public ItemType myType;
    public string ItemName;
    [TextArea]
    public string ItemDes;
    public float value;
    public float BuyPrice;
    public float SellPrice;
    
}
