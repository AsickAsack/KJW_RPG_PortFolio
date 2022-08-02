using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillBtn : MonoBehaviour , IPointerClickHandler
{
    public int Skill_Index;
    public int Skill_Mp;
    public Button btn;
    public float CoolTime;
    private float CirTempCoolTime;
    public Text Cooltimetext;
    public Image Cool_Image;
    Coroutine CoolRoutine = null;
    bool IsActive = false;
    public Animator animator;
    public Knight knight;
    bool IsSkill = false;
  

    IEnumerator CoolTimeLogic()
    {
        

        if (Skill_Index == 4)
            yield return new WaitForSeconds(2.0f);
        btn.interactable = false;
        CirTempCoolTime = CoolTime;
        
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
        IsSkill = false;
        CoolRoutine = null;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        //mp있을경우
        if (Skill_Mp <= GameData.Instance.playerdata.CurMP && !IsSkill && !knight.IsSkill)
        {
            if (CoolRoutine == null && CoolTime > 0.0f)
            { 
                IsSkill = true;
                CoolRoutine = StartCoroutine(CoolTimeLogic());
            }
            switch (Skill_Index)
            {
                case 1:
                    if (GameData.Instance.playerdata.KingFight)
                    { 
                        SoundManager.Instance.PlayEffect1Shot(11);
                        GameData.Instance.SetNotify("보스와의 전투중에는 쓸 수 없습니다!");
                        return;
                    }
                    knight.WarpToHome();
                    break;
                case 2:
                    knight.SilentModeBtn();
                    break;
                case 3:
                    knight.SpecialAttack();
                    break;
                case 4:
                    knight.skillbtn();
                    break;
            }

            GameData.Instance.playerdata.CurMP -= Skill_Mp;
            UIManager.Instance.SetMp();

         
        }
        else
        {
            SoundManager.Instance.PlayEffect1Shot(11);
        }

    }


    public void SilentAnim()
    {
        IsActive = !IsActive;
        animator.SetBool("IsActive", IsActive); 
    }

}
