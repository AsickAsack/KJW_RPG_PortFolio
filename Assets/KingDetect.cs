using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingDetect : MonoBehaviour
{
    public List<GameObject> Enemy = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Enemy.Add(other.gameObject);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Enemy.Remove(other.gameObject);

        }
        //µµ¸Á°¡°Ù³Ä´Â ¸Þ¼¼Áö ¶ç¿ì±â
    }

}
