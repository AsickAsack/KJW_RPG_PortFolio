using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark : MonoBehaviour
{
    public BossKing boss;
    GameObject SparkAir;
    Collider[] myCol;

    
    public void SetAirSpark()
    {
        StartCoroutine(SpawnSpark());
    }


    IEnumerator SpawnSpark()
    {
        yield return new WaitForSeconds(1.0f);

        SparkAir = ObjectPool.Instance.Effects[7].Get();
        SparkAir.transform.position = this.transform.position + new Vector3(0.0f, 0.5f, 0.0f);

        yield return new WaitForSeconds(0.3f);
        myCol = Physics.OverlapSphere(SparkAir.transform.position, 1.5f, 1 << LayerMask.NameToLayer("Player"));
        for(int i = 0; i < myCol.Length; i++)
        {
            myCol[i].GetComponent<BattleSystem>()?.DamageSound(1);
            myCol[i].GetComponent<BattleSystem>()?.OnDamage(1, Random.Range(boss.stat.ATK - 5.0f, boss.stat.ATK + 5.0f)*0.5f, boss.transform);
        }

        yield return new WaitForSeconds(3.0f);
        ObjectPool.Instance.Effects[7].Release(SparkAir);
        ObjectPool.Instance.Effects[6].Release(this.gameObject);
    }
}
