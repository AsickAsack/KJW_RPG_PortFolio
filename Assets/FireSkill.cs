using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkill : MonoBehaviour
{
    public BossKing boss;
    public Collider[] myCol;

    void RealeseThis()
    {
        ObjectPool.Instance.Effects[3].Release(this.gameObject);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.GetComponent<BattleSystem>()?.DamageSound(1);
            other.GetComponent<BattleSystem>()?.OnDamage(1, Random.Range(boss.stat.ATK - 5.0f, boss.stat.ATK + 5.0f), boss.transform);
            
            Invoke("RealeseThis", 2.0f);
        }
        else
        {
            myCol = Physics.OverlapSphere(this.transform.position, 4.0f, 1 << LayerMask.NameToLayer("Player"));
            for(int i = 0; i < myCol.Length; i++)
            {
                myCol[i].GetComponent<BattleSystem>()?.DamageSound(1);
                myCol[i].GetComponent<BattleSystem>()?.OnDamage(1, Random.Range(boss.stat.ATK - 5.0f, boss.stat.ATK + 5.0f), boss.transform);
            }
            Invoke("RealeseThis", 2.0f);
        }


    }

}
