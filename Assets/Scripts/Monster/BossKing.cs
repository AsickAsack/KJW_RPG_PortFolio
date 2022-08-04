using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct BossStat
{
    public string SoldierName;
    public float MaxHP;
    public float HP;
    public float ATK;
    public float DEF;
    public float Speed;
    public float AttackDelay;
    public float EXP;

    public void InitStat(string name, float MaxHp, float HP, float ATK, float DEF, float Speed, float AttackDelay, float EXP)
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



public class BossKing : MonoBehaviour, BattleSystem
{
    public enum KingState
    {
        Idle, Battle_Near, Battle_Far, Death
    }

    public KingState Mystate = KingState.Idle;

    public SoldierData KingData;
    public Animator myAnim;
    public Transform FootPos;
    public Transform[] SkillPos;
    public SkinnedMeshRenderer myRenderer;
    public AudioSource myAudio;
    public BossStat stat;
    public KingDetect Detect;
    public GameObject MoveEffect;
    public ParticleSystem MoveSmoke;
    private GameObject[] Fireball = new GameObject[3];
    Coroutine BattleRoutine = null;
    private float DamageText;
    Collider[] Coll;
    int Rand;
    public bool IsSkill = false;
    public GameObject FireShieldEffect;
    private GameObject SpecialFire;
    public GameObject RecoveryEffect;
    public GameObject SparkSphere;
    BossKing Boss;

    [Header("[체력 UI]")]
    public Slider HP_Slider;
    public Text Hp_Text;
    public Animator Hp_Anim;


    //데이터 붙여넣기
    private void Awake()
    {
        stat.InitStat(KingData.SoldierName, KingData.MaxHP, KingData.HP, KingData.ATK, KingData.DEF, KingData.Speed, KingData.AttackDelay, KingData.EXP);
       // stat.HP *= 0.2f;
        SetHp();
        Boss = this.GetComponent<BossKing>();

    }


    void ChangeState(KingState s)
    {
        if (Mystate == s) return;
        Mystate = s;

        switch (Mystate)
        {
            case KingState.Battle_Near:
                Battle_Near();
                break;
            case KingState.Battle_Far:
                break;
            case KingState.Death:
                myAnim.SetTrigger("Death");
                StartCoroutine(DelayGameOver());
                break;
        }

    }
    IEnumerator DelayGameOver()
    {
        yield return new WaitForSeconds(2.0f);
        SceneLoader.Instance.Loading_LoadScene(4);
    }


    void StateProcess()
    {
        switch (Mystate)
        {

            case KingState.Idle:
             
                break;



            case KingState.Battle_Near:
                LookRot();
                break;

            case KingState.Battle_Far:
                Battle_Far();
                LookRot();
                break;

        }

    }

    private void Update()
    {
        StateProcess();
    }

    public void CameraShake(int index)
    {
        Detect.knight.CameraShake(index);

    }

    void Battle_Near()
    {
        myAnim.SetTrigger("Kick");
    }

    public void MoveStart()
    {
        BattleRoutine = StartCoroutine(MovePos());
    }

    public void LookRot()
    {
        if (!myAnim.GetBool("IsMove"))
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(Detect.Enemy[0].transform.position - this.transform.position), Time.deltaTime * 20.0f);
    }


    IEnumerator MovePos()
    {
        IsSkill = true;
        MoveEffect.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        myAnim.SetBool("IsMove", false);
        this.transform.position = MonsterSpawnManager.Instance.GetKingMovePos();
        //Pivot.LookAt(this.transform); 적 바라보게?
        ChangeState(KingState.Battle_Far);
        MoveEffect.SetActive(false);
        BattleRoutine = null;

    }

    public void PlayMoveSmoke()
    {
        MoveSmoke.Play();
    }

    public void Skillcheck()
    {
        IsSkill = false;
    }

    void Battle_Far()
    {
        if (Vector3.Distance(Detect.Enemy[0].transform.position, this.transform.position) < 2.5f && !IsSkill)
        {
            ChangeState(KingState.Battle_Near);
        }
        else
            if (!IsSkill && BattleRoutine == null)
            BattleRoutine = StartCoroutine(FarRoutine());
    }

    

    IEnumerator FarRoutine()
    {
        IsSkill = true;
        if (stat.HP / stat.MaxHP > 0.5f)
        {
            Rand = Random.Range(1, 11);
            if (Rand < 6)
            {
                myAnim.SetTrigger("FireBall");
                yield return new WaitForSeconds(6.0f);
            }
            else
            {
                myAnim.SetTrigger("Special");
                yield return new WaitForSeconds(1.0f);
                ShotSpecial();
                yield return new WaitForSeconds(1.0f);
                myAnim.SetBool("IsSpecial", false);
                yield return new WaitForSeconds(5.0f);
            }
        }
        else
        {

            Rand = Random.Range(1, 11);
            if (Rand <= 1)
            {
                //회복
                myAudio.PlayOneShot(SoundManager.Instance.myEffectClip[12]);
                myAnim.SetTrigger("Heal");
                yield return new WaitForSeconds(2.5f);
            }
            else if (Rand > 1 && 5 >= Rand)
            {
                myAnim.SetTrigger("Spark");
                CameraShake(0);
                yield return new WaitForSeconds(3.0f);
                myAnim.SetBool("IsSpark", false);
                CameraShake(1);

                yield return new WaitForSeconds(3.0f);
            }
            else
            {
                Rand = Random.Range(2, 5);
                myAnim.SetTrigger("Special");
                yield return new WaitForSeconds(1.0f);
                for(int j=0;j<Rand;j++)
                { 
                    ShotSpecial();
                    yield return new WaitForSeconds(1.0f);
                }
                myAnim.SetBool("IsSpecial", false);
                yield return new WaitForSeconds(5.0f);
            }
            
        }
        BattleRoutine = null;
    }


    GameObject SparkGround;
    Spark spark;

    public void SetSpark()
    {
        IsSkill = true;
        SparkSphere.gameObject.SetActive(true);
        SparkGround = ObjectPool.Instance.Effects[6].Get();
        SparkGround.transform.position = Detect.Enemy[0].transform.position;
        SparkRoutine();
        for (int i = 0; i < 9; i++)
        {
            SparkGround = ObjectPool.Instance.Effects[6].Get();
            SparkGround.transform.position = MonsterSpawnManager.Instance.GetKingMovePos();
            SparkRoutine();
        }
    }

    public void SparkRoutine()
    {
        spark = SparkGround.GetComponent<Spark>();
        spark.boss = Boss;
        spark.SetAirSpark();
    }

    public void EndSpark()
    {
        SparkSphere.gameObject.SetActive(false);
        IsSkill = false;
    }


    public void DoRecovery()
    {
        RecoveryEffect.SetActive(true);
        stat.HP += 100.0f;
        SetHp();
    }

    public void EndReCovery()
    {
        RecoveryEffect.SetActive(true);
        IsSkill = false;
    }

    public void SetSpecial()
    {
        FireShieldEffect.SetActive(true);
    }

    FireSkill fireSkill;

    public void ShotSpecial()
    {
        SpecialFire = ObjectPool.Instance.Effects[3].Get();
        fireSkill = SpecialFire.GetComponent<FireSkill>();
        fireSkill.boss = Boss;

        SpecialFire.transform.position = Detect.Enemy[0].transform.position + new Vector3(0, 5.5f, 0.0f);
    }



    public void EndSpecial()
    {
        FireShieldEffect.SetActive(false);
        IsSkill = false;
    }


    public void SetFireball()
    {
        Rand = Random.Range(1, 4);
        myAudio.PlayOneShot(SoundManager.Instance.myEffectClip[19]);
        for (int i=0;i<Rand;i++)
        {
            Fireball[i] = ObjectPool.Instance.Effects[1].Get();
            Fireball[i].transform.position = SkillPos[i].position;
        }
    }

    public void ShotFireBall()
    {
        
        myAnim.SetBool("IsFireBall", false);
        for (int i = 0; i < Rand; i++)
        {
            Fireball[i].GetComponent<Fireball>().Shot(this.transform, Detect.Enemy[0].transform, stat.ATK);
        }
        
    }
    public void KnockBackKick()
    {
        Coll = Physics.OverlapSphere(FootPos.position, 1.0f, 1<<LayerMask.NameToLayer("Player"));

        for(int i=0;i<Coll.Length;i++)
        {
            if(Coll[i].GetComponent<BattleSystem>() != null)
            {
                Coll[i].GetComponent<BattleSystem>()?.DamageSound(1);
                Coll[i].GetComponent<BattleSystem>().OnDamage(2, stat.ATK, this.transform);
                Coll[i].GetComponent<Rigidbody>().AddForce(this.transform.forward * 5.0f, ForceMode.Impulse);
            }
        }
            

    }

    void SetHp(bool anim = false)
    {
        HP_Slider.value = stat.HP / stat.MaxHP;
        Hp_Text.text = ((int)(stat.HP)).ToString() + " / "+ ((int)(stat.MaxHP)).ToString();

        if (anim)
            Hp_Anim.SetTrigger("HitUI");
    }


    public bool OnDamage(int index, float Damage, Transform tr)
    {

        if (Mystate != KingState.Death)
        {
            GameObject myeffect = ObjectPool.Instance.Effects[2].Get();
            myeffect.transform.position = this.transform.position + new Vector3(0, 1.0f, -0.5f);



            if (!IsSkill && !myAnim.GetBool("IsMove") && !myAnim.GetBool("IsFireBall") && !myAnim.GetBool("IsSpark")) 
            { 
                switch (index)
                {
                    case 0:
                        myAnim.SetTrigger("GetHitL");
                        break;
                    case 1:
                        myAnim.SetTrigger("GetHitR");
                        break;      
                    case 3:
                        myAnim.SetTrigger("GetHitDown");
                        break;                    

                }
            }

            StartCoroutine(HitColor(myRenderer.material));
            DamageRoutine((int)(Damage - stat.DEF), 0);
            return true;

        }
        return true;
    }

    public void DamageSound(int SoundIndex)
    {
        if (Mystate == KingState.Death) return;

        switch (SoundIndex)
        {
            //칼맞았을때
            case 0:
                myAudio.PlayOneShot(SoundManager.Instance.myEffectClip[4]);
                break;
            case 1:
                //주먹으로 맞았을때
                myAudio.PlayOneShot(SoundManager.Instance.myEffectClip[25]);
                break;
        }
    }

    //맞았을때 색변화
    IEnumerator HitColor(Material mat)
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        mat.color = Color.white;
    }

    //데미지 텍스트 오브젝트풀, 데미지 세팅과정
    public void DamageRoutine(float damage, int index)
    {
        DamageText = damage < 0 ? 0 : damage;
        GameObject obj1 = ObjectPool.Instance.ObjectManager[3].Get();
        obj1.GetComponent<DamageText>()?.SetText(this.transform, DamageText.ToString(), index);
        stat.HP -= DamageText;
        if (stat.HP <= 0.0f)
        {
            stat.HP = 0.0f;
            ChangeState(KingState.Death);
        }
        SetHp(true);
    }
}
