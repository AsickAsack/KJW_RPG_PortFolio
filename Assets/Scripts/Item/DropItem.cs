using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropItem : MonoBehaviour 
{
    public List<ItemData> itemData = new List<ItemData>();
    public Rigidbody myRigid;
    public GameObject CheckBtn;
    float disApearTime;

    private void Start()
    {
        //myRigid.AddForce(Vector3.up * 400.0f + Vector3.right*400.0f);
    }


    //60�ʰ� �帣�ų�, �������� ȹ��Ǹ� �ݳ�
    private void Update()
    {
        disApearTime += Time.deltaTime;
        

        if (disApearTime >= 60.0f || itemData.Count == 0)
        {
            disApearTime = 0.0f;
            ObjectPool.Instance.ObjectManager[0].Release(this.gameObject);
        }

    }

    //��ó�� ���� ������ Ȯ�ι�ư �ѱ�
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            disApearTime = 0.0f;
            CheckBtn.SetActive(true);
        }

    }
    
    //������ ������ Ȯ�ι�ư ����
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            CheckBtn.SetActive(false);
        }
    }

    //�ݳ��ɶ� �����۵����� ����Ʈ Ŭ����
    private void OnDisable()
    {
        if(itemData.Count != 0)
        itemData.Clear();
    }


    /*
    private void CheckItem()
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            if (itemData[i].myType == ItemType.Equip)
                GameData.Instance.playerdata.myItems.Add(new Item(itemData[i]));
            else
            {
                Item temp = GameData.Instance.playerdata.myItems.Find(x => x.itemData == this.itemData[i]);
                if (temp == null)
                    GameData.Instance.playerdata.myItems.Add(new Item(itemData[i]));
                else
                    temp.ItemCount++;
            }
        }
    }
    */
}

