using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRock : MonoBehaviour
{

    public Vector3 OrgPos;
    public int index = 0;
    Vector3[] RemitPos = new Vector3[2];
    Vector3 TargetPos;
    Coroutine MoveCo;
    float speed = 2.0f;

    //움직일 범위와 되돌아올 자리를 선언해줌
    private void Awake()
    {
        OrgPos = transform.localPosition;
        RemitPos[0] = new Vector3(OrgPos.x - 5.0f,OrgPos.y,OrgPos.z);
        RemitPos[1] = new Vector3(OrgPos.x + 5.0f,OrgPos.y, OrgPos.z);
        TargetPos = RemitPos[index++ % 2];

    }


    //플레이어가 범위 안에 들어오면 돌 무브 시작
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (MoveCo == null)
                MoveCo = StartCoroutine(MoveStart());
        }
    }


    IEnumerator MoveStart()
    {
        while (true)
        {

            if (Vector3.Distance(this.transform.localPosition, TargetPos) < 0.5f)
            {
                TargetPos= RemitPos[index++ % 2];
            }

            this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, TargetPos, Time.deltaTime * speed);

            yield return null;
        }
       
    }

    //나가면 돌 무빙 종료하고 원위치
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (MoveCo != null)
            {
                StopCoroutine(MoveCo);
                MoveCo = null;
            }
            this.transform.localPosition = OrgPos;
        }
    }

    //돌에 올라가면 플레이어의 위치를 고정시키기위해 자식으로 만들어줌
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    //움직일때는 부모를 해제
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(collision.transform.GetComponent<Knight>().myJoystic.MoveOn)
            {
                collision.transform.SetParent(null);
            }
            else
            {
                collision.transform.SetParent(this.transform);
            }
        }
    }

    //나가면 부모를 해제
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.transform.SetParent(null);
        }
    }


}
