using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillBtn : MonoBehaviour , IPointerClickHandler
{
    public int Skill_Index;
    public Button btn;
    public float CoolTime;
    private float CirTempCoolTime;
    public Text Cooltimetext;
    public Image Cool_Image;
    Coroutine CoolRoutine = null;
    bool IsActive = false;
    public Animator animator;


    IEnumerator CoolTimeLogic()
    {

        if(Skill_Index == 4)
            yield return new WaitForSeconds(2.0f);
       
        CirTempCoolTime = CoolTime;
        btn.interactable = false;
        Cool_Image.gameObject.SetActive(true);

        while (CirTempCoolTime > 0.0f)
        {
            CirTempCoolTime -= Time.deltaTime;
            Cooltimetext.text = ((int)CirTempCoolTime).ToString();
            Cool_Image.fillAmount = (1.0f / CoolTime) * CirTempCoolTime;

            yield return null;
        }

        Cool_Image.gameObject.SetActive(false);
        Cool_Image.fillAmount = 1.0f;
        btn.interactable = true;
        CoolRoutine = null;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (Skill_Index == 2)
        { 
            IsActive = !IsActive;
            animator.SetBool("IsActive", IsActive); // ¹Ù²Ù±â
        }

        if (CoolRoutine == null && CoolTime > 0.0f)
            CoolRoutine = StartCoroutine(CoolTimeLogic());
    }
}
