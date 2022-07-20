using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDetect : MonoBehaviour
{
    public List<GameObject> Enemy;
    public LayerMask DetectLayer;
    public GameObject AssaButton;
    public Transform mychar;
    public GameObject NearEnemy;
    public Soldier NESolider;

    private void Awake()
    {
        Enemy = new List<GameObject>();        
    }




    private void OnTriggerEnter(Collider other)
    {
        if (DetectLayer == 1 << other.gameObject.layer)
        {
            Enemy.Add(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (DetectLayer == 1 << other.gameObject.layer)
        {

            if(other.gameObject)


            if(Vector3.Distance(other.transform.position, mychar.position) < 1.5f)
            {
                NearEnemy = other.gameObject;
                
            }
            
            if(NearEnemy !=null)
            {
                if (NESolider == null)
                    NESolider = NearEnemy.GetComponent<Soldier>();

                if (NESolider.myState == Soldier.S_State.Patrol)
                {
                    AssaButton.transform.position = Camera.main.WorldToScreenPoint(NearEnemy.transform.position + new Vector3(1.5f, 1.0f, 0.0f));
                    AssaButton.gameObject.SetActive(true);
                }
                if (NESolider.myState == Soldier.S_State.Assasination || NESolider.myState == Soldier.S_State.Battle)
                    AssaButton.gameObject.SetActive(false);
            }



        }

    }


    private void OnTriggerExit(Collider other)
    {
        if (DetectLayer == 1 << other.gameObject.layer)
        {
            Enemy.Remove(other.gameObject);
        }
    }



}
