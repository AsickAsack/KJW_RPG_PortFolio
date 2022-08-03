using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


[System.Serializable]
public class Item
{
    
    public UseItemData itemData;
    public int ItemCount;
    public int UID;
    public bool IsEquip;
    public int QuickIndex;

    public Item()
    {

    }


    //장비용 생성자
    public Item(UseItemData itemdata)
    {
        this.itemData = itemdata;
        this.ItemCount = 1;
        this.QuickIndex = 0;
        if (itemData.myType == ItemType.Equip)
        {
            while(true)
            { 
                this.UID = Random.Range(int.MinValue, int.MaxValue);
                Item temp = GameData.Instance.playerdata.myItems.Find(x => x.UID == this.UID);
                if (temp == null)
                    break;
            }
        }
        else
            UID = 0;
        
    }

    //소비템용 생성자
    public Item(UseItemData itemdata, int count)
    {
        this.itemData = itemdata;
        this.ItemCount = count;
        this.QuickIndex=0;
        UID = 0;

    }

}
 

