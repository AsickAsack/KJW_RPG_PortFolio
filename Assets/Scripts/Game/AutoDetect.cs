using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDetect : MonoBehaviour
{
    public List<GameObject> Enemy;
    public LayerMask DetectLayer;
    public GameObject AssaButton;
    public Transform mychar;

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

    private void OnTriggerStay(Collider other)
    {
        if (DetectLayer == 1 << other.gameObject.layer)
        {
           
            if(Vector3.Distance(Enemy[0].transform.position, mychar.position) < 1.5f)
            {
         
                AssaButton.gameObject.SetActive(true);
            }
            else
            {
           
                if (AssaButton.gameObject.activeSelf)
                {
                    AssaButton.gameObject.SetActive(false);
                }
            }
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
