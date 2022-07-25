using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float Damage;
    public Transform king;
    float MoveTime = 1.0f;
    float Speed = 15.0f;

    public void Shot(Transform tr,Transform Enemy,float KDamage)
    {
        king = tr;
        Damage = KDamage;

        StartCoroutine(GoShot(king, Enemy, Damage));


    }

    IEnumerator GoShot(Transform tr, Transform Enemy, float Damage)
    {
        while(MoveTime > 0.0f)
        {
            MoveTime -= Time.deltaTime;

            this.transform.Translate((Enemy.position - tr.position).normalized * Time.deltaTime * Speed,Space.World);
            yield return null;
        }
        ObjectPool.Instance.Effects[1].Release(this.gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.GetComponent<BattleSystem>()?.OnDamage(1, Damage, king);
            ObjectPool.Instance.Effects[1].Release(this.gameObject);
        }

    }

    private void OnDisable()
    {
        MoveTime = 1.0f;
    }
}
