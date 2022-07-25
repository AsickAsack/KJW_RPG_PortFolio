using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkill : MonoBehaviour
{
    public float Damage;
    public Transform King;
    public GameObject Parent;

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("¸ÂÀ½");
        if (other.layer == LayerMask.NameToLayer("Player"))
        {
            other.GetComponent<BattleSystem>()?.OnDamage(1, Damage, King);
            Invoke("RealeseThis", 0.5f);
        }
        else
            Invoke("RealeseThis", 0.5f);

    }

    void RealeseThis()
    {
        ObjectPool.Instance.Effects[3].Release(Parent);
    }
}
