using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public Item itemdata;
    public Image Icon;
    public GameObject SelectImage;
    public Text ItemCountTx;
    public GameObject Equip;
    public Animator myAnim;
    public Image DragImage;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>().Icon.gameObject.activeSelf)
        {
            DragImage.sprite = itemdata.itemData.ItemImage;
            DragImage.gameObject.SetActive(true);
            DragImage.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragImage.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragImage.gameObject.SetActive(false);
    }
}
