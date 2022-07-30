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
    float OrgVolume;
    

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            SoundManager.Instance.SetBGM(SoundManager.Instance.myBgmClip[1],true);
            OrgVolume = SoundManager.Instance.EffectVolume;
            SoundManager.Instance.EffectVolume = 0.0f;
            other.GetComponent<Knight>().myJoystic.MoveOn = false;
            for(int i = 0; i < ExitCanavses.Length; i++)
            {
                ExitCanavses[i].enabled = false;
            }

            Director.Play(); 


        }
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
        this.GetComponent<BoxCollider>().enabled = false;

    }

}
