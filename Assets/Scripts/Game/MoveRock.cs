using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRock : MonoBehaviour
{
    public Vector2 RimitPos;
    public Vector3 OrgPos;
    Coroutine MoveCo;
    float speed = 2.0f;

    int rand = 0;

    private void Awake()
    {
        OrgPos = transform.localPosition;
    }



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

        rand = Random.Range(0, 2);


        if (rand == 0)
        {
            while (true)
            {

                while (this.transform.localPosition.x > RimitPos.x)
                {
                    this.transform.localPosition += -Vector3.right * Time.deltaTime * speed;
                    yield return null;
                }

                while (this.transform.localPosition.x < RimitPos.y)
                {
                    this.transform.localPosition += Vector3.right * Time.deltaTime * speed;
                    yield return null;
                }

            }
        }
        else
        { 
            while (true)
            {
                while (this.transform.localPosition.x < RimitPos.y)
                {
                    this.transform.localPosition += Vector3.right * Time.deltaTime * speed;
                    yield return null;
                }

                while (this.transform.localPosition.x > RimitPos.x)
                {
                    this.transform.localPosition += -Vector3.right * Time.deltaTime * speed;
                    yield return null;
                }

            }
        }
    }




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
}
