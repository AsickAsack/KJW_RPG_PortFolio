using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Cinemachine;

public class Knight : Player, BattleSystem
{
    
    public enum State
    {
        Relax,Battle,Ladder,Fly, Assasination
    }

    [Header("[조이스틱 움직임]")]

    public JoySticPanel myJoystic;
    public GameObject MyChar = null;
    Quaternion MyCharRotate = Quaternion.identity;

    [Header("[애니메이션 관련]")]

    private GameObject RelaxSword;
    private GameObject AttackSword;
    private Transform AttackSpot;
    public GameObject[] Swords;
    public GameObject[] BackSwords;
    public GameObject LandEffect;
    public Transform RightHand;
    public Transform LeftHand;
    public Transform Foot;
    public Animator Uianim;
    public State myState = State.Relax;
    public Rig AnimationRig;
    public LayerMask JumpMask;
    public SkinnedMeshRenderer myRenderer;
    public AutoDetect Detect;
    public CapsuleCollider CapColl;
    float DamageT;
    public ParticleSystem[] Potion;

    [Header("[행글라이더]")]
    public GameObject Hanglider;


    Coroutine AssamoveCo;
    Coroutine WeightChange;
    Collider[] myCol;
    Quaternion MyCamRot = Quaternion.identity;
    Vector3 myDirecTion;
    bool IsBlock = false;
    bool ladderMove = false;
    public bool SilentMode = true;
    public GameObject Effect;
    public GameObject SwordTrail;
    public CinemachineVirtualCamera mycamera;
    NoiseSettings mynoisedef;
    public GameObject[] SkillEffect;
    Coroutine DefenceSkill = null;

    private void Awake()
    {
        CheckWeapon();
        mynoisedef = Resources.Load("First_Noise") as NoiseSettings;
        UIManager.Instance.GetButtonFunc(JumpButton, BlockBtn, WPChange_Btn, AttackButton);
        UIManager.Instance.PickUp += Pickup;
        UIManager.Instance.PotionConsume += PotionConsume;
    }

    #region 유한상태기계

    //상태가 바뀔때
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
  
                break;
            case State.Ladder:
                myRigid.useGravity = false;
                myRigid.isKinematic = true;
                break;
            case State.Fly:
                DragSet(10);
                break;
            case State.Assasination:
                break;

                
        }

    }

    //상태마다의 업데이트
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
                KnighteRotate();
                HangCheck();
                    break;
            case State.Assasination:
                break;
        }
    }


    #endregion

    #region 행글라이더 관련

    public void HangCheck()
    {

        if (Physics.Raycast(this.transform.position, -MyChar.transform.up, 0.1f))
        {
            myAnim.SetBool("IsHang", false);
            Hanglider.SetActive(false);
            myRigid.drag = 0;
            ChangeState(State.Relax);

        }
    }

    public void HangBtn()
    {
        //버튼 끄기
        
        StartCoroutine(ChangeHangMonde());

        
    }

    IEnumerator ChangeHangMonde()
    {
        //화면끄기

        if (myState == State.Battle)
        {
            ChangeState(State.Relax);
            yield return new WaitForSeconds(1.0f);
        }

        
        ChangeState(State.Fly);
        Hanglider.SetActive(true);
        this.transform.position = this.transform.position + new Vector3(0, 0, 5.0f);
        myAnim.SetTrigger("GoHang");

        yield return null;

    }



    public void DragSet(int index)
    {
        myRigid.drag = index;
    }


        #endregion


        #region 업데이트, 이동관련

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
                if (myJoystic.MoveOn && !myAnim.GetBool("IsAttack") && !myAnim.GetBool("IsPunch")&&!myAnim.GetBool("IsChange")&& !myAnim.GetBool("Block") && !myAnim.GetBool("IsLadder") && !myAnim.GetBool("IsCharge"))
                {
                    myAnim.SetBool("IsRWalk", true);
                    myRigid.MovePosition(this.transform.position + myDirecTion * Time.deltaTime * GameData.Instance.playerdata.MoveSpeed);
                }
                else
                    myAnim.SetBool("IsRWalk", false);


                break;
            case State.Battle:
                if (myJoystic.MoveOn && !myAnim.GetBool("IsAttack") && !myAnim.GetBool("IsPunch") && !myAnim.GetBool("IsChange") && !myAnim.GetBool("Block") && !myAnim.GetBool("IsLadder") && !myAnim.GetBool("IsCharge"))
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
                            if (ladderMove)
                                myRigid.MovePosition(this.transform.position + Vector3.up * Time.deltaTime * 1.0f);
                        }
                        else if (myJoystic.Dir.z < 0 && myAnim.GetBool("IsLadder"))
                        {
                            myAnim.SetInteger("LadderIndex", 2);
                            if(ladderMove)
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

            case State.Fly:
                {
                    myRigid.MovePosition(this.transform.position + myDirecTion * Time.deltaTime * GameData.Instance.playerdata.MoveSpeed);
                }
                break;


        }
    }

    // 회전로직
    void KnighteRotate()
    {
        if (myJoystic.MoveOn && myJoystic.Dir != Vector3.zero && !myAnim.GetBool("IsAttack") && !myAnim.GetBool("IsPunch") && !myAnim.GetBool("IsChange") && !myAnim.GetBool("Block") && !myAnim.GetBool("IsLadder"))
        {
            MyCamRot = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
            myDirecTion = MyCamRot * new Vector3(myJoystic.Dir.x, 0.0f, myJoystic.Dir.z);
            MyCharRotate = Quaternion.LookRotation((this.transform.position + myDirecTion) - this.transform.position);
            MyChar.transform.rotation = Quaternion.Slerp(MyChar.transform.rotation, MyCharRotate, Time.deltaTime * 15.0f);
        }
    }

#endregion

    #region 사다리 관련

    public void LadderCheck()
    {
        if (Physics.Raycast(this.transform.position, -MyChar.transform.up, 0.05f, 1 << LayerMask.NameToLayer("Wall")))
        {
            Debug.Log("레이");
            myAnim.SetInteger("LadderIndex", 4);
            ChangeState(State.Relax);
        }
    }

    //사다리 무브 확인
    public void setladdermove(int index)
    {
        if (index > 0)
            ladderMove = true;
        else
            ladderMove = false;
    }

    #endregion

    #region 무기 교체, IK

    //손이 떼어지는 애니메이션일때 IK Weight 설정
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
        if (Value.Equals(0))
        {
            while (AnimationRig.weight != Value)
            {
                AnimationRig.weight = Value;
                yield return null;
            }
        }
        else
        {
            while (AnimationRig.weight != Value)
            {
                AnimationRig.weight += Time.deltaTime * 2.0f;
                yield return null;
            }
        }
    }

    //애니메이션에 맞춰 무기 교체
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

    public void CheckWeapon()
    {
        if(AttackSword!=null)
        {
            AttackSword.SetActive(false);
            RelaxSword.SetActive(false);
        }

        if (GameData.Instance.playerdata.Weapon == null)
        {
            AttackSword = Swords[0];
            RelaxSword = BackSwords[0];
            AttackSpot = Swords[0].GetComponentInChildren<Transform>();
        }
        else
        {
            AttackSword = Swords[1];
            RelaxSword = BackSwords[1];
            AttackSpot = Swords[1].GetComponentInChildren<Transform>();

        }

        if (myState == State.Battle)
        {
            AttackSword.SetActive(true);
            RelaxSword.SetActive(false);
        }
        else
        {
            AttackSword.SetActive(false);
            RelaxSword.SetActive(true);
        }

    }


    #endregion

    #region 점프 관련

    Vector3 JumpPos;
    float JumpDistance;

    public void AirCheck()
    {
        if (!Physics.Raycast(this.transform.position, -MyChar.transform.up, 0.1f))
        {
            if (!myAnim.GetBool("IsJump") && !myAnim.GetBool("IsAir") && myState != State.Ladder && !myAnim.GetBool("IsLadder"))
            {
                JumpPos = this.transform.position;
                myAnim.SetTrigger("Fall");
            }
            
        }
        else
        { 
            if (myAnim.GetBool("IsJump"))
            {
                myAnim.SetBool("IsJump", false);
            }
        }

    }

    public void AirDamageCheck()
    {
        JumpDistance = Vector3.Distance(JumpPos, this.transform.position);

        if (JumpDistance > 5.0f)
        {
            Uianim.SetTrigger("HPhit");
            DamageRoutine((int)(JumpDistance * 5.0f), 0);
            StartCoroutine(HitColor(myRenderer.material));
        }
        
    }

    //애니메이션 타이밍에 맞춰 점프함
    void Jump()
    {
        JumpPos = this.transform.position;
        LandEffect.SetActive(false);
        myRigid.AddForce(this.transform.up * 5.0f, ForceMode.Impulse);
        myAnim.SetBool("IsJump", true);
    }



    //착지할때 이펙트켜줌
    public void Landing()
    {
        LandEffect.SetActive(true);
        AirDamageCheck();
    }

    #endregion

    #region 암살기능

    //암살 기능
    public void Assasinationing()
    {
        if(myState == State.Relax)
        { 
            ChangeState(State.Assasination);
            myAnim.SetTrigger("Assasination");
        }
        else if(myState == State.Battle)
        {
            ChangeState(State.Assasination);
            myAnim.SetTrigger("Assasination2");
        }
    }

    public void Assamove(int index)
    {
        AssamoveCo = StartCoroutine(AssasinMove(index));
    }

    IEnumerator AssasinMove(int index)
    {
        switch(index)
        {
            case 0:
                while(true)
                {
                    myRigid.MovePosition(this.transform.position + (Detect.NearEnemy.transform.position - this.transform.position).normalized * Time.deltaTime * 1.0f);
                    yield return null;
                }
                
            case 1:
                while (true)
                {
                    myRigid.MovePosition(this.transform.position + -transform.forward * Time.deltaTime * 0.7f);
                    yield return null;
                }
            case 2:
                while (true)
                {
                    MyChar.transform.rotation = Quaternion.Slerp(MyChar.transform.rotation, Detect.NearEnemy.transform.rotation, Time.deltaTime * 3.0f);
                    this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(Detect.NearEnemy.transform.position.x, Detect.NearEnemy.transform.position.y, Detect.NearEnemy.transform.position.z+0.2f), Time.deltaTime * 3.0f);
                    yield return null;
                }

        }
    }

    public void StopAssaMove()
    {
        StopCoroutine(AssamoveCo);
    }

    //암살
    public void AssaEnemyTrriger(int index)
    {
        Detect.NearEnemy.GetComponent<Soldier>().OnAssa(index);
    }

    public void goTarget()
    {
        CapColl.isTrigger = true;
        myRigid.useGravity = false;
        Assamove(2);
       
    }

    public void EndAssa()
    {
        CapColl.isTrigger = false;
        myRigid.useGravity = true;
        ChangeState(State.Relax);

    }
