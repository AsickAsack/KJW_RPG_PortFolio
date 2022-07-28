using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemSlotManager : MonoBehaviour
{
    public DropItem Dropitem;
    public DropItemSlot[] dropItemSlots;
    public GameObject DropItemPanel;
    private string Not;


    public void ExitPopUp()
    {
        for(int i=0;i< dropItemSlots.Length;i++)
        {
            dropItemSlots[i].gameObject.SetActive(false);
        }
    }


    // ÷�� ��������
    public void openDropItem()
    {
        SoundManager.Instance.PlayEffect1Shot(10);
        DropItemPanel.SetActive(true);

        for(int i=0;i<Dropitem.itemData.Count;i++)
        {
            if (!dropItemSlots[i].gameObject.activeSelf)
            {
                dropItemSlots[i].ItemNameText.text = Dropitem.itemData[i].myType == ItemType.Gold ? Dropitem.itemData[i].value.ToString() + " ���" : Dropitem.itemData[i].ItemName;
                dropItemSlots[i].Icon.sprite = UIManager.Instance.ItemIcon[Dropitem.itemData[i].ItemCode];
                dropItemSlots[i].gameObject.SetActive(true);
            }
        }
    }

    //���� ȹ�� ������
    public void AllGetItem()
    {


        if(Dropitem.itemData.Count+GameData.Instance.playerdata.myItems.Count > 12)
        {
            GameData.Instance.SetNotify("�κ��丮�� ��á���ϴ�.");
            SoundManager.Instance.PlayEffect1Shot(11);

            return;
        }


        for(int i=0;i<Dropitem.itemData.Count;i++)
        {
            
            if (Dropitem.itemData[i].myType == ItemType.Equip)
                GameData.Instance.playerdata.myItems.Add(new Item(Dropitem.itemData[i]));
            else if (Dropitem.itemData[i].myType == ItemType.Gold)
            {
                GameData.Instance.playerdata.money += (int)Dropitem.itemData[i].value;
            }
            else
            {
                Item temp = GameData.Instance.playerdata.myItems.Find(x => x.itemData == Dropitem.itemData[i]);
                if (temp == null)
                    GameData.Instance.playerdata.myItems.Add(new Item(Dropitem.itemData[i]));
                else
                    temp.ItemCount++;
            }

            UIManager.Instance.PickUp?.Invoke();
            Not = Dropitem.itemData[i].myType == ItemType.Gold ? Dropitem.itemData[i].value.ToString() + " ��带 ȹ���߽��ϴ�." : Dropitem.itemData[i].ItemName + " 1���� ȹ���߽��ϴ�.";
            GameData.Instance.SetNotify(Not);

        }
        SoundManager.Instance.PlayEffect1Shot(16);
        Dropitem.itemData.Clear();
    }


    //�ϳ��� ��������
    public void ClickItem(int index)
    {
   

        if (GameData.Instance.playerdata.myItems.Count+1 > 12)
        {
            GameData.Instance.SetNotify("�κ��丮�� ��á���ϴ�.");
            SoundManager.Instance.PlayEffect1Shot(11);

            return;
        }


        if (Dropitem.itemData[index] != null)
        {

            if (Dropitem.itemData[index].myType == ItemType.Equip)
                GameData.Instance.playerdata.myItems.Add(new Item(Dropitem.itemData[index]));
            else if (Dropitem.itemData[index].myType == ItemType.Gold)
            {
                GameData.Instance.playerdata.money += (int)Dropitem.itemData[index].value;
            }
            else
            {
                Item temp = GameData.Instance.playerdata.myItems.Find(x => x.itemData == Dropitem.itemData[index]);
                if (temp == null)
                    GameData.Instance.playerdata.myItems.Add(new Item(Dropitem.itemData[index]));
                else
                    temp.ItemCount++;
            }

            UIManager.Instance.PickUp?.Invoke();
            SoundManager.Instance.PlayEffect1Shot(16);
            Not = Dropitem.itemData[index].myType == ItemType.Gold ? Dropitem.itemData[index].value.ToString() + " ��带 ȹ���߽��ϴ�." : Dropitem.itemData[index].ItemName + "1���� ȹ���߽��ϴ�.";
            GameData.Instance.SetNotify(Not);
            Dropitem.itemData.RemoveAt(index);
            ExitPopUp();
            openDropItem();
        }
    }
}
