using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public AutoDetect Detect;

    public GameObject Sword;
    public GameObject BackSword;

    public GameObject HpCanvas;
    public Slider HPSlider;

    protected Vector3 Dir = Vector3.zero;
    protected bool Move = false;
    protected Collider[] myCol;
    protected Coroutine AttackDelay;
    protected Coroutine StunCo;
    public float HitTime = 0.0f;
    public Button Killbtn;

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
        Patrol, Battle, Death, Stun
    }

    //��Ʈ�ѷ� ����
    public S_State myState = S_State.Patrol;




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
                myAnim.SetBool("IsWalk", false);
                break;

            case S_State.Battle:
                myAnim.SetBool("IsWalk", true);
                break;
            case S_State.Stun:

                Move = false;
                if (StunCo == null)
                    StunCo = StartCoroutine(Stun(3.0f));
                else
                {
                    StopCoroutine(StunCo);
                    StunCo = StartCoroutine(Stun(3.0f));
                }

                break;
            case S_State.Death:
                Death();
                Move = false;
                HpCanvas.gameObject.SetActive(false);
                StartCoroutine(DeathAfter(2.0f));
                break;

        }
    }

    IEnumerator DeathAfter(float time)
    {
        yield return new WaitForSeconds(time);

        while(this.transform.position.y > -10.0f)
        {
            this.transform.position += Vector3.down * Time.deltaTime*0.5f;
            yield return null;
        }

        this.transform.gameObject.SetActive(false);
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
                
                Patrol();
                break;
            case S_State.Battle:
                BattleState();

                break;
            case S_State.Stun:

                break;


            case S_State.Death:
          
                break;
        }
    }

    public void Patrol()
    {
        if (Detect.Enemy.Count > 0)
        {
            ChangeState(S_State.Battle);
        }

    }

    //������ٵ� �̵�
    private void FixedUpdate()
    {
        if (Move && HitTime < 0.1f && myState != S_State.Stun && !myAnim.GetBool("IsAttack") && myState != S_State.Patrol)
            myRigid.MovePosition(this.transform.position + Dir.normalized * Time.deltaTime * myStat.Speed);
    }


    //���� �ɷ�����
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

    //��Ʋ��Ȳ�϶�
    public void BattleState()
    {
        //���� ������� ��Ʈ�ѷ� ��ȯ
        if (Detect.Enemy.Count == 0)
        {
            ChangeState(S_State.Patrol);
        }

        //������ ��Ÿ�� �þ
        if (myAnim.GetBool("IsHit"))
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
            if (HitTime > 0.0f)
                HitTime -= Time.deltaTime;
        }

        //��Ÿ���� �������� ���� ��ƾ

        if (HitTime < 0.1f && Detect.Enemy.Count != 0)
        {
            //�Ÿ��� 2.0f���� ũ�ٸ�
            if (Vector3.Distance(this.transform.position, Detect.Enemy[0].transform.position) > 2.0f)
            {
                Dir = Detect.Enemy[0].transform.position - this.transform.position;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(Dir), Time.deltaTime * 20.0f);
                Move = true;
                myAnim.SetBool("IsWalk", true);

            }
            ////�Ÿ��� 2.0f���� �۴ٸ�
            else
            {
                Move = false;
                myAnim.SetBool("IsWalk", false);
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

        yield return new WaitForSeconds(time);

        AttackDelay = null;
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
                if(myCol[i].GetComponent<BattleSystem>() != null)
               if(!myCol[i].GetComponent<BattleSystem>().OnDamage(index,Random.Range(myStat.ATK-5, myStat.ATK + 5)))
                {
                    ChangeState(S_State.Stun);
                    
                }
            }
        }
        myCol = null;
    }


    //������ �Ծ������� ����
    public bool OnDamage(int index,float damage)
    {
        StartCoroutine(HitColor(myRenderer.material));

        //���з� ����
        Rand = Random.Range(1, 11);
        if(Rand < 3)
        {
            myAudio.PlayOneShot(SoundManager.Instance.myEffectClip[3]);
            myAnim.SetTrigger("GetHitS");

            DamageRoutine((int)((damage - myStat.DEF) * 0.5f), 1);
            return false;
        }

        //�ָ����� ������ �Ҹ� �ֱ�
        myAudio.PlayOneShot(SoundManager.Instance.myEffectClip[4]);

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

    //������ �ؽ�Ʈ ������ƮǮ, ������ ���ð���
    public void DamageRoutine(float damage,int index)
    {
        DamageT = damage;
        GameObject obj1 = ObjectPool.Instance.ObjectManager[4].Get();
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

    private void OnEnable()
    {
        HpCanvas.gameObject.SetActive(true);
        myStat.InitStat(myData.SoldierName, myData.MaxHP, myData.HP, myData.ATK, myData.DEF, myData.Speed, myData.AttackDelay, myData.EXP);
    }

    
}
