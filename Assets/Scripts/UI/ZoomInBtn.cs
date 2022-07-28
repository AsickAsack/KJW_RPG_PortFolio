using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomInBtn : MonoBehaviour,IPointerClickHandler
{
    public Transform myCam;
    float posZ;
    float speed = 20.0f;
    Coroutine ZoomCo;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(ZoomCo == null)
        ZoomCo = StartCoroutine(ZoomIn());
        else
        {
            StopCoroutine(ZoomCo);
            ZoomCo = StartCoroutine(ZoomIn());
        }
    }

    IEnumerator ZoomIn()
    {
        SoundManager.Instance.PlayEffect1Shot(23);
        float Zoomtime = 0.0f;
        while (Zoomtime < 1.0f)
        {
            Zoomtime += Time.deltaTime;

            posZ = myCam.transform.localPosition.z;
            posZ += Time.deltaTime * speed;

            posZ = Mathf.Clamp(posZ, 3.0f, 30.0f);


            myCam.transform.localPosition = Vector3.Lerp(myCam.transform.localPosition, new Vector3(0, 0, posZ), Time.deltaTime * 15.0f);

            yield return null;
        }
        
    }
}
