using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{

    public static ObjectPool Instance;

    public ObjectPool<GameObject>[] ObjectManager = new ObjectPool<GameObject>[6];
    public ItemData[] itemDatas;
    public GameObject DropItem;
    public GameObject SwordMan;
    public GameObject SpearMan;
    public GameObject S_SwordMan;
    public GameObject Damage_text;
    public GameObject Coin;

    public Transform[] folder;
    int ItemRand;

    private void Awake()
    {

        Instance = this;

        Init(0, DropItem);
        Init(1, SwordMan);
        Init(2, SpearMan);
        Init(3, S_SwordMan);
        Init(4, Damage_text);
        Init(5, Coin);


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


    public GameObject GetItem(int index,Transform tr)
    {
        GameObject obj = ObjectManager[0].Get();
        obj.transform.position = tr.transform.position+ new Vector3(-0.5f,0.0f,0.0f);
        //obj.transform.rotation = tr.transform.rotation;
        DropItem temp = obj.GetComponent<DropItem>();
        temp.itemData.Add(itemDatas[index]);

            return obj;

    }
    
    public GameObject GetItem(int index, Transform tr, int minMoney, int MaxMoney)
    {
        GameObject obj = ObjectManager[0].Get();
        obj.transform.position = tr.transform.position + new Vector3(-0.5f, 0.0f, 0.0f);
        //obj.transform.rotation = tr.transform.rotation;
        DropItem temp = obj.GetComponent<DropItem>();

        ItemRand = Random.Range(1, 101);
        temp.itemData.Clear();
        temp.itemData.Add(itemDatas[8]);
        temp.itemData[0].value = Random.Range(minMoney, MaxMoney + 1);
        temp.itemData.Add(itemDatas[index]);
        temp.itemData.Add(itemDatas[4]);

        if (ItemRand <= 20)
            temp.itemData.Add(itemDatas[Random.Range(0, 2)]);
        else if (ItemRand >= 95)
            temp.itemData.Add(itemDatas[Random.Range(2, 5)]);
        
            return obj;

    }





}




