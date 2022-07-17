using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BinoCulars : MonoBehaviour, IDragHandler
{
    public GameObject MyCam;
    public Transform Pivot;
    public float RotSpeed = 10.0f;
    public Vector2 LimitRotX = Vector2.zero;
    public Vector2 LimitRotY = Vector2.zero;
    Vector3 RotPivot = Vector3.zero;
    Vector3[] OrgPos = new Vector3[2];
    public Canvas ViewCanvas;

    public GameObject Canvases;

    private void Awake()
    {
        OrgPos[0] = MyCam.transform.localPosition;
        OrgPos[1] = Pivot.position;
    }

    public void OnDrag(PointerEventData eventData)
    {


        RotPivot.y += eventData.delta.x * Time.deltaTime * RotSpeed;
        RotPivot.x -= eventData.delta.y * Time.deltaTime * RotSpeed;


         RotPivot.x = Mathf.Clamp(RotPivot.x, LimitRotX.x, LimitRotX.y);
         RotPivot.y = Mathf.Clamp(RotPivot.y, LimitRotY.x, LimitRotY.y);

        if (RotPivot.x > 180.0f) RotPivot.x -= 360.0f;

        Pivot.rotation = Quaternion.Slerp(Pivot.rotation, Quaternion.Euler(RotPivot), Time.deltaTime * 15.0f);
        
    }

    public void ExitView()
    {
        ViewCanvas.enabled = false;
        Canvases.SetActive(true);
        MyCam.transform.localPosition = OrgPos[0];
        MyCam.gameObject.SetActive(false);
        Pivot.transform.position = OrgPos[1];
        
    }

    public void StartView()
    {
        Canvases.SetActive(false);
        MyCam.gameObject.SetActive(true);
        ViewCanvas.gameObject.SetActive(true);
        ViewCanvas.enabled = true;
    }



}