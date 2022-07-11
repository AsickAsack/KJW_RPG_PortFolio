using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{

    public static ObjectPool Instance;

    public ObjectPool<GameObject>[] ObjectManager = new ObjectPool<GameObject>[5];
    public ItemData[] itemDatas;
    public GameObject DropItem;
    public GameObject SwordMan;
    public GameObject SpearMan;
    public GameObject S_SwordMan;
    public GameObject Damage_text;

    public Transform[] folder;


    private void Awake()
    {

        Instance = this;

        Init(0, DropItem);
        Init(1, SwordMan);
        Init(2, SpearMan);
        Init(3, S_SwordMan);
        Init(4, Damage_text);


    }

    void Init(int index ,GameObject prefab)
    {
        ObjectManager[index] = new ObjectPool<GameObject>(
        //���ο� ������Ʈ�� �����Ҷ�
        createFunc: () =>
        {
            var CreateObj = Instantiate(prefab,folder[index]);
            return CreateObj;
        },
        //��������
        actionOnGet: (Obj) =>
        {
            Obj.gameObject.SetActive(true);
        },
        // �ٽ� ������ �ö�
        actionOnRelease: (Obj) =>
        {  
            Obj.gameObject.SetActive(false);
        },
        //Ÿ�� ������Ʈ���� ������ �Լ�
        actionOnDestroy: (Obj) =>
        {
            Destroy(Obj.gameObject);
        }, maxSize: 5);
    }

    public GameObject GetItem(int index,Transform tr)
    {
        GameObject obj = ObjectManager[0].Get();
        obj.transform.position = tr.transform.position;
        obj.transform.rotation = tr.transform.rotation;
        DropItem temp = obj.GetComponent<DropItem>();
        temp.itemData = itemDatas[index];

        return obj;

    }




    
}




