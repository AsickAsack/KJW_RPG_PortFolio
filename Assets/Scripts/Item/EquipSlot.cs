using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipSlot : MonoBehaviour, IDropHandler, IPointerDownHandler,IPointerUpHandler,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    public InventoryManager ivManager;
    public Item EquipItem;
    public Image Icon;
    public int ItemCode;
    public Image DragImgae;
    

    private void Awake()
    {
        CheckEquip();
        
    }

    public void TakeOff()
    {
        if(EquipItem != null)
        {
            switch (EquipItem.itemData.ItemCode)
            {
                case 2:
                    GameData.Instance.playerdata.Helmet = null;
                    break;
                case 3:
                    GameData.Instance.playerdata.Shoes = null;
                    break;
                case 4:
                    GameData.Instance.playerdata.Weapon = null;
                    break;

            }
            EquipItem.IsEquip = false;
            EquipItem = null;
            Icon.gameObject.SetActive(false);
            ivManager.Changebutton((int)ivManager.slotState);
            UIManager.Instance.SetStatPopup();

        }
    }



    public void OnDrop(PointerEventData eventData)
    {
        
        if (ivManager.CurItem != null)
        {

            EquipItem = ivManager.CurItem.itemdata;

            if (EquipItem.itemData.ItemCode == ItemCode)
            {
        
                Icon.sprite = EquipItem.itemData.ItemImage;
                Icon.gameObject.SetActive(true);

                switch(ItemCode)
                {
                    //투구
                    case 2:
                        if(GameData.Instance.playerdata.Helmet != null)
                            GameData.Instance.playerdata.Helmet.IsEquip = false;
                        GameData.Instance.playerdata.Helmet = EquipItem;
                        GameData.Instance.playerdata.Helmet.IsEquip = true;
                        break;

                        //신발
                    case 3:
                        if (GameData.Instance.playerdata.Shoes != null)
                            GameData.Instance.playerdata.Shoes.IsEquip = false;
                        GameData.Instance.playerdata.Shoes = EquipItem;
                        GameData.Instance.playerdata.Shoes.IsEquip = true;
                        break;
                        //무기
                    case 4:
                        if (GameData.Instance.playerdata.Weapon != null)
                            GameData.Instance.playerdata.Weapon.IsEquip = false;
                        GameData.Instance.playerdata.Weapon = EquipItem;
                        GameData.Instance.playerdata.Weapon.IsEquip = true;
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
        switch (ItemCode)
        {
            case 2:
                if (GameData.Instance.playerdata.Helmet != null)
                {
                    EquipItem = GameData.Instance.playerdata.Helmet;
                    Icon.sprite = EquipItem.itemData.ItemImage;
                    Icon.gameObject.SetActive(true);
                }
                break;
            case 3:
                if (GameData.Instance.playerdata.Shoes != null)
                {
                    EquipItem = GameData.Instance.playerdata.Shoes;
                    Icon.sprite = EquipItem.itemData.ItemImage;
                    Icon.gameObject.SetActive(true);
                }
                break;
            case 4:
                if (GameData.Instance.playerdata.Weapon != null)
                {
                    EquipItem = GameData.Instance.playerdata.Weapon;
                    Icon.sprite = EquipItem.itemData.ItemImage;
                    Icon.gameObject.SetActive(true);
                }
                break;

        }
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
        DragImgae.sprite = EquipItem.itemData.ItemImage;
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
