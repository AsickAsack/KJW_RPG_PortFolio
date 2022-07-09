using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Player , BattleSystem
{
    public AutoDetect Detect;
    public GameObject Sword;
    public GameObject BackSword;
    public GameObject LandEffect;
    public LayerMask LandLayer;
    Vector3 Dir = Vector3.zero;
    bool Move = false;
    Collider[] myCol;

    public enum S_State
    {
        Patrol, Battle, Death ,OnAir
    }

    public S_State myState = S_State.Patrol;

    private void Update()
    {
        StateProcess();
    }

    public void ChangeState(S_State s)
    {
        if (myState.Equals(s)) return;

        myState = s;

        switch (myState)
        {
            case S_State.Patrol:
                myAnim.SetTrigger("GoIdle");
                break;

            case S_State.Battle:
                myAnim.SetTrigger("GoBattle");
                StartCoroutine(DelayBattle());
                break;
            case S_State.OnAir:
                Time.timeScale = 0.5f;
                break;
            case S_State.Death:
                break;

        }

    }

    IEnumerator DelayBattle()
    {
        yield return new WaitForSeconds(1.0f);
        myAnim.SetBool("IsWalk", true);
    }

    void StateProcess()
    {
        switch (myState)
        {
            case S_State.Patrol:
                if (Detect.Enemy.Count != 0)
                    ChangeState(S_State.Battle);
                break;

            case S_State.Battle:
                if(myAnim.GetBool("IsWalk"))
                FollowPlayer();
                break;
            case S_State.OnAir:
       


                break;
            case S_State.Death:
                break;
        }
    }

    private void FixedUpdate()
    {   if (Move && !myAnim.GetBool("IsAttack"))
            myRigid.MovePosition(this.transform.position +Dir.normalized * Time.deltaTime * 3.0f);
    }

    void FollowPlayer()
    {
        if (Detect.Enemy.Count == 0)
            ChangeState(S_State.Patrol);
        else
        {


            if (Vector3.Distance(this.transform.position, Detect.Enemy[0].transform.position) < 2.0f)
            {
                Move = false;
                myAnim.SetTrigger("Attack");
            }
            else
            { 
                if (!myAnim.GetBool("IsAttack"))
                { 
                    Dir = Detect.Enemy[0].transform.position - this.transform.position;
                    Move = true;
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(Dir), Time.deltaTime * 10.0f);
                }
            }
        }
    }

    public void CheckFloor()
    {
        StartCoroutine(CheckF());
    }

    IEnumerator CheckF()
    {
        while(true)
        {
            if (Physics.Raycast(this.transform.position, -this.transform.up, 0.2f, LandLayer))
            {
                LandEffect.SetActive(true);
                myAnim.SetBool("OnAir", false);
                break;
                
            }

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        LandEffect.SetActive(false);
    }

    public void offLandEffect()
    {
        LandEffect.SetActive(false);
    }

    public void HideSword()
    {
        Sword.SetActive(false);
        BackSword.SetActive(true);
        
    }

    public void GetSword()
    {
        Sword.SetActive(true);
        BackSword.SetActive(false);
    }

    public void OnAttack()
    {
        myCol = Physics.OverlapSphere(Sword.transform.position, 1.0f, 1 << LayerMask.NameToLayer("Player"));
        if (myCol != null)
        {
            for (int i = 0; i < myCol.Length; i++)
            {
                myCol[i].GetComponent<BattleSystem>()?.OnDamage(0);
            }
        }
        myCol = null;
    }

    //20ÆÛ È®·ü·Î ½¯µå 
    public bool OnDamage(int index)
    {
        int rand = Random.Range(1, 11);
        if(rand < 3)
        {
            myAnim.SetTrigger("GetHitS");
            return false;
        }

        switch(index)
        {
            case 0:
                myAnim.SetTrigger("GetHitL");
                break;
                
            case 1:
                myAnim.SetTrigger("GetHitR");
                break;
            case 2:
                myAnim.SetTrigger("GetHitM");
                break;
            case 3:
                myAnim.SetTrigger("Upper");
                break;
        }

        return true;
    }
}
