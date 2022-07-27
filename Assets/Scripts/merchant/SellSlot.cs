using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SellSlot : MonoBehaviour ,IPointerClickHandler
{
    public Item item;
    public Image Icon;
    public Text ItemCount;
    public Text ItemName;
    public Text SellPrice;

    public GameObject NoTouchPanel;
    public GameObject SellPopup;
    public Text SellCount;

    public GameObject ErrorPopup;
    public Text ErrorText;
    static Item CurItem;

    public Merchant_Manager Sell_Manager;


    public void OnPointerClick(PointerEventData eventData)
    {
        CurItem = this.item;
        if (CurItem != null)
        {
            NoTouchPanel.SetActive(true);
            SellPopup.SetActive(true);
            SellCount.text = "1";
        }
    }

    public void DecideSell()
    {
        if(CurItem != null)
        {
            if(CurItem.ItemCount < int.Parse(SellCount.text))
            {
                ErrorPopup.SetActive(true);
                ErrorText.text = "������ �����մϴ�.";
            }
            else
            {
                GameData.Instance.playerdata.money += (int)CurItem.itemData.SellPrice * int.Parse(SellCount.text); 
                CurItem.ItemCount -= int.Parse(SellCount.text);
                if(CurItem.ItemCount <= 0)
                {
                    GameData.Instance.playerdata.myItems.Remove(CurItem);
                }
                Sell_Manager.OpenShop();
                NoTouchPanel.SetActive(false);
                SellPopup.SetActive(false);

            }
        }
    }


    public void SetItem()
    {
        Icon.sprite = UIManager.Instance.ItemIcon[item.itemData.ItemCode];
        ItemCount.text = "x"+item.ItemCount.ToString();
        ItemName.text = item.itemData.ItemName;
        SellPrice.text = item.itemData.SellPrice.ToString("N0")+" ���";
    }


    public void UpDownCount(int index)
    {
        switch (index)
        {
            case 0:
                if (int.Parse(SellCount.text) != 1)
                {
                    SellCount.text = (int.Parse(SellCount.text) - 1).ToString("N0");
                    // �������� ȿ����
                }
                break;
            case 1:
                SellCount.text = (int.Parse(SellCount.text) + 1).ToString("N0");
                break;

        }

    }
}
