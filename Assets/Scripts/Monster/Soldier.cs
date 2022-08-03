using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public struct Stat
{
    public string SoldierName;
    public float MaxHP;
    public float HP;
    public float ATK;
    public float DEF;
    public float Speed;
    public float AttackDelay;
    public float EXP;

    public void InitStat(string name, float MaxHp, float HP ,float ATK,float DEF, float Speed, float AttackDelay, float EXP )
    {
        this.SoldierName = name;
        this.MaxHP = MaxHp;
        this.HP = HP;
        this.ATK = ATK;
        this.DEF = DEF;
        this.Speed = Speed;
        this.AttackDelay = AttackDelay;
        this.EXP = EXP;
    }
}



public abstract class Soldier : MonoBehaviour, BattleSystem
{
    #region ������ҽ�,������ٵ�,�ִϸ����� ������Ƽ

    private AudioSource _myAudio;
    public AudioSource myAudio
    {
        get
        {
            _myAudio = GetComponent<AudioSource>();
            return _myAudio;
        }
    }

    private Rigidbody _myRigid;
    public Rigidbody myRigid
    {
        get
        {
            _myRigid = GetComponent<Rigidbody>();
            return _myRigid;
        }
    }


    private Animator _myAnim;
    public Animator myAnim
    {
        get
        {
            _myAnim = GetComponent<Animator>();
            return _myAnim;
        }
    }

    #endregion

    #region ������, �߻��Լ���

    protected SkinnedMeshRenderer myRenderer;
    [SerializeField]
    protected SoldierData myData;
    [SerializeField]
    protected Stat myStat;
    public FieldOfViewAngle Detect;
    Vector3 PatrolDir=Vector3.zero;
    public LayerMask PatrolMask;


    public GameObject Sword;

    public GameObject HpCanvas;
    public Slider HPSlider;

    protected Vector3 Dir = Vector3.zero;
    protected Collider[] myCol;
    protected Coroutine Wait;
    protected Coroutine AttackDelay;
    protected Coroutine StunCo;
    public float HitTime = 0.0f;
    protected bool IsAssaDeath = false;
    public NavMeshAgent myNavi;
    public int patrolIndex = 0;

    //���̾��� ���� ��������� ����
    int Rand;
    int ItemRand;
    float DamageT;

    //�߻��Լ�
    public abstract void Death();

    #endregion

    #region �����ũ,��ŸƮ,������Ʈ

    // �ε��� ���� ����
    private void Awake()
    {
        myStat = new Stat();
        myStat.InitStat(myData.SoldierName, myData.MaxHP, myData.HP, myData.ATK, myData.DEF, myData.Speed, myData.AttackDelay, myData.EXP);
        myRenderer = this.GetComponentInChildren<SkinnedMeshRenderer>();
    
    }

    private void Start()
    {
       SoundManager.Instance.AddEffectSource(this.GetComponent<AudioSource>());
    }



    private void Update()
    {

        HpCanvas.transform.rotation = Camera.main.transform.rotation;
        StateProcess();

    }

    #endregion

    //������ ����
    public enum S_State
    {
        Create,Patrol, Battle, Death, Stun, Assasination
    }

    //��Ʈ�ѷ� ����
    public S_State myState = S_State.Create;




    public void FootR()
    {
        myAudio.PlayOneShot(SoundManager.Instance.myEffectClip[9]);
    }

    public void FootL()
    {
        myAudio.PlayOneShot(SoundManager.Instance.myEffectClip[8]);
    }

    public void ChangeState(S_State s)
    {
        if (myState.Equals(s)) return;

        myState = s;

        switch (myState)
        {
            case S_State.Patrol:
                
                myNavi.speed = 1.0f;
                myNavi.isStopped = true;
                myAnim.SetBool("IsRun", false);
                if(patrolIndex > 6)
                { 
                    myAnim.SetBool("IsWalk", true);
                    SetAiDir();
                }
                else
                    MonsterSpawnManager.Instance.GetStandDir(patrolIndex);
                myNavi.isStopped = false;
                break;

            case S_State.Battle:
                {
                    ChangeBattle();

                }
                break;
            case S_State.Stun:

                //Move = false;
                if (StunCo == null)
                    StunCo = StartCoroutine(Stun(3.0f));
                else
                {
                    StopCoroutine(StunCo);
                    StunCo = StartCoroutine(Stun(3.0f));
                }

                break;

            case S_State.Assasination:
                myNavi.isStopped = true;
                if (Wait != null)
                {
                    StopCoroutine(Wait);
                    Wait = null;
                }
                
                break;

            case S_State.Death:
                myNavi.enabled = false;
                if (Wait != null)
                {
                    StopCoroutine(Wait);
                    Wait = null;
                }
                if (AttackDelay != null)
                {
                    StopCoroutine(AttackDelay);
                    AttackDelay = null;
                }
                Death();
                NotifyToPlayer();
                HpCanvas.gameObject.SetActive(false);
               
                break;

        }
    }

