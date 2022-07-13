using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour , IPointerDownHandler, IPointerUpHandler
{
    public enum SlotState
    {
        All,Weapon,Use,Etc
    }


    public InventorySlot[] Slot;
    public List<Item> items = new List<Item>();
    public SlotState slotState = SlotState.All;
    public InventorySlot CurItem;
    public GameObject[] ButtonSelectImage;
    public GameObject ItemDespanel;
    public Text[] ItemDesText;
    public GameObject QuickRegisterBtn;
    public GameObject[] QuickSlot;
    public Text GoldText;

    private void Awake()
    {
        Slot = this.transform.GetComponentsInChildren<InventorySlot>();
    }

    public void OpenInventory()
    {
        slotState = SlotState.All;
        ButtonSelectImage[0].SetActive(true);
        GoldText.text = GameData.Instance.playerdata.money.ToString("N0") + " °ñµå";

        for (int i=0;i<GameData.Instance.playerdata.myItems.Count;i++)
        {
            Slot[i].itemdata = GameData.Instance.playerdata.myItems[i];
            Slot[i].Icon.sprite = GameData.Instance.playerdata.myItems[i].itemData.ItemImage;
            Slot[i].Icon.gameObject.SetActive(true);
            if(GameData.Instance.playerdata.myItems[i].itemData.myType == ItemType.Equip)
            {
                if (GameData.Instance.playerdata.myItems[i].IsEquip)
                {
                    Slot[i].Equip.SetActive(true);
                }
            }
            else
            {
                Slot[i].ItemCountTx.text = GameData.Instance.playerdata.myItems[i].ItemCount.ToString("N0");
                Slot[i].ItemCountTx.gameObject.SetActive(true);
            }
            
        }
    }

    public void Changebutton(int index)
    {
        for(int i=0;i<ButtonSelectImage.Length;i++)
        {
            ButtonSelectImage[i].SetActive(i == index);
        }

        switch(index)
        {

            case 0:
                ResetSlots();
                OpenInventory();
                break;

            case 1:
                slotState = SlotState.Weapon;
                ChangeSlot(ItemType.Equip);
                break;
            case 2:
                slotState = SlotState.Use;
                ChangeSlot(ItemType.Use);
                break;
            case 3:
                slotState = SlotState.Etc;
                ChangeSlot(ItemType.Etc);
                break;
        }


    }


    public void ChangeSlot(ItemType itemType)
    {
        ResetSlots();
        if(items != null)
        items.Clear();
        QuickRegisterBtn.SetActive(false);

        items = GameData.Instance.playerdata.myItems.FindAll(x => x.itemData.myType == itemType);

        for (int i = 0; i < items.Count; i++)
        {
            Slot[i].itemdata = items[i];
            Slot[i].Icon.sprite = items[i].itemData.ItemImage;
            Slot[i].Icon.gameObject.SetActive(true);
            if (items[i].itemData.myType == ItemType.Equip)
            {
                if (items[i].IsEquip)
                {
                    Slot[i].Equip.SetActive(true);
                }
            }
            else
            {
                Slot[i].ItemCountTx.text = GameData.Instance.playerdata.myItems[i].ItemCount.ToString("N0");
                Slot[i].ItemCountTx.gameObject.SetActive(true);
            }
        }
    }

    public void ResetSlots()
    {
        if (CurItem != null)
        {
            CurItem.myAnim.SetBool("IsSelect", false);
            CurItem.SelectImage.SetActive(false);
        }
        for (int i=0;i<Slot.Length;i++)
        {
            Slot[i].Icon.gameObject.SetActive(false);
            Slot[i].ItemCountTx.gameObject.SetActive(false);
            Slot[i].Equip.SetActive(false);
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>() != null)
        {
            QuickRegisterBtn.SetActive(false);
            if (CurItem != null)
            {
                CurItem.myAnim.SetBool("IsSelect", false);
                CurItem.SelectImage.SetActive(false);   
            }

            CurItem = eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>();

            if (CurItem.Icon.gameObject.activeSelf)
            {
                CurItem.myAnim.SetBool("IsSelect", true);
                CurItem.SelectImage.SetActive(true);
                ItemDespanel.transform.position = CurItem.transform.position+new Vector3(180.0f, 40.0f, 0.0f);
                ItemDespanel.gameObject.SetActive(true);
                ItemDesText[0].text = CurItem.itemdata.itemData.ItemName;
                ItemDesText[1].text = CurItem.itemdata.itemData.ItemDes;

                if(CurItem.itemdata.itemData.myType == ItemType.Use)
                {
                    QuickRegisterBtn.transform.position = CurItem.transform.position - new Vector3(100.0f, 0.0f, 0.0f);
                    QuickRegisterBtn.SetActive(true);
                }
            }
        }
      
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (CurItem != null)
        {
            ItemDespanel.gameObject.SetActive(false);
        }
    }

    public void QuickRegister(int index)
    {
        QuickSlot[index].GetComponent<QuicSlot>().SetQuickSlot(CurItem.itemdata);


    }

    public void InVenUseItem()
    {
        if (CurItem != null)
        {
            if (CurItem.itemdata.ItemCount > 0)
            {
                CurItem.itemdata.itemData.UseItem(CurItem.itemdata.itemData.ItemCode);
                CurItem.itemdata.ItemCount--;
                if (CurItem.itemdata.ItemCount == 0)
                {
                    GameData.Instance.playerdata.myItems.Remove(CurItem.itemdata);
                    QuickRegisterBtn.SetActive(false);
                }
                Changebutton((int)slotState);
                QuickSlot[0].GetComponent<QuicSlot>().RePrintQuickSlot();
                QuickSlot[1].GetComponent<QuicSlot>().RePrintQuickSlot();
            }
        }
    }

}
