using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Soldier : Player , BattleSystem
{
    public AutoDetect Detect;
    public GameObject Sword;
    public GameObject BackSword;
    public GameObject LandEffect;
    public LayerMask LandLayer;
    protected Vector3 Dir = Vector3.zero;
    protected bool Move = false;
    protected Collider[] myCol;
    protected Coroutine AttackDelay;
    protected Coroutine StunCo;
    public float HitTime = 0.0f;
    public Button Killbtn;


    public abstract void ChangeState(S_State s);

    //몬스터의 상태
    public enum S_State
    {
        Patrol, Battle, Death ,OnAir,Stun
    }

    //패트롤로 시작
    public S_State myState = S_State.Patrol;

    private void Update()
    {
        StateProcess();
    }



    protected IEnumerator GoBattle(float time)
    {
        yield return new WaitForSeconds(time);
        Move = true;
        myAnim.SetBool("IsWalk", true);
    }

    //아이들 상태로 갈때
    public void HideSword()
    {
        Sword.SetActive(false);
        BackSword.SetActive(true);

    }

    //배틀 상태로 
    public void GetSword()
    {
        Sword.SetActive(true);
        BackSword.SetActive(false);
       
    }



    void StateProcess()
    {
        switch (myState)
        {
            case S_State.Patrol:
                if(Detect.Enemy.Count > 0)
                {
                    ChangeState(S_State.Battle);
                }
     
                break;
            case S_State.Battle:
                if (Detect.Enemy.Count.Equals(0))
                {
                    ChangeState(S_State.Patrol);
                }

                if(myAnim.GetBool("IsHit"))
                {
                    HitTime = 0.5f;
                    if (AttackDelay != null)
                    { 
                        StopCoroutine(AttackDelay);
                        AttackDelay = null;
                    }
                }
                else
                {
                    if(HitTime > 0.0f)
                    HitTime -=Time.deltaTime;
                }

                if(HitTime < 0.1f)
                BattleState();

                break;
            case S_State.OnAir:
                break;

            case S_State.Stun:

                break;



            case S_State.Death:
                break;
        }
    }

    protected IEnumerator Stun(float time)
    {
        Killbtn.gameObject.SetActive(true);
        Killbtn.transform.position = Camera.main.WorldToScreenPoint(this.transform.position + new Vector3(0, 0.1f, 0));
        
        myAnim.SetTrigger("Stun");
        myAnim.SetBool("IsStun", true);

        yield return new WaitForSeconds(time);
       

        if (myState != S_State.Death)
        {
            myAnim.SetBool("IsStun", false);

            if (Detect.Enemy.Count > 0)
            {
                ChangeState(S_State.Battle);
            }
            else
            {
                ChangeState(S_State.Patrol);
            }
        }

         Killbtn.gameObject.SetActive(false);
    }


    public void BattleState()
    {
        
        //거리가 2.0f보다 크다면
        if (Vector3.Distance(this.transform.position, Detect.Enemy[0].transform.position) > 2.0f)
        {
            Dir = Detect.Enemy[0].transform.position - this.transform.position;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(Dir), Time.deltaTime * 20.0f);
            Move = true;
            myAnim.SetBool("IsWalk", true);
            
        }
        ////거리가 2.0f보다 작다면
        else
        {
            Move = false;
            myAnim.SetBool("IsWalk", false);
            if (AttackDelay == null)
            {
                AttackDelay = StartCoroutine(AtkDelay(3.0f));
            }
        
        }
    }

    IEnumerator AtkDelay(float time)
    {
        myAnim.SetTrigger("Attack");

        yield return new WaitForSeconds(time);

        AttackDelay = null;
    }


    private void FixedUpdate()
    { 
        if(Move && HitTime < 0.1f && myState != S_State.Stun)
        myRigid.MovePosition(this.transform.position +Dir.normalized * Time.deltaTime * 3.0f);
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

    

    public void OnAttack(int index)
    {
        myCol = Physics.OverlapSphere(Sword.transform.position, 1.5f, 1 << LayerMask.NameToLayer("Player"));
        if (myCol != null)
        {
            for (int i = 0; i < myCol.Length; i++)
            {
                //널체크
                if(myCol[i].GetComponent<BattleSystem>() != null)
               if(!myCol[i].GetComponent<BattleSystem>().OnDamage(index))
                {
                    ChangeState(S_State.Stun);
                    
                }
            }
        }
        myCol = null;
    }


    //20퍼 확률로 쉴드 
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
            case 4:
                myAnim.SetTrigger("OneShot");
                break;
        }

        return true;
    }
}
