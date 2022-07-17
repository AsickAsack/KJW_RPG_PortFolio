using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomInBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Transform myCam;
    bool IsClick = false;
    float posZ;
    float speed = 20.0f;

    private void Update()
    {
        if(IsClick)
        {
            posZ = myCam.transform.localPosition.z;
            posZ += Time.deltaTime * speed;

            posZ = Mathf.Clamp(posZ, 3.0f, 30.0f);


            myCam.transform.localPosition = Vector3.Lerp(myCam.transform.localPosition, new Vector3(0, 0, posZ),Time.deltaTime * 15.0f);


        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsClick = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsClick = false;
    }
}
