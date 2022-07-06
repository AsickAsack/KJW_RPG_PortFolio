using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCtrl : MonoBehaviour
{
    public GameObject oEqupped;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.Translate(Vector3.forward * Time.deltaTime * 10);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.Translate(Vector3.forward * Time.deltaTime * -10);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Translate(Vector3.left * Time.deltaTime * 10);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Translate(Vector3.left * Time.deltaTime * -10);
        }
    }

    public void AddItem(ItemInfo item)
    {
        int nowCnt = Common.myChar.hasItems.Length;

        ItemInfo[] temp = new ItemInfo[nowCnt];
        for(int i=0; i < Common.myChar.hasItems.Length; i++)
        {
            temp[i] = Common.myChar.hasItems[i];
        }

        Common.myChar.hasItems = new ItemInfo[nowCnt + 1];

        for(int i=0; i < Common.myChar.hasItems.Length; i++)
        {
            if(i < temp.Length)
            {
                Common.myChar.hasItems[i] = temp[i];
            }
            else
            {
                Common.myChar.hasItems[i] = item;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag.Equals("item") == true)
        {
            var cude = collision.gameObject.GetComponent<ItemCube>();

            if(cude.info.itemCategory == ItemCategory.Equpped)
            {
                oEqupped.SetActive(true);
            }
            else if (cude.info.itemCategory == ItemCategory.NoneConsume)
            {
                AddItem(cude.info);
            }
            else if (cude.info.itemCategory == ItemCategory.Consume)
            {
                //ToDo Consume;
            }

            collision.gameObject.SetActive(false);

        }
    }
}
