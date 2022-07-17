using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class binoculars_Trriger : MonoBehaviour
{
    public GameObject binoculars_Btn;
    Vector3 TargetPos = Vector3.zero;

    private void Awake()
    {
        TargetPos = this.transform.position + new Vector3(0.2f, 0.0f, 0.0f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            binoculars_Btn.transform.position = Camera.main.WorldToScreenPoint(TargetPos);
            binoculars_Btn.SetActive(true);
        }

        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(binoculars_Btn.activeSelf)
            binoculars_Btn.SetActive(false);
        }
    }


}
