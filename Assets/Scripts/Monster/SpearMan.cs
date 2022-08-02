using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearMan : Soldier
{



    //�׾�����
    public override void Death()
    {
        if(!IsAssaDeath)
        myAnim.SetTrigger("Death");
        SoundManager.Instance.DeleteEffectSource(this.GetComponent<AudioSource>());
        myRigid.isKinematic = true;
        dropRand(Random.Range(5,8),150, 200);
        GameData.Instance.SetNotify(myData.EXP + "�� ����ġ�� ȹ���߽��ϴ�.");
        GameData.Instance.playerdata.CurEXP += myData.EXP;
        UIManager.Instance.SetExp();
        MonsterSpawnManager.Instance.ReservationSpawn(1,patrolIndex);
        StartCoroutine(DeathAfter(2.0f,1));
    }






}