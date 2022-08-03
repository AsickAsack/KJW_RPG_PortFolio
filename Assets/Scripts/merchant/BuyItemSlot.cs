using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuyItemSlot : MonoBehaviour ,IPointerDownHandler,IPointerUpHandler
{
    public UseItemData itemdata;
    public Image Icon;
    public Text ItemName;
    public Text ItemPrice;

    public GameObject ItemDesPanel;
    public Text DesItemName;
    public Text DesItemPrice;
    Vector2 MovePos;

    public GameObject BuyPopup;
    public Text Count;
    public GameObject ErrorPopup;
    public Text ErrorMessage;
    public GameObject NotouchPanel;
    static UseItemData CurItemdata;

    public Merchant_Manager Buy_Manager;

    private void Awake()
    {
        
        ItemName.text = itemdata.ItemName;
        ItemPrice.text = itemdata.BuyPrice.ToString("N0") + "골드";
        MovePos = new Vector2(ItemDesPanel.GetComponent<RectTransform>().sizeDelta.x * 0.5f, ItemDesPanel.GetComponent<RectTransform>().sizeDelta.y * 0.5f);
        
    }
    private void Start()
    {
        Icon.sprite = UIManager.Instance.ItemIcon[itemdata.ItemCode];
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DesItemName.text = itemdata.ItemName;
        DesItemPrice.text = itemdata.ItemDes;
        ItemDesPanel.gameObject.SetActive(true);
        ItemDesPanel.transform.position = eventData.position+MovePos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ItemDesPanel.gameObject.SetActive(false);
    }

    public void BuyBtn()
    {
        SoundManager.Instance.PlayEffect1Shot(10);
        CurItemdata = this.itemdata;
        NotouchPanel.gameObject.SetActive(true);
        BuyPopup.SetActive(true);
        Count.text = "1";
    }

    public void BuyDecide()
    {
        if (CurItemdata != null)
        {
            if (GameData.Instance.playerdata.money >= CurItemdata.BuyPrice * int.Parse(Count.text))
            {
                if (GameData.Instance.playerdata.myItems.Count+int.Parse(Count.text) > 20)
                {
                    ErrorPopup.SetActive(true);
                    ErrorMessage.text = "인벤토리 공간이 부족합니다.";
                    SoundManager.Instance.PlayEffect1Shot(11);
                }
                else
                {
                    GameData.Instance.playerdata.money -= (int)CurItemdata.BuyPrice * int.Parse(Count.text);

                    if (CurItemdata.myType == ItemType.Equip)
                    {
                        for(int i=0;i< int.Parse(Count.text);i++)
                        GameData.Instance.playerdata.myItems.Add(new(CurItemdata));
                    }
                    else
                    {
                        Item temp = GameData.Instance.playerdata.myItems.Find(x => x.itemData.ItemCode == CurItemdata.ItemCode);

                        if(temp != null)
                        {
                            temp.ItemCount += int.Parse(Count.text);
                        }
                        else
                        {
                            GameData.Instance.playerdata.myItems.Add(new(CurItemdata, int.Parse(Count.text)));
                        }


                    }

                    GameData.Instance.ReQuick?.Invoke();
                    NotouchPanel.gameObject.SetActive(false);
                    BuyPopup.SetActive(false);
                    Buy_Manager.OpenShop();
                    SoundManager.Instance.PlayEffect1Shot(20);

                }

            }
            else
            {
                ErrorPopup.SetActive(true);
                ErrorMessage.text = "돈이 부족합니다.";
                SoundManager.Instance.PlayEffect1Shot(11);
            }
        }

        
    }

    public void UpDownCount(int index)
    {
        switch(index)
        {
            case 0:
                if(int.Parse(Count.text) != 1)
                {
                    Count.text = (int.Parse(Count.text)-1).ToString("N0");
                }
                break;
            case 1:
                Count.text = (int.Parse(Count.text) + 1).ToString("N0");
                break;

        }
        SoundManager.Instance.PlayEffect1Shot(10);

    }





}
