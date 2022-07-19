using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hanglider : MonoBehaviour
{
    public GameObject HangBtn;
    Vector3 TargetPos = Vector3.zero;

    private void Awake()
    {
        TargetPos = this.transform.position + new Vector3(0.2f, 0.0f, 0.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            HangBtn.transform.position = Camera.main.WorldToScreenPoint(TargetPos);
            HangBtn.SetActive(true);
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (HangBtn.activeSelf)
                HangBtn.SetActive(false);
        }
    }


}
