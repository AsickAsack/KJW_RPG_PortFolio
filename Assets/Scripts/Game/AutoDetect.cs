using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDetect : MonoBehaviour
{
    public List<GameObject> Enemy;
    public LayerMask DetectLayer;

    private void Awake()
    {
        Enemy = new List<GameObject>();        
    }



    private void OnTriggerEnter(Collider other)
    {
        if (DetectLayer == 1 << other.gameObject.layer)
        {
            Enemy.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (DetectLayer == 1 << other.gameObject.layer)
        {
            Enemy.Remove(other.gameObject);
        }
    }



}
