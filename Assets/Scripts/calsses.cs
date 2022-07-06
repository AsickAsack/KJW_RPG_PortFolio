using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Common
{
    public static CharInfo myChar;
}

public struct StatusInfo
{
    public int STR;
    public int DEX;
    public int CON;
}

public struct CharInfo
{
    public string cid;
    public string Name;
    public int exp_now;
    public int exp_max;
    public int curr_lev;
    public int next_lev;

    public StatusInfo status;
    public ItemInfo[] hasItems;
}

public struct AccountInfo
{
    public string uid;
    public string token;

    public CharInfo[] Chars;
}

[System.Serializable]
public class ItemInfo
{
    public int ino;
    public string iName;

    public ItemCategory itemCategory;

    public int pow;
    public int pow_min;
    public int pow_max;
}

public class ItemNoneConsumInfo : ItemInfo
{
    public ItemNoneConsumeType iType;
}

public class ItemConsumInfo : ItemInfo
{
    public ItemConsumeType iType;
}

public class ItemEquppedInfo : ItemInfo
{
    public ItemEquppedType iType;
}

[System.Serializable]
public enum ItemCategory
{
    NoneConsume = 0,
    Consume = 1,
    Equpped = 2
}

[System.Serializable]
public enum ItemNoneConsumeType
{
    etc = 0,
    Conins = 1,
    Decal = 2,
}

[System.Serializable]
public enum ItemConsumeType
{
    etc = 0,
    potion = 1,
    eat = 2 
}

[System.Serializable]
public enum ItemEquppedType
{
    Wappone = 0,
    Shield, Two_Hands, Bow
}


