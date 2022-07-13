using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongMan : Soldier
{



    //죽었을때
    public override void Death()
    {
        myAnim.SetTrigger("Death");
        SoundManager.Instance.DeleteEffectSource(this.GetComponent<AudioSource>());
        myRigid.isKinematic = true;
        dropRand(Random.Range(5, 8), 200, 300);
        GameData.Instance.SetNotify(myData.EXP + "의 경험치를 획득했습니다.");
        GameData.Instance.playerdata.CurEXP += myData.EXP;
        UIManager.Instance.SetExp();

    }
}