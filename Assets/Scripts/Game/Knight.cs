using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Knight : Player, BattleSystem
{

    public enum State
    {
        Relax,Battle,Climb,Fly

    }



    [Header("[조이스틱 움직임]")]
    public JoySticPanel myJoystic;
    Vector3 Dir = Vector3.zero;
    public GameObject MyChar = null;
    Quaternion MyCharRotate = Quaternion.identity;

    [Header("[애니메이션 관련]")]

    public GameObject RelaxSword;
    public GameObject AttackSword;
    public GameObject LandEffect;
    public Transform RightHand;
    public Transform LeftHand;
    public Transform AttackSpot;
    public Transform Foot;
    public Transform MyRayPoint;
    public Transform MyHand;
    public GameObject HitEffect;
    Collider[] myCol;
    Quaternion MyCamRot = Quaternion.identity;
    Vector3 myDirecTion;
    public State myState = State.Relax;
    public Rig AnimationRig;
    Coroutine WeightChange;
    public LayerMask JumpMask;
    Vector3 ClimbDir;

    #region 유한상태기계

    //상태가 바뀔때
    public void ChangeState(State s)
    {
        if (myState == s) return;
        myState = s;

        switch(myState)
        {
            case State.Relax:
                myAnim.SetBool("IsBattle", false);
                myAnim.SetBool("IsRelax", true);
                
     
                break;
            case State.Battle:
                myAnim.SetBool("IsBattle", true);
                myAnim.SetBool("IsRelax", false);
                WeightChange = StartCoroutine(SetIK(0.5f));

                break;
            case State.Climb:
                break;
            case State.Fly:
                break;
        }

    }

    //양손검에 붙이는 IK를 자연스럽게 붙여준다.
    IEnumerator SetIK(float time)
    {
        yield return new WaitForSeconds(time);
        while(AnimationRig.weight != 1)
        {
            AnimationRig.weight += Time.deltaTime;
            yield return null;
        }    
    }

    //상태마다의 업데이트
    void StateProcess()
    {
       
        switch (myState)
        {
            case State.Relax:
                KnighteRotate();
                break;
            case State.Battle:
                KnighteRotate();
                break;
            case State.Climb:

                break;
            case State.Fly:
                break;
        }
    }



    #endregion
    // 회전로직
    void KnighteRotate()
    {
        if (myJoystic.MoveOn && myJoystic.Dir != Vector3.zero && !myAnim.GetBool("IsAttack") && !myAnim.GetBool("IsPunch") && !myAnim.GetBool("IsChange"))
        {
            MyCamRot = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
            myDirecTion = MyCamRot * new Vector3(myJoystic.Dir.x, 0.0f, myJoystic.Dir.z);
            MyCharRotate = Quaternion.LookRotation((this.transform.position + myDirecTion) - this.transform.position);
            MyChar.transform.rotation = Quaternion.Slerp(MyChar.transform.rotation, MyCharRotate, Time.deltaTime * 15.0f);
        }
    }


    private void Update()
    {
        StateProcess();
    }

    private void FixedUpdate()
    {
        //스테이트로 이동제한 걸기

        switch (myState)
        {
            case State.Relax:
                if (myJoystic.MoveOn && !myAnim.GetBool("IsAttack") && !myAnim.GetBool("IsPunch")&&!myAnim.GetBool("IsChange"))
                {
                    myAnim.SetBool("IsRWalk", true);
                    myRigid.MovePosition(this.transform.position + myDirecTion * Time.deltaTime * 4.0f);
                }
                else
                    myAnim.SetBool("IsRWalk", false);


                break;
            case State.Battle:
                if (myJoystic.MoveOn && !myAnim.GetBool("IsAttack") && !myAnim.GetBool("IsPunch") && !myAnim.GetBool("IsChange"))
                {
                    myAnim.SetBool("IsWalk", true);
                    myRigid.MovePosition(this.transform.position + myDirecTion * Time.deltaTime * 4.0f);
                }
                else
                    myAnim.SetBool("IsWalk", false);
                break;

          

        }
    }



    #region 애니메이션 함수들

    //애니메이션 타이밍에 맞춰 점프함
    void Jump()
    {
        LandEffect.SetActive(false);
        myRigid.AddForce(this.transform.up * 7.0f, ForceMode.Impulse);
    }


    //점프버튼
    public void JumpButton()
    { 
        myAnim.SetTrigger("Jump");
        StartCoroutine(Flying());
    }

    //점프 후에 바닥에 레이저를 쏴서 착지 확인
    IEnumerator Flying()
    {
       yield return new WaitForSeconds(0.5f);
        while (true)
        {
            if (Physics.Raycast(Foot.position, -Foot.up, out RaycastHit hit2, 0.5f, JumpMask))
            {
                myAnim.SetBool("IsJump", false);
                break;

            }
            yield return null;
        }
    }


    public void AttackButton()
    {
        switch (myState)
        {
            case State.Relax:
                
                myAnim.SetTrigger("Punch");
                break;
            case State.Battle:
                
                    myAnim.SetTrigger("Attack");
                break;
        }

    }

    public void WPChange_Btn()
    {
        if (!myAnim.GetBool("IsChange"))
        {
            switch (myState)
            {
                case State.Relax:
                    ChangeState(State.Battle);

                    break;
                case State.Battle:
                    ChangeState(State.Relax);
                    if (WeightChange != null)
                    {
                        StopCoroutine(WeightChange);
                    }
                    AnimationRig.weight = 0;
                    break;
            }
        }

    }


    public void HitCheck()
    {
        
    }

    //어퍼 어택 했을때 루틴
    public void UpperHitCheck()
    {
        //HitEffect.gameObject.SetActive(true);
        myCol = Physics.OverlapSphere(AttackSpot.position, 1.0f,1<<LayerMask.NameToLayer("Monster"));
        if(myCol.Length != 0)
        { 
            for(int i=0;i<myCol.Length;i++)
            {
                if (myCol[i].GetComponent<BattleSystem>().OnDamage(3))
                {
                    myCol[i].GetComponent<Rigidbody>().AddForce(this.transform.up * 12.5f, ForceMode.VelocityChange);
                    Soldier Temp = myCol[i].GetComponent<Soldier>();
                    Temp.myAnim.SetBool("OnAir", true);
                    StartCoroutine(CheckDelay(Temp));                   
                }
                else
                    return;
            }
            myAnim.SetBool("IsHit", true);
            myAnim.SetTrigger("Attack");
        }

        myCol = null;
    }

    IEnumerator CheckDelay(Soldier Temp)
    {
        yield return new WaitForSeconds(1.0f);
        Temp.CheckFloor();
    }

    //어퍼로 올리고 점프할때
    public void JumpAttack()
    {
        myRigid.AddForce(this.transform.up * 10.0f, ForceMode.VelocityChange);
        myAnim.SetBool("IsHit", false);
        Time.timeScale = 0.5f;

    }
    
    // 내려찍을때
    public void JumpAttackCheck()
    {
        //HitEffect.gameObject.SetActive(true);
        myCol = Physics.OverlapSphere(AttackSpot.position, 1.0f, 1 << LayerMask.NameToLayer("Monster"));
        if (myCol != null)
        {
            for (int i = 0; i < myCol.Length; i++)
            {
                myCol[i].GetComponent<Rigidbody>().AddForce((myDirecTion + (-this.transform.up)).normalized * 10.0f, ForceMode.VelocityChange);
            }
        }
        myCol = null;
        //HitEffect.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }
    

    public void FootSound()
    {

    }

    //착지할때 이펙트켜줌
    public void Landing()
    {
        LandEffect.SetActive(true);
    }


    public void ChangeWeapon()
    {
        switch (myState)
        {
            case State.Relax:
                RelaxSword.SetActive(true);
                AttackSword.SetActive(false);
                break;
            case State.Battle:
                RelaxSword.SetActive(false);
                AttackSword.SetActive(true);
                break;
        }
    }


    #endregion

    //오른쪽으로 후리면 0 , 왼쪽은 1 , 가운데는 2
    public void SwordAttack(int index)
    {
        myCol = Physics.OverlapSphere(AttackSpot.position, 1.0f, 1 << LayerMask.NameToLayer("Monster"));
        if (myCol != null)
        {
            for (int i = 0; i < myCol.Length; i++)
            {
                myCol[i].GetComponent<BattleSystem>()?.OnDamage(index);
            }
        }
        myCol = null;
    }

    //오른쪽 펀치 공격할때
    public void RPunchAttack()
    {
        myCol = Physics.OverlapSphere(RightHand.position, 1.0f, 1 << LayerMask.NameToLayer("Monster"));
        if (myCol != null)
        {
            for (int i = 0; i < myCol.Length; i++)
            {
                myCol[i].GetComponent<BattleSystem>()?.OnDamage(0);
            }
        }
        myCol = null;
    }

    public void LPunchAttack(int index)
    {
        myCol = Physics.OverlapSphere(LeftHand.position, 1.0f, 1 << LayerMask.NameToLayer("Monster"));
        if (myCol != null)
        {
            for (int i = 0; i < myCol.Length; i++)
            {
                myCol[i].GetComponent<BattleSystem>()?.OnDamage(index);
            }
        }
        myCol = null;
    }

    //맞았을때
    public bool OnDamage(int index)
    {
        myAnim.SetTrigger("GetHit");
        return true;
    }
}
