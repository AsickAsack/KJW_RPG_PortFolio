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

    //������ ������ �ǵ��ƿ� �ڸ��� ��������
    private void Awake()
    {
        OrgPos = transform.localPosition;
        RemitPos[0] = new Vector3(OrgPos.x - 5.0f,OrgPos.y,OrgPos.z);
        RemitPos[1] = new Vector3(OrgPos.x + 5.0f,OrgPos.y, OrgPos.z);
        TargetPos = RemitPos[index++ % 2];

    }


    //�÷��̾ ���� �ȿ� ������ �� ���� ����
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

    //������ �� ���� �����ϰ� ����ġ
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

    //���� �ö󰡸� �÷��̾��� ��ġ�� ������Ű������ �ڽ����� �������
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    //�����϶��� �θ� ����
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

    //������ �θ� ����
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.transform.SetParent(null);
        }
    }


}
