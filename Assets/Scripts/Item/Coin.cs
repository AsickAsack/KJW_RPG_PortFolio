using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    public int gold = 0;
    public Rigidbody myRigid;

    private void Start()
    {
        myRigid.AddForce(Vector3.up * 400.0f + -Vector3.right * 400.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            GameData.Instance.playerdata.money += gold;
            ObjectPool.Instance.ObjectManager[5].Release(this.gameObject);

        }

    }
}
