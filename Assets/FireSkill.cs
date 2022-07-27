using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkill : MonoBehaviour
{
    public float Damage;
    public Transform King;


    void RealeseThis()
    {
        ObjectPool.Instance.Effects[3].Release(this.gameObject);
    }

    


    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.gameObject);

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("플레이어에맞음");
            other.GetComponent<BattleSystem>().OnDamage(1, Damage, King);
            Invoke("RealeseThis", 2.0f);
        }
        else
            Invoke("RealeseThis", 2.0f);
    }

}
