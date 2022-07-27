using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Skill_panel : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    [Header("[½ºÅ³¿òÂ©]")]

    public int index;
    public GameObject Skill_GiF;
    public Animator Gif_Anim;
    Vector2 Pos;

    private void Awake()
    {
        Pos.x = Skill_GiF.GetComponent<RectTransform>().sizeDelta.x * 0.5f;
        //Pos.y = Skill_GiF.GetComponent<RectTransform>().sizeDelta.y * 0.5f;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        Skill_GiF.SetActive(true);
        Skill_GiF.transform.position = eventData.position + Pos;

        switch (index)
        {
            case 0:
                Gif_Anim.SetTrigger("GoHome");
                break;
            case 1:
                Gif_Anim.SetTrigger("Silence");
                break;
            case 2:
                Gif_Anim.SetTrigger("Wind");
                break;
            case 3:
                Gif_Anim.SetTrigger("Defence");
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Skill_GiF.SetActive(false);
        Gif_Anim.SetBool("IsPlay", false);

    }
}
