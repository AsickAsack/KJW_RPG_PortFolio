using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    float myTime = 0.6f;

    private void Update()
    {
        myTime -= Time.deltaTime;

        if(myTime < 0.0f)
        {
            ObjectPool.Instance.Effects[2].Release(this.gameObject);
        }
    }

    private void OnDisable()
    {
        myTime = 0.6f;
    }
}