    public void NotifyToPlayer()
    {
        Collider[] col = Physics.OverlapSphere(this.transform.position, 5.0f, 1 << LayerMask.NameToLayer("Player"));
        for (int i = 0; i < col.Length; i++)
        {
            col[i].GetComponent<Knight>().Detect.Enemy.Remove(this.gameObject);
        }
    }


    protected IEnumerator DeathAfter(float time,int index)
    {
        yield return new WaitForSeconds(time);

        while (this.transform.position.y > -2.0f)
        {
            this.transform.position += Vector3.down * Time.deltaTime * 0.5f;
            yield return null;
        }

        ObjectPool.Instance.ObjectManager[index].Release(this.gameObject);

        yield return null;
    }

    void StateProcess()
    {

        if (myStat.HP <= 0.0f)
        {
            ChangeState(S_State.Death);
        }


        switch (myState)
        {
            case S_State.Patrol:

                AIPatrol();
                break;
            case S_State.Battle:
                BattleState();

                break;
            case S_State.Stun:

                break;

            case S_State.Assasination:
                break;
        }
    }


    public void SetAiDir()
    {
        myNavi.SetDestination(MonsterSpawnManager.Instance.GetPatrolDir(patrolIndex,this.transform));
    }

    int AIRand = 0;

    public void AIPatrol()
    {

        if(Detect.Enemy.Count > 0)
        {
            ChangeState(S_State.Battle);
        }

        if (myNavi.remainingDistance < 0.1f)
        { 
            if (patrolIndex > 6)
            {
                AIRand = Random.Range(0, 2);
                if (Wait == null)
                {
                    if (AIRand == 0)
                        SetAiDir();
                    else
                        Wait = StartCoroutine(WaitPatrol());
                }         
            }
            else
            {
                if (Wait == null)
                {
                    Wait = StartCoroutine(StandPatrol(AIRand++ % 2));
                }

            }
        }
        
        
    }
    IEnumerator StandPatrol(int rand)
    {

        myAnim.SetBool("IsWalk", false);
        yield return new WaitForSeconds(Random.Range(3.0f,5.0f));
        myAnim.SetBool("IsWalk", true);

        if(rand == 0)
            SetAiDir();
        else
            MonsterSpawnManager.Instance.GetStandDir(patrolIndex);

        Wait = null;
    }



    IEnumerator WaitPatrol()
    {
        myAnim.SetBool("IsWalk", false);

        yield return new WaitForSeconds(Random.Range(3.0f, 5.0f));
        myAnim.SetBool("IsWalk", true);
        SetAiDir();
        Wait = null;
    }



    //���� �ɷ�����
    protected IEnumerator Stun(float time)
    {

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

    }

    public void ChangeBattle()
    {
        myAnim.SetBool("IsRun", true);
       
        myNavi.speed = 2.0f;
        if (Wait != null)
        {
            StopCoroutine(Wait);
            Wait = null;
        }
        myAnim.SetBool("IsWalk", false);
        myNavi.SetDestination(Detect.Enemy[0].transform.position);
    }

    //��Ʋ��Ȳ�϶�
    public void BattleState()
    {
        //���� ������� ��Ʈ�ѷ� ��ȯ
        if (Detect.Enemy.Count == 0)
        {
            ChangeState(S_State.Patrol);
        }

        Debug.Log(myNavi.remainingDistance);
        Debug.DrawLine(this.transform.position, new Vector3(myNavi.destination.x, myNavi.destination.y, myNavi.destination.z),Color.red);

        //������ ��Ÿ�� �þ
        if (myAnim.GetBool("IsHit"))
        {
            HitTime = 0.5f;
            myNavi.isStopped = true;
            if (AttackDelay != null)
            {
                StopCoroutine(AttackDelay);
                AttackDelay = null;
            }
        }
        else
        {
            if (HitTime > 0.0f)
                HitTime -= Time.deltaTime;
        }

        //��Ÿ���� �������� ���� ��ƾ
        if (HitTime < 0.1f && Detect.Enemy.Count != 0)
        {
           
            //�Ÿ��� 1.5f���� ũ�ٸ�
            if (myNavi.remainingDistance > 1.5f)
            {
                myNavi.SetDestination(Detect.Enemy[0].transform.position);
                myNavi.isStopped = false;
                myAnim.SetBool("IsRun", true);
            }
            //�Ÿ��� 1.5f���� �۴ٸ�
            else
            {
                myNavi.isStopped = true;
                myAnim.SetBool("IsRun", false);
                if (AttackDelay == null)
                {
                    AttackDelay = StartCoroutine(AtkDelay(myStat.AttackDelay));
                }

            }
        }
    }

    //���� ��Ÿ��
    IEnumerator AtkDelay(float time)
    {
        myAnim.SetTrigger("Attack");
        myAnim.SetBool("IsWalk", false);
        yield return new WaitForSeconds(time);

        AttackDelay = null;

        if(Detect.Enemy.Count != 0 && myState != S_State.Death)
        myNavi.SetDestination(Detect.Enemy[0].transform.position);
    }

    //��������
    public void OnAttack(int index)
    {
        myCol = Physics.OverlapSphere(Sword.transform.position, 2f, 1 << LayerMask.NameToLayer("Player"));
        if (myCol != null)
        {
            for (int i = 0; i < myCol.Length; i++)
            {
                //��üũ, ��밡 ���Ҵ��� Ȯ��
                if (myCol[i].GetComponent<BattleSystem>() != null)
                    if (!myCol[i].GetComponent<BattleSystem>().OnDamage(index, Random.Range(myStat.ATK - 5, myStat.ATK + 5), this.transform))
                    {
                        
                        ChangeState(S_State.Stun);
                        
                    }
                    else
                        myCol[i].GetComponent<BattleSystem>()?.DamageSound(0);
            }
        }
        myCol = null;
    }

    public void LookRotation(Transform tr)
    {
        this.transform.LookAt(tr.transform.position);
    }

    public void OnAssa(int index)
    {
        ChangeState(S_State.Assasination);

        switch(index)
        {
            case 0:
                myAnim.SetTrigger("Assassination");
                break;
            case 1:
                myAnim.SetTrigger("OneShot");
                DamageRoutine(myStat.HP, 0);
                break;
        }
    }

    public void OnDamageAssa()
    {
        DamageRoutine(myStat.HP, 0);
        IsAssaDeath = true;
    }

    IEnumerator MoveAttackDelay()
    {
        myNavi.isStopped = true;

        yield return null;

        if (Detect.Enemy.Count != 0 && myState != S_State.Death)
        {
            myNavi.isStopped = false;
            myNavi.SetDestination(Detect.Enemy[0].transform.position);
        }
            
    }


    //������ �Ծ������� ����
    public bool OnDamage(int index,float damage,Transform tr)
    {
        if (myState != S_State.Death)
        {
            GameObject myeffect = ObjectPool.Instance.Effects[2].Get();
            myeffect.transform.position = this.transform.position + new Vector3(0, 1.0f, -0.5f);

            if (myState == S_State.Patrol)
            {
                this.transform.LookAt(tr);
                ChangeState(S_State.Battle);
            }

            StartCoroutine(HitColor(myRenderer.material));

            //���з� ����
            Rand = Random.Range(1, 11);
            if (Rand < 3)
            {
                myAudio.PlayOneShot(SoundManager.Instance.myEffectClip[3]);
                myAnim.SetTrigger("GetHitS");

                DamageRoutine((int)((damage - myStat.DEF) * 0.5f), 1);
                return false;
            }

            switch (index)
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
                    DamageRoutine(myStat.HP, 0);
                    return true;
            }
           
            DamageRoutine((int)(damage - myStat.DEF), 0);

            return true;
        }
        return true;
    }

    public void DamageSound(int SoundIndex)
    {
        if (myState == S_State.Death) return;

        switch (SoundIndex)
        {
            //Į�¾�����
            case 0:
                myAudio.PlayOneShot(SoundManager.Instance.myEffectClip[4]);
                break;
            case 1:
                //�ָ����� �¾�����
                myAudio.PlayOneShot(SoundManager.Instance.myEffectClip[25]);
                break;
        }
    }

    //������ �ؽ�Ʈ ������ƮǮ, ������ ���ð���
    public void DamageRoutine(float damage,int index)
    {
        DamageT = damage;
        GameObject obj1 = ObjectPool.Instance.ObjectManager[3].Get();
        obj1.GetComponent<DamageText>()?.SetText(this.transform, DamageT.ToString(), index);
        myStat.HP -= DamageT;
        SetHp();
    }

    //���������� HP�� ����
    public void SetHp()
    {
        HPSlider.value = myStat.HP / myStat.MaxHP;
    }

    //�¾����� ����ȭ
    IEnumerator HitColor(Material mat)
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        mat.color = Color.white;
    }

    //��������� ���ϴ� �Լ�
    protected void dropRand(int index, int MinMoney, int MaxMoney)
    {
        ObjectPool.Instance.GetItem(index, this.transform, MinMoney, MaxMoney);

    }

    private void OnDisable()
    {
        myNavi.enabled = false;
        HpCanvas.gameObject.SetActive(true);
        myStat.InitStat(myData.SoldierName, myData.MaxHP, myData.HP, myData.ATK, myData.DEF, myData.Speed, myData.AttackDelay, myData.EXP);
        IsAssaDeath = false;
        SetHp();
       
    }

    
    
}
