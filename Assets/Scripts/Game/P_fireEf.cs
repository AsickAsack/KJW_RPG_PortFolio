using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_fireEf : MonoBehaviour
{
    float myTime = 1.0f;

    private void Update()
    {
        myTime -= Time.deltaTime;

        if (myTime < 0.0f)
        {
            ObjectPool.Instance.Effects[5].Release(this.gameObject);
        }
    }

    private void OnDisable()
    {
        myTime = 1.0f;
    }
}
