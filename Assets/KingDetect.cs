using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingDetect : MonoBehaviour
{
    public List<GameObject> Enemy = new List<GameObject>();
    public Knight knight;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            knight = other.GetComponent<Knight>();
            Enemy.Add(other.gameObject);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Enemy.Remove(other.gameObject);

        }
    }

}
