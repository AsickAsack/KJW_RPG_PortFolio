using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemSlotManager : MonoBehaviour
{
    public DropItem Dropitem;
    public DropItemSlot[] dropItemSlots;
    public GameObject DropItemPanel;
    private string Not;


    public void ExitPopUp()
    {
        for(int i=0;i< dropItemSlots.Length;i++)
        {
            dropItemSlots[i].gameObject.SetActive(false);
        }
    }


    // √∑ø° ø≠∑»¿ª∂ß
    public void openDropItem()
    {
        DropItemPanel.SetActive(true);

        for(int i=0;i<Dropitem.itemData.Count;i++)
        {
            if (!dropItemSlots[i].gameObject.activeSelf)
            {
                dropItemSlots[i].ItemNameText.text = Dropitem.itemData[i].myType == ItemType.Gold ? Dropitem.itemData[i].value.ToString() + " ∞ÒµÂ" : Dropitem.itemData[i].ItemName;
                dropItemSlots[i].Icon.sprite = Dropitem.itemData[i].ItemImage;
                dropItemSlots[i].gameObject.SetActive(true);
            }
        }
    }

    //¿¸∫Œ »πµÊ «ﬂ¿ª∂ß
    public void AllGetItem()
    {


        if(Dropitem.itemData.Count+GameData.Instance.playerdata.myItems.Count > 12)
        {
            GameData.Instance.SetNotify("¿Œ∫•≈‰∏Æ∞° ≤À√°Ω¿¥œ¥Ÿ.");
            //æ»µ«¥¬ ªÁøÓµÂ

            return;
        }


        for(int i=0;i<Dropitem.itemData.Count;i++)
        {
            
            if (Dropitem.itemData[i].myType == ItemType.Equip)
                GameData.Instance.playerdata.myItems.Add(new Item(Dropitem.itemData[i]));
            else if (Dropitem.itemData[i].myType == ItemType.Gold)
            {
                GameData.Instance.playerdata.money += (int)Dropitem.itemData[i].value;
            }
            else
            {
                Item temp = GameData.Instance.playerdata.myItems.Find(x => x.itemData == Dropitem.itemData[i]);
                if (temp == null)
                    GameData.Instance.playerdata.myItems.Add(new Item(Dropitem.itemData[i]));
                else
                    temp.ItemCount++;
            }

            Not = Dropitem.itemData[i].myType == ItemType.Gold ? Dropitem.itemData[i].value.ToString() + " ∞ÒµÂ∏¶ »πµÊ«ﬂΩ¿¥œ¥Ÿ." : Dropitem.itemData[i].ItemName + " 1∞≥∏¶ »πµÊ«ﬂΩ¿¥œ¥Ÿ.";
            GameData.Instance.SetNotify(Not);

        }
        Dropitem.itemData.Clear();
    }


    //«œ≥™∏∏ ¥≠∑∂¿ª∂ß
    public void ClickItem(int index)
    {
   

        if (GameData.Instance.playerdata.myItems.Count+1 > 12)
        {
            GameData.Instance.SetNotify("¿Œ∫•≈‰∏Æ∞° ≤À√°Ω¿¥œ¥Ÿ.");
            //æ»µ«¥¬ ªÁøÓµÂ

            return;
        }


        if (Dropitem.itemData[index] != null)
        {

            if (Dropitem.itemData[index].myType == ItemType.Equip)
                GameData.Instance.playerdata.myItems.Add(new Item(Dropitem.itemData[index]));
            else if (Dropitem.itemData[index].myType == ItemType.Gold)
            {
                GameData.Instance.playerdata.money += (int)Dropitem.itemData[index].value;
            }
            else
            {
                Item temp = GameData.Instance.playerdata.myItems.Find(x => x.itemData == Dropitem.itemData[index]);
                if (temp == null)
                    GameData.Instance.playerdata.myItems.Add(new Item(Dropitem.itemData[index]));
                else
                    temp.ItemCount++;
            }

            Not = Dropitem.itemData[index].myType == ItemType.Gold ? Dropitem.itemData[index].value.ToString() + " ∞ÒµÂ∏¶ »πµÊ«ﬂΩ¿¥œ¥Ÿ." : Dropitem.itemData[index].ItemName + "1∞≥∏¶ »πµÊ«ﬂΩ¿¥œ¥Ÿ.";
            GameData.Instance.SetNotify(Not);
            Dropitem.itemData.RemoveAt(index);
            ExitPopUp();
            openDropItem();
        }
    }
}
