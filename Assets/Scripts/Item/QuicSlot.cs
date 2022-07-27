using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuicSlot : MonoBehaviour, IPointerClickHandler
{
    public int index;
    Item SlotItem;
    public Image Icon;
    public Text ItemCounttx;


    private void Awake()
    {
        CheckLoadQuick();
    }

    public void CheckLoadQuick()
    {
        if(GameData.Instance.playerdata.QuickSlot[index] != null)
        {
            SetQuickSlot(GameData.Instance.playerdata.QuickSlot[index]);
        }
    }

    public void SetQuickSlot(Item item)
    {
        SlotItem = item;
        Icon.sprite = UIManager.Instance.ItemIcon[SlotItem.itemData.ItemCode];
        ItemCounttx.text = SlotItem.ItemCount.ToString();
        Icon.gameObject.SetActive(true);
        ItemCounttx.gameObject.SetActive(true);
        GameData.Instance.playerdata.QuickSlot[index] = item;
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
