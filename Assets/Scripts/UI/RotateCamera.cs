using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateCamera : MonoBehaviour ,IDragHandler
{
    public Transform Pivot;
    public float RotSpeed = 10.0f;
    public Vector2 LimitRotX = Vector2.zero;
    Vector3 RotPivot = Vector3.zero;

    public void OnDrag(PointerEventData eventData)
    {
        RotPivot.y += eventData.delta.x * Time.deltaTime * RotSpeed;
        RotPivot.x -= eventData.delta.y * Time.deltaTime * RotSpeed;

        RotPivot.x = Mathf.Clamp(RotPivot.x, LimitRotX.x, LimitRotX.y);
        if (RotPivot.x > 180.0f) RotPivot.x -= 360.0f;

        Pivot.rotation = Quaternion.Slerp(Pivot.rotation, Quaternion.Euler(RotPivot), Time.deltaTime * 20.0f);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
