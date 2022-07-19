using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewAngle : MonoBehaviour
{
    public float ViewAngle;
    public float Distance;
    public LayerMask TargetMask;
    private Vector3 LeftBoundary;
    private Vector3 RightBoundary;
    private Collider[] myColl;
    public List<GameObject> Enemy;
    public Transform mychar;

    private void OnTriggerStay(Collider other)
    {
        if(1<<other.gameObject.layer == TargetMask)
        {
            SearchTarget();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (1 << other.gameObject.layer == TargetMask)
        {
            if (Enemy.Contains(other.transform.gameObject))
            {
                Enemy.Remove(other.transform.gameObject);
            }
        }
    }

    private Vector3 CirAngle(float angle)
    {
        angle += mychar.rotation.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Rad2Deg), 0.0f , Mathf.Cos(angle * Mathf.Rad2Deg));
    }


    void SearchTarget()
    {
        LeftBoundary = CirAngle(-ViewAngle * 0.5f);
        RightBoundary = CirAngle(ViewAngle * 0.5f);

        Debug.DrawRay(mychar.position + mychar.up, LeftBoundary, Color.red);
        Debug.DrawRay(mychar.position + mychar.up, RightBoundary, Color.red);

        myColl = Physics.OverlapSphere(mychar.position,Distance,TargetMask);
        for(int i=0; i < myColl.Length; i++)
        {
            Transform Target = myColl[i].transform;
            if(1<<Target.gameObject.layer == TargetMask)
            {
                Vector3 Dir = (Target.transform.position - mychar.position).normalized;
                float angle = Vector3.Angle(Dir, mychar.forward);

                if(angle < ViewAngle * 0.5f)
                {
                    if(Physics.Raycast(mychar.position + mychar.up,Dir,out RaycastHit hit,Distance,TargetMask))
                    {
                        Debug.DrawRay(mychar.position + mychar.up, Dir, Color.blue);

                        if(!Enemy.Contains(hit.transform.gameObject))
                        {
                            Enemy.Add(hit.transform.gameObject);
                        }
                    }
                }

            }

        }




    }
}
