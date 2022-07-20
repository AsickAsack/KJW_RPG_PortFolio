using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public ItemData itemData;
    public int ItemCount;
    public int UID;
    public bool IsEquip;

    //장비용 생성자
    public Item(ItemData itemdata)
    {
        this.itemData = itemdata;
        this.ItemCount = 1;
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
    public Item(ItemData itemdata, int count)
    {
        this.itemData = itemdata;
        this.ItemCount = count;
        UID = 0;

    }

}
 

