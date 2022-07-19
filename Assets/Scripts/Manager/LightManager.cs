using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.right * Time.deltaTime*20);
        Debug.Log(this.transform.rotation.eulerAngles.x);
    }
}
