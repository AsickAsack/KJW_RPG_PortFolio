using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class CineBridge : MonoBehaviour
{
    public PlayableDirector Director;
    public Canvas[] ExitCanavses;
    public BossKing King;
    public GameObject KingHPbar;
    public GameObject NotifyPopup;
    public GameObject NoTouchPanel;
    float OrgVolume;
    Knight knight;
    public GameObject PreventRun;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            knight = other.GetComponent<Knight>();
            NotifyPopup.SetActive(true);
            NoTouchPanel.SetActive(true);
        }
    }

    public void EnterKingbtn()
    {
        SoundManager.Instance.SetBGM(SoundManager.Instance.myBgmClip[1], true);
        OrgVolume = SoundManager.Instance.EffectVolume;
        SoundManager.Instance.EffectVolume = 0.0f;
        knight.myJoystic.MoveOn = false;
        for (int i = 0; i < ExitCanavses.Length; i++)
        {
            ExitCanavses[i].enabled = false;
        }
        Director.Play();
    }

    public void CancelEnter()
    {
        knight.transform.position = new Vector3(knight.transform.position.x, knight.transform.position.y, knight.transform.position.z - 2.0f);
    }

    public void StartKingFight()
    {
        SoundManager.Instance.EffectVolume = OrgVolume;
        for (int i = 0; i < ExitCanavses.Length; i++)
        {
            ExitCanavses[i].enabled = true;
        }
        GameData.Instance.playerdata.KingFight = true;
        KingHPbar.SetActive(true);
        King.Mystate = BossKing.KingState.Battle_Far;
        PreventRun.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        

    }

}
