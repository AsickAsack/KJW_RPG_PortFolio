using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BasicButtons : MonoBehaviour , IPointerDownHandler, IPointerUpHandler
{
    public int BtnIndex;
    public Image BtnIcon;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("BtnIndex" + BtnIndex))
        {
            BtnIndex = PlayerPrefs.GetInt("BtnIndex" + BtnIndex);
        }
       
    }

    private void Start()
    {
        BtnIcon.sprite = UIManager.Instance.ButtonImage[BtnIndex];
    }

    public void SetBtn()
    {
        BtnIcon.sprite = UIManager.Instance.ButtonImage[BtnIndex];
        PlayerPrefs.SetInt("BtnIndex"+BtnIndex, BtnIndex);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UIManager.Instance.ButtonFunc[BtnIndex]?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       if(BtnIndex == 1)
            UIManager.Instance.ButtonFunc[BtnIndex]?.Invoke();
    }

}
