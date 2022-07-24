using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeRecovery : MonoBehaviour
{
    public GameObject HomeRecoveryBtn;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            HomeRecoveryBtn.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
            HomeRecoveryBtn.SetActive(true);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            HomeRecoveryBtn.SetActive(false);
        }
    }


}
