using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{

    public static ObjectPool Instance;

    public ObjectPool<GameObject>[] ObjectManager = new ObjectPool<GameObject>[4];
    public ObjectPool<GameObject> Drop;
    public ObjectPool<GameObject>[] Effects = new ObjectPool<GameObject>[8];
    public UseItemData[] itemDatas;
    public GameObject DropItem;
    public GameObject SwordMan;
    public GameObject SpearMan;
    public GameObject S_SwordMan;
    public GameObject Damage_text;
    public GameObject WarpDoorObject;
    public GameObject FireBall;
    public GameObject HitEffect;
    public GameObject FireSpecial;
    public GameObject P_Common;
    public GameObject P_Fire;
    public GameObject SparkGround;
    public GameObject SparkAir;

    public Transform[] folder;
    int ItemRand;

    private void Awake()
    {

        Instance = this;

        dropInit();
        Init(0, SwordMan);
        Init(1, SpearMan);
        Init(2, S_SwordMan);
        Init(3, Damage_text);
        InitEffect(0, WarpDoorObject, 5, 2);
        InitEffect(1, FireBall, 6, 5);
        InitEffect(2, HitEffect, 7, 10);
        InitEffect(3, FireSpecial, 8, 3);
        InitEffect(4, P_Common, 9, 3);
        InitEffect(5, P_Fire, 10, 3);
        InitEffect(6, SparkGround, 11, 10);
        InitEffect(7, SparkAir, 12, 10);

    }

    void InitEffect(int index,GameObject prefab,int findex,int maxCount)
    {
        Effects[index] = new ObjectPool<GameObject>(
      //새로운 오브젝트를 생성할때
      createFunc: () =>
      {
          var CreateObj = Instantiate(prefab, folder[findex]);
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
      }, maxSize: maxCount);
    }



    void dropInit()
    {
        Drop = new ObjectPool<GameObject>(
        //새로운 오브젝트를 생성할때
        createFunc: () =>
        {
            var CreateObj = Instantiate(DropItem, folder[4]);
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


    void Init(int index, GameObject prefab)
    {
        ObjectManager[index] = new ObjectPool<GameObject>(
        //새로운 오브젝트를 생성할때
        createFunc: () =>
        {
            var CreateObj = Instantiate(prefab, folder[index]);
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

   

    public GameObject GetMonster(int index)
    {
        GameObject Gmonster;
        Gmonster = ObjectManager[index].Get();

     
        return Gmonster;

    }





    public void GetItem(int index, Transform tr, int minMoney, int MaxMoney)
    {
        GameObject DropItem = Drop.Get();
        DropItem temp = DropItem.GetComponent<DropItem>();
        DropItem.transform.position = tr.transform.position + new Vector3(-0.5f, 0.0f, 0.0f);

        temp.itemData.Clear();
        ItemRand = Random.Range(1, 101);
        temp.itemData.Add(itemDatas[8]);
        temp.itemData[0].value = Random.Range(minMoney, MaxMoney + 1);
        temp.itemData.Add(itemDatas[index]);
        temp.itemData.Add(itemDatas[0]);

        if (ItemRand <= 20)
            temp.itemData.Add(itemDatas[Random.Range(0, 2)]);
        else if (ItemRand >= 95)
            temp.itemData.Add(itemDatas[Random.Range(2, 5)]);
    }





}




