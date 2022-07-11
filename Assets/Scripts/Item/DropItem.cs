using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public ItemData itemData;
    public Rigidbody myRigid;


    private void Start()
    {
        myRigid.AddForce(Vector3.up * Time.deltaTime * 5.0f, ForceMode.Impulse);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            ObjectPool.Instance.ObjectManager[0].Release(this.gameObject);
        }

       
    }



}
