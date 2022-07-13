using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuicSlot : MonoBehaviour, IPointerClickHandler
{
    Item SlotItem;
    public Image Icon;
    public Text ItemCounttx;


    public void SetQuickSlot(Item item)
    {
        SlotItem = item;
        Icon.sprite = SlotItem.itemData.ItemImage;
        ItemCounttx.text = SlotItem.ItemCount.ToString();
        Icon.gameObject.SetActive(true);
        ItemCounttx.gameObject.SetActive(true);

    }



    public void OnPointerClick(PointerEventData eventData)
    {
        if (SlotItem != null)
        {
            if(SlotItem.ItemCount>0)
            { 
                SlotItem.itemData.UseItem(SlotItem.itemData.ItemCode);
                SlotItem.ItemCount--;
                if (SlotItem.ItemCount == 0)
                { 
                    GameData.Instance.playerdata.myItems.Remove(SlotItem);
                    Icon.gameObject.SetActive(false);
                    ItemCounttx.gameObject.SetActive(false);
                }
                else
                    ItemCounttx.text = SlotItem.ItemCount.ToString();
            }
        }

    }

    public void RePrintQuickSlot()
    {
        if (SlotItem != null)
        {
            if (SlotItem.ItemCount == 0)
            {
                Icon.gameObject.SetActive(false);
                ItemCounttx.gameObject.SetActive(false);
            }
            else
                ItemCounttx.text = SlotItem.ItemCount.ToString();
        }
    }

}
