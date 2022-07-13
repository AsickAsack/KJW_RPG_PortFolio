using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSwordMan : Soldier
{



    //�׾�����
    public override void Death()
    {
        myAnim.SetTrigger("Death");
        SoundManager.Instance.DeleteEffectSource(this.GetComponent<AudioSource>());
        myRigid.isKinematic = true;
        dropRand(Random.Range(5, 8), 50, 100);
        GameData.Instance.SetNotify(myData.EXP + "�� ����ġ�� ȹ���߽��ϴ�.");
        GameData.Instance.playerdata.CurEXP += myData.EXP;
        UIManager.Instance.SetExp();

    }
}
