using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{

    public static ObjectPool Instance;

    public ObjectPool<GameObject>[] ObjectManager = new ObjectPool<GameObject>[10];
    public GameObject BulletPf;
    public GameObject SkillBulletPf;
    public GameObject CoinPf;
    public GameObject SlimePf;
    public GameObject SlimeEtcPf;
    public GameObject TurtleShellPf;
    public GameObject TurtleShellEtcPf;
    public GameObject MolePf;
    public GameObject MoleEtcPf;
    public GameObject DamageTextPf;

    public Transform[] folder;
    public Queue<GameObject> RedBullet = new Queue<GameObject>();

    private void Awake()
    {

        Instance = this;

        for(int i=0;i<5;i++)
        {
            GameObject obj = Instantiate(BulletPf,Vector3.zero,Quaternion.identity, folder[0]);
            RedBullet.Enqueue(obj);
            obj.SetActive(false);
        }


        Init(0, BulletPf);
        Init(1, SkillBulletPf);
        Init(2, CoinPf);
        Init(3, SlimePf);
        Init(4, SlimeEtcPf);
        Init(5, TurtleShellPf);
        Init(6, TurtleShellEtcPf);
        Init(7, MolePf);
        Init(8, MoleEtcPf);
        Init(9, DamageTextPf);
 
    }

    void Init(int index ,GameObject prefab)
    {
        ObjectManager[index] = new ObjectPool<GameObject>(
        //새로운 오브젝트를 생성할때
        createFunc: () =>
        {
            var CreateObj = Instantiate(prefab,folder[index]);
            return CreateObj;
        },
        //가져갈때
        actionOnGet: (Obj) =>
        {
            Obj.gameObject.SetActive(true);
        },
        // 다시 가지고 올때
        actionOnRelease: (Obj) =>
        {  

            

            Obj.gameObject.SetActive(false);
        },
        //타겟 오브젝트에게 적용할 함수
        actionOnDestroy: (Obj) =>
        {
            Destroy(Obj.gameObject);
        }, maxSize: 5);
    }


    public void DropItem(int index,int index2,Vector3 pos)
    {
        
     
        GameObject DropTem = ObjectPool.Instance.ObjectManager[index].Get();
        DropTem.transform.position = pos;

        GameObject DropTem2 = ObjectPool.Instance.ObjectManager[index2].Get();
        DropTem2.transform.position = pos;

        int RandomNum = Random.Range(0, 2);
        Vector3 RandomVector = RandomNum <= 0 ? Vector3.left : Vector3.right;

        DropTem.GetComponent<Rigidbody>().AddForce(Vector3.up * 200 + RandomVector * 100);
        DropTem2.GetComponent<Rigidbody>().AddForce(Vector3.up * 200 + RandomVector * 100);

    }

    public GameObject GetBullet()
    {
        Debug.Log(RedBullet.Count);

        if(RedBullet.Count > 0)
        {
            GameObject obj = RedBullet.Dequeue();
            obj.GetComponent<TrailRenderer>().Clear();
            obj.transform.SetParent(null);
            
            return obj;
        }
        else
        {
            return Instantiate(BulletPf, Vector3.zero, Quaternion.identity, folder[0]);

        }
    }


    public void ReturnBullet(GameObject obj)
    {
        obj.transform.position = Vector3.zero;
        obj.SetActive(false);
        obj.transform.SetParent(folder[0]);
        RedBullet.Enqueue(obj);

    }
}




