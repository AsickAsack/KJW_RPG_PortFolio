using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoySticPanel : MonoBehaviour ,IDragHandler,IEndDragHandler , IBeginDragHandler
{
    public GameObject JoystickBack;
    public RectTransform stick;
    public bool MoveOn = false;
    public Vector3 Dir = Vector3.zero;
    Vector3 StartDir = Vector3.zero;
    Vector3 MovePos = Vector3.zero;


    //�巡�׽� ���̽�ƽ Ű��
    public void OnBeginDrag(PointerEventData eventData)
    {
        MoveOn = true;
        JoystickBack.gameObject.SetActive(true);
        JoystickBack.transform.position = eventData.position;
        StartDir = stick.transform.position;
        MoveOn = true;
    }


    // �巡�� �Ҷ� ���̽�ƽ ũ��ȿ����� �����̰� �������ָ� ������
    public void OnDrag(PointerEventData eventData)
    {

        MovePos = eventData.position;
        MovePos.x = Mathf.Clamp(MovePos.x, StartDir.x-30, StartDir.x+30);
        MovePos.y = Mathf.Clamp(MovePos.y, StartDir.y-30, StartDir.y+30);

        stick.transform.position = Vector3.Lerp(stick.transform.position, MovePos, Time.deltaTime * 25.0f);

        Dir.x = (MovePos - StartDir).normalized.x;
        Dir.z = (MovePos - StartDir).normalized.y;
    }

    //�巡�װ� ������
    public void OnEndDrag(PointerEventData eventData)
    {
        JoystickBack.gameObject.SetActive(false);
        stick.anchoredPosition = Vector2.zero;
        MoveOn = false;
    }

}
