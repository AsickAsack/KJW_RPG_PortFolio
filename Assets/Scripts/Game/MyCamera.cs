using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    float ZoomDist;
    public GameObject myCam;
    public LayerMask CameraMask;
    float Virtualradius = 1.0f;
    Ray ray = new Ray();

    private void Awake()
    {
        ZoomDist = -this.transform.localPosition.z;
    }

    //카메라가 벽에 걸리면 카메라 위치를 벽 앞으로 바꿔줌
    void Update()
    {
        ZoomDist = Mathf.Clamp(ZoomDist, 5, 5);
        myCam.transform.localPosition = new Vector3(0.0f, 0.0f, -ZoomDist);

        ray.origin = this.transform.position;
        ray.direction = -this.transform.forward;

        if (Physics.Raycast(ray, out RaycastHit hit, ZoomDist+ Virtualradius, CameraMask))
        {
            myCam.transform.position = hit.point - ray.direction * Virtualradius;

        }
    }
}
