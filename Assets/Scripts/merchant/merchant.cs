using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class merchant : MonoBehaviour
{
    public GameObject DBtn;
    public GameObject SBtn;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            DBtn.SetActive(true);
            SBtn.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            DBtn.SetActive(false);
            SBtn.SetActive(false);
        }
    }
}