#endregion

    #region 버튼 함수 모음

    public void SpecialAttack()
    {
        myAnim.SetTrigger("Special");
        StartCoroutine(SpecialCool());
    }

    IEnumerator SpecialCool()
    {
        yield return new WaitForSeconds(1.5f);

        myAnim.SetBool("IsSpecial", false);
    }


    public void SilentModeBtn()
    {
        if (SilentMode)
        {
            SilentMode = false;
            GameData.Instance.playerdata.MoveSpeed *= 2.0f;
        }
        else
        {
            SilentMode = true;
            GameData.Instance.playerdata.MoveSpeed *= 0.5f;
        }
    }


    //포션 흡입
    public void PotionConsume()
    {
        myAnim.SetTrigger("Activate");
        Potion[0].Play();
        Potion[1].Play();
    }

    public void WarpToHome()
    {
        myAnim.SetTrigger("Activate");
        UIManager.Instance.WarpUI(this.transform);
    }

    //픽업
    public void Pickup()
    {
        myAnim.SetTrigger("PickUp");
    }

    //점프버튼
    public void JumpButton()
    {
        if(!myAnim.GetBool("IsHang"))
        myAnim.SetTrigger("Jump");
    }

    //공격버튼
    public void AttackButton()
    {
        if (!myAnim.GetBool("IsHang"))
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
    }

    //실드 버튼
    public void BlockBtn()
    {
        if (!myAnim.GetBool("IsHang")&& !myAnim.GetBool("IsHang"))
        {
            IsBlock = !IsBlock;
            myAnim.SetBool("Block", IsBlock);
        }
    }

    //무기바꾸기 버튼
    //오디오 나중에 다른거 넣기!!
    public void WPChange_Btn()
    {
        if (!myAnim.GetBool("IsChange") && !myAnim.GetBool("IsHang"))
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
                    break;
            }
        }

    }

    public void skillbtn()
    {
        //if(DefenceSkill == null)
        
        myAnim.SetTrigger("Charge");
        SkillEffect[0].SetActive(true);
        mycamera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        mycamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = mynoisedef;
        DefenceSkill = StartCoroutine(DefenceUp());
        
    }

    IEnumerator DefenceUp()
    {
        yield return new WaitForSeconds(2.0f);
        mycamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = null ;
        myAnim.SetBool("IsCharge", false);
        SkillEffect[0].SetActive(false);
        SkillEffect[1].SetActive(true);
        GameData.Instance.playerdata.DEF += 10;

        yield return new WaitForSeconds(60.0f);
        GameData.Instance.playerdata.DEF -= 10;

        SkillEffect[1].SetActive(false);
        DefenceSkill = null;
    }
 
    #endregion

    #region 배틀 관련

    //오른쪽으로 후리면 0 , 왼쪽은 1 , 가운데는 2
    public void SwordAttack(int index)
    {
        myAudio.PlayOneShot(SoundManager.Instance.myEffectClip[Random.Range(6, 8)]);
        myCol = Physics.OverlapSphere(AttackSpot.position, 2.0f, 1 << LayerMask.NameToLayer("Monster"));
        if (myCol != null)
        {
            
            
            for (int i = 0; i < myCol.Length; i++)
            {   
                myCol[i].GetComponent<BattleSystem>()?.OnDamage(index, Random.Range(GameData.Instance.playerdata.ATK - 5, GameData.Instance.playerdata.ATK + 5), this.transform);
                myAnim.SetBool("Block", false);
                GameObject myeffect = Instantiate(Effect, myCol[i].transform.position + new Vector3(0,1.0f,-0.5f), Quaternion.identity);
            }
        }
        myCol = null;
    }

    //맨손으로 죽일때도


    //오른쪽 펀치 공격할때
    public void RPunchAttack()
    {
        myCol = Physics.OverlapSphere(RightHand.position, 1.0f, 1 << LayerMask.NameToLayer("Monster"));
        if (myCol != null)
        {
            for (int i = 0; i < myCol.Length; i++)
            {
                myCol[i].GetComponent<BattleSystem>()?.OnDamage(0, Random.Range(20,30), this.transform);
            }
        }
        myCol = null;
    }

    //왼손으로 펀치할때
    public void LPunchAttack(int index)
    {
        myCol = Physics.OverlapSphere(LeftHand.position, 1.0f, 1 << LayerMask.NameToLayer("Monster"));
        if (myCol != null)
        {
            for (int i = 0; i < myCol.Length; i++)
            {
                myCol[i].GetComponent<BattleSystem>()?.OnDamage(index, Random.Range(20,30), this.transform);
            }
        }
        myCol = null;
    }

    //맞았을때
    public bool OnDamage(int index, float damage,Transform tr)
    {
        Uianim.SetTrigger("HPhit");
        StartCoroutine(HitColor(myRenderer.material));

        //방패로 막고있다면
        if (myAnim.GetBool("IsBlock"))
        {
            int rand = Random.Range(1, 11);
            myAudio.PlayOneShot(SoundManager.Instance.myEffectClip[3]);
            switch (index)
            {
                //오른쪽 맞았을때
                case 0:
                    myAnim.SetTrigger("BlockHitR");
                    break;

                //왼쪽 맞았을때
                case 1:
                    myAnim.SetTrigger("BlockHitL");
                    break;        
            }

            DamageRoutine((int)((damage - GameData.Instance.playerdata.DEF) * 0.5f), 1);

            //일정 확률로 리턴을 하여 스턴시킴
            if (rand < 9 && !myAnim.GetBool("IsRelax"))
            {
                return false;
            }
        }
        else
        {
            myAudio.PlayOneShot(SoundManager.Instance.myEffectClip[4]);
            if (myState != State.Assasination)
            {
                switch (index)
                {
                    //오른쪽 맞았을때
                    case 0:
                        myAnim.SetTrigger("GetHitR");
                        break;

                    //왼쪽 맞았을때
                    case 1:
                        myAnim.SetTrigger("GetHitL");
                        break;
                }
            }
            DamageRoutine((int)(damage - GameData.Instance.playerdata.DEF), 2);
        }
        return true;
    }

    //데미지 받는 함수 
    public void DamageRoutine(float Damage, int index)
    {
        DamageT = Damage < 0 ? 0 : Damage;
        GameObject obj = ObjectPool.Instance.ObjectManager[3].Get();
        obj.GetComponent<DamageText>()?.SetTextP(this.transform, DamageT.ToString(), index);
        GameData.Instance.playerdata.CurHP -= DamageT;
        UIManager.Instance.SetHP();
    }

    

    IEnumerator HitColor(Material mat)
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        mat.color = Color.white;
    }

    public void SetSwordtrail(int index)
    {

        SwordTrail.SetActive(0==index);
    }

    #endregion
}
