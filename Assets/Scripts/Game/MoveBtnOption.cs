using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MoveBtnOption : MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler, IDropHandler
{
    public BasicButtons[] BasicButtons;
    public Image Icon;
    public Image DrageIMage;
    public int ConstIndex;
    public int BtnIndex;
    Image DragImage;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        DragImage = DrageIMage.transform.GetChild(0).GetComponent<Image>();
        DragImage.sprite = Icon.sprite;
        DragImage.rectTransform.sizeDelta = Icon.rectTransform.sizeDelta;
        DrageIMage.rectTransform.sizeDelta = this.GetComponent<RectTransform>().sizeDelta;
        DrageIMage.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        DrageIMage.transform.position = eventData.position;
;    }

    public void OnDrop(PointerEventData eventData)
    {
        MoveBtnOption DragBtn = eventData.pointerDrag.GetComponent<MoveBtnOption>();
        MoveBtnOption DragBtn2 = eventData.pointerCurrentRaycast.gameObject.GetComponent<MoveBtnOption>();

        int tempindex = DragBtn.BtnIndex;
        DragBtn.BtnIndex = DragBtn2.BtnIndex;
        DragBtn2.BtnIndex = tempindex;

        DragBtn.ChangeIndex();
        DragBtn2.ChangeIndex(); 

    }

    public void ChangeIndex()
    {

        BasicButtons[ConstIndex].BtnIndex = BtnIndex;
        BasicButtons[ConstIndex].SetBtn();
        Icon.sprite = BasicButtons[ConstIndex].BtnIcon.sprite;
    }





    public void OnEndDrag(PointerEventData eventData)
    {
        DrageIMage.gameObject.SetActive(false);
    }

    

    // Start is called before the first frame update
    void Start()
    {
        BtnIndex = BasicButtons[ConstIndex].BtnIndex;
        Icon.sprite = BasicButtons[BtnIndex].BtnIcon.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
