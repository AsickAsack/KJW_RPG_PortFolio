using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant_Manager : MonoBehaviour
{
    SellSlot[] sellslot;
    public UnityEngine.UI.Text GoldText;


    private void Awake()
    {
        sellslot = this.GetComponentsInChildren<SellSlot>();
    }

    public void OpenShop()
    {
        GoldText.text = GameData.Instance.playerdata.money.ToString("N0")+" °ñµå";

        for (int i = 0; i < sellslot.Length; i++)
        {
            sellslot[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < GameData.Instance.playerdata.myItems.Count; i++)
        {
            if (!sellslot[i].gameObject.activeSelf)
            {
                sellslot[i].gameObject.SetActive(true);
                sellslot[i].item = GameData.Instance.playerdata.myItems[i];
                sellslot[i].SetItem();
            }

        }
    }

}
