using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipSlot : MonoBehaviour, IDropHandler, IPointerDownHandler,IPointerUpHandler,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    public InventoryManager ivManager;
    public Item EquipItem = null;
    public Image Icon;
    public int ItemCode;
    public Image DragImgae;
    public Knight knight;

    private void Awake()
    {
        //EquipItem = null;

        EquipItem = GameData.Instance.EquipCheck(ItemCode);
    }

    private void Start()
    {
        CheckEquip();
    }

    public void TakeOff()
    {
        if(EquipItem != null)
        {
            SoundManager.Instance.PlayEffect1Shot(13);
            switch (EquipItem.itemData.ItemCode)
            {
                //투구
                case 2:
                    {
                            GameData.Instance.playerdata.DEF -= (int)EquipItem.itemData.value;
                            GameData.Instance.playerdata.MaxHP -= (int)EquipItem.itemData.value * 5;
                            UIManager.Instance.SetHP();
                    }
                    break;
                //신발
                case 3:
                    {
                            GameData.Instance.playerdata.MoveSpeed -= (int)EquipItem.itemData.value;
                            GameData.Instance.playerdata.StatSpeed -= (int)EquipItem.itemData.value;
                    }
                    break;
                //무기
                case 4:
                    {
                            GameData.Instance.playerdata.ATK -= (int)EquipItem.itemData.value;

                    }
                    break;
            }
            EquipItem.IsEquip = false;
            EquipItem = null;
            knight.CheckWeapon();
            Icon.gameObject.SetActive(false);
            ivManager.Changebutton((int)ivManager.slotState);
            UIManager.Instance.SetStatPopup();

        }
    }


    //장착했을때
    public void OnDrop(PointerEventData eventData)
    {
        
        if (ivManager.CurItem != null)
        {

            if (ivManager.CurItem.itemdata.itemData.ItemCode == ItemCode)
            {
                if (EquipItem != null)
                    TakeOff();
                else
                    SoundManager.Instance.PlayEffect1Shot(13);

                EquipItem = ivManager.CurItem.itemdata;
                EquipItem.IsEquip = true;
                Icon.sprite = UIManager.Instance.ItemIcon[EquipItem.itemData.ItemCode];
                Icon.gameObject.SetActive(true);

                switch (ItemCode)
                {
                    //투구
                    case 2:
                        {
                            GameData.Instance.playerdata.DEF += (int)EquipItem.itemData.value;
                            GameData.Instance.playerdata.MaxHP += (int)EquipItem.itemData.value * 5;
                            UIManager.Instance.SetHP();
                        }
                        break;

                        //신발
                    case 3:
                        {
                            GameData.Instance.playerdata.MoveSpeed += (int)EquipItem.itemData.value;
                            GameData.Instance.playerdata.StatSpeed += (int)EquipItem.itemData.value;
                        }
                        break;
                        //무기
                    case 4:
                        {
                            GameData.Instance.playerdata.ATK += (int)EquipItem.itemData.value;
                            knight.CheckWeapon();
                        }
                        break;
                }
               
                
                ivManager.Changebutton((int)ivManager.slotState);
                UIManager.Instance.SetStatPopup();

            }
            else
                return;
        }
    }

  
    public void CheckEquip()
    {
        knight.CheckWeapon();

        if (EquipItem == null) return;

        Icon.sprite = UIManager.Instance.ItemIcon[EquipItem.itemData.ItemCode];
        Icon.gameObject.SetActive(true);
 
}

    public void OnPointerDown(PointerEventData eventData)
    {
        if (EquipItem != null)
        {
            ivManager.ItemDespanel.transform.position = this.transform.position + new Vector3(180.0f, 40.0f, 0.0f);
            ivManager.ItemDespanel.gameObject.SetActive(true);
            ivManager.ItemDesText[0].text = EquipItem.itemData.ItemName;
            ivManager.ItemDesText[1].text = EquipItem.itemData.ItemDes;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ivManager.ItemDespanel.gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        DragImgae.sprite = UIManager.Instance.ItemIcon[EquipItem.itemData.ItemCode];
        DragImgae.gameObject.SetActive(true);
        DragImgae.transform.position = eventData.position;
    }


    public void OnDrag(PointerEventData eventData)
    {
        DragImgae.transform.position = eventData.position;
    }
   

    public void OnEndDrag(PointerEventData eventData)
    {
        DragImgae.gameObject.SetActive(false);
        TakeOff();
    }
}
