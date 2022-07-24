using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpDoor : MonoBehaviour
{
    float DestroyTime = 0.0f;

    // Update is called once per frame
    void Update()
    {
        DestroyTime += Time.deltaTime;
        if (DestroyTime > 60.0f)
        {
            ObjectPool.Instance.WarpDoor[0].Release(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.transform.position = UIManager.Instance.OrgPos;
            ObjectPool.Instance.WarpDoor[0].Release(this.gameObject);
        }
    }
}
