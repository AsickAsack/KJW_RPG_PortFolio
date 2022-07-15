using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Knight : Player, BattleSystem
{

    public enum State
    {
        Relax,Battle,Ladder,Fly

    }

    [Header("[���̽�ƽ ������]")]

    public JoySticPanel myJoystic;
    Vector3 Dir = Vector3.zero;
    public GameObject MyChar = null;
    Quaternion MyCharRotate = Quaternion.identity;

    [Header("[�ִϸ��̼� ����]")]

    public GameObject RelaxSword;
    public GameObject AttackSword;
    public GameObject LandEffect;
    public Transform RightHand;
    public Transform LeftHand;
    public Transform AttackSpot;
    public Transform Foot;
    public GameObject HitEffect;
    public Animator Uianim;
    public State myState = State.Relax;
    public Rig AnimationRig;
    public LayerMask JumpMask;
    public SkinnedMeshRenderer myRenderer;
    float DamageT;


    Coroutine IsAir;
    Coroutine WeightChange;
    Collider[] myCol;
    Quaternion MyCamRot = Quaternion.identity;
    Vector3 myDirecTion;
    Vector3 ClimbDir;
    bool IsBlock = false;

    #region ���ѻ��±��

    //���°� �ٲ�
    public void ChangeState(State s)
    {
        if (myState == s) return;
        myState = s;

        switch(myState)
        {
            case State.Relax:
                myRigid.useGravity = true;
                myRigid.isKinematic = false;
                myAnim.SetBool("IsBattle", false);
                myAnim.SetBool("IsRelax", true);
                
     
                break;
            case State.Battle:
                myAnim.SetBool("IsBattle", true);
                myAnim.SetBool("IsRelax", false);
                WeightChange = StartCoroutine(SetIK(0.5f));

                break;
            case State.Ladder:
                myRigid.useGravity = false;
                myRigid.isKinematic = true;
                break;
            case State.Fly:
                break;
        }

    }

    //��հ˿� ���̴� IK�� �ڿ������� �ٿ��ش�.
    IEnumerator SetIK(float time)
    {
        yield return new WaitForSeconds(time);
        while(AnimationRig.weight != 1)
        {
            AnimationRig.weight += Time.deltaTime;
            yield return null;
        }    
    }

    public void AirCheck()
    {
        if(myRigid.velocity.y < -2.0f)
        {
           
            if(IsAir == null)
            myAnim.SetBool("IsAir", true);
            IsAir = StartCoroutine(Flying());
        }
    }

    //���¸����� ������Ʈ
    void StateProcess()
    {
       
        switch (myState)
        {
            case State.Relax:
                KnighteRotate();
                AirCheck();
                break;
            case State.Battle:
                AirCheck();
                KnighteRotate();
                break;
            case State.Ladder:
                LadderCheck();
                break;
            case State.Fly:
                break;
        }
    }



    #endregion

    public void LadderCheck()
    {
        Debug.DrawRay(this.transform.position, -MyChar.transform.up);
        if (Physics.Raycast(this.transform.position, -MyChar.transform.up, 0.1f,1<<LayerMask.NameToLayer("Wall")))
        {
            myAnim.SetInteger("LadderIndex", 4);
            ChangeState(State.Relax);


        }
    }

    // ȸ������
    void KnighteRotate()
    {
        if (myJoystic.MoveOn && myJoystic.Dir != Vector3.zero && !myAnim.GetBool("IsAttack") && !myAnim.GetBool("IsPunch") && !myAnim.GetBool("IsChange") && !myAnim.GetBool("Block")&&!myAnim.GetBool("IsLadder"))
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
        //������Ʈ�� �̵����� �ɱ�

        switch (myState)
        {
            case State.Relax:
                if (myJoystic.MoveOn && !myAnim.GetBool("IsAttack") && !myAnim.GetBool("IsPunch")&&!myAnim.GetBool("IsChange")&& !myAnim.GetBool("Block") && !myAnim.GetBool("LadderChange"))
                {
                    myAnim.SetBool("IsRWalk", true);
                    myRigid.MovePosition(this.transform.position + myDirecTion * Time.deltaTime * GameData.Instance.playerdata.MoveSpeed);
                }
                else
                    myAnim.SetBool("IsRWalk", false);


                break;
            case State.Battle:
                if (myJoystic.MoveOn && !myAnim.GetBool("IsAttack") && !myAnim.GetBool("IsPunch") && !myAnim.GetBool("IsChange") && !myAnim.GetBool("Block"))
                {
                    myAnim.SetBool("IsWalk", true);
                    myRigid.MovePosition(this.transform.position + myDirecTion * Time.deltaTime * GameData.Instance.playerdata.MoveSpeed);
                }
                else
                    myAnim.SetBool("IsWalk", false);
                break;

            case State.Ladder:
                {
                    
                    if (myJoystic.MoveOn &&!myAnim.GetBool("LadderChange")) 
                    {
                        if (myJoystic.Dir.z > 0 && myAnim.GetBool("IsLadder"))
                        {
                            myAnim.SetInteger("LadderIndex", 1);
                            myRigid.MovePosition(this.transform.position + Vector3.up * Time.deltaTime * 1.0f);
                        }
                        else if (myJoystic.Dir.z < 0 && myAnim.GetBool("IsLadder"))
                        {
                            myAnim.SetInteger("LadderIndex", 2);
                            myRigid.MovePosition(this.transform.position + Vector3.down * Time.deltaTime * 1.0f);
                        }
                    }
                    else
                    {
                        if(myAnim.GetBool("IsLadder"))
                        { 
                            myAnim.SetInteger("LadderIndex", 0);
                            myJoystic.Dir = Vector3.zero;
                        }
                    }
                    
                }
                break;


        }
    }

    


    #region �ִϸ��̼� �Լ���

    //���� �������� �ִϸ��̼��϶� IK Weight ����
    public void SetWeight(int Value)
    {
        if (WeightChange != null)
        { 
            StopCoroutine(WeightChange);
            WeightChange = StartCoroutine(ChangeIK(Value));
        }
        else
            WeightChange = StartCoroutine(ChangeIK(Value));
    }

    IEnumerator ChangeIK(int Value)
    {
        if(Value.Equals(0))
        { 
            while(AnimationRig.weight != Value)
            {
                AnimationRig.weight = Value;
                yield return null;
            }
        }
        else
        {
            while (AnimationRig.weight != Value)
            {
                AnimationRig.weight += Time.deltaTime *2.0f;
                yield return null;
            }
        }
    }

    //�ִϸ��̼� Ÿ�ֿ̹� ���� ������
    void Jump()
    {
        LandEffect.SetActive(false);
        myRigid.AddForce(this.transform.up * 7.0f, ForceMode.Impulse);
    }

    //������ư
    public void JumpButton()
    { 
        myAnim.SetTrigger("Jump");
        StartCoroutine(Flying());
    }

    //���� �Ŀ� �ٴڿ� �������� ���� ���� Ȯ��
    IEnumerator Flying()
    {
       yield return new WaitForSeconds(0.5f);
        while (true)
        {
            if (Physics.Raycast(Foot.position, -Foot.up, out RaycastHit hit2, 0.5f, JumpMask))
            {
                myAnim.SetBool("IsJump", false);
                myAnim.SetBool("IsAir", false);
                break;

            }
            yield return null;
        }

        IsAir = null; 

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

    //���� �ٲܶ�
    //����� ���߿� �ٸ��� �ֱ�!!
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



    //�����Ҷ� ����Ʈ����
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

    //���������� �ĸ��� 0 , ������ 1 , ����� 2
    public void SwordAttack(int index)
    {
        myAudio.PlayOneShot(SoundManager.Instance.myEffectClip[Random.Range(6, 8)]);
        myCol = Physics.OverlapSphere(AttackSpot.position, 2.0f, 1 << LayerMask.NameToLayer("Monster"));
        if (myCol != null)
        {
            for (int i = 0; i < myCol.Length; i++)
            {
                myCol[i].GetComponent<BattleSystem>()?.OnDamage(index, Random.Range(GameData.Instance.playerdata.ATK - 5, GameData.Instance.playerdata.ATK + 5));
                myAnim.SetBool("Block", false);
            }
        }
        myCol = null;
    }

    //�Ǽ����� ���϶���


    //������ ��ġ �����Ҷ�
    public void RPunchAttack()
    {
        myCol = Physics.OverlapSphere(RightHand.position, 1.0f, 1 << LayerMask.NameToLayer("Monster"));
        if (myCol != null)
        {
            for (int i = 0; i < myCol.Length; i++)
            {
                myCol[i].GetComponent<BattleSystem>()?.OnDamage(0, Random.Range(GameData.Instance.playerdata.ATK-5, GameData.Instance.playerdata.ATK+5)*0.5f);
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
                myCol[i].GetComponent<BattleSystem>()?.OnDamage(index, Random.Range(GameData.Instance.playerdata.ATK - 5, GameData.Instance.playerdata.ATK + 5)*0.5f);
            }
        }
        myCol = null;
    }

    //�¾�����
    public bool OnDamage(int index, float damage)
    {
        Uianim.SetTrigger("HPhit");
        StartCoroutine(HitColor(myRenderer.material));

        //���з� �����ִٸ�
        if (myAnim.GetBool("IsBlock"))
        {
            int rand = Random.Range(1, 11);
            myAudio.PlayOneShot(SoundManager.Instance.myEffectClip[3]);
            switch (index)
            {
                //������ �¾�����
                case 0:
                    myAnim.SetTrigger("BlockHitR");
                    break;

                //���� �¾�����
                case 1:
                    myAnim.SetTrigger("BlockHitL");
                    break;        
            }

            DamageRoutine((int)((damage - GameData.Instance.playerdata.DEF) * 0.5f), 1);

            //���� Ȯ���� ������ �Ͽ� ���Ͻ�Ŵ
            if (rand < 9 && !myAnim.GetBool("IsRelax"))
            {
                return false;
            }
        }
        else
        {
            myAudio.PlayOneShot(SoundManager.Instance.myEffectClip[4]);
            switch (index)
            {
                //������ �¾�����
                case 0:
                    myAnim.SetTrigger("GetHitR");
                    break;

                //���� �¾�����
                case 1:
                    myAnim.SetTrigger("GetHitL");
                    break;
            }

            DamageRoutine((int)(damage - GameData.Instance.playerdata.DEF), 2);
        }
        return true;
    }

    public void DamageRoutine(float Damage, int index)
    {
        DamageT = Damage < 0 ? 0 : Damage;
        GameObject obj = ObjectPool.Instance.ObjectManager[4].Get();
        obj.GetComponent<DamageText>()?.SetTextP(this.transform, DamageT.ToString(), index);
        GameData.Instance.playerdata.CurHP -= DamageT;
        UIManager.Instance.SetHP();
    }

    public void BlockBtn()
    {
      
        IsBlock = !IsBlock;
        myAnim.SetBool("Block", IsBlock);
    }

    IEnumerator HitColor(Material mat)
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        mat.color = Color.white;
    }


}
