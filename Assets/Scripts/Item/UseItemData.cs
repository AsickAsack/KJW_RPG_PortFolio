using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "������ ������",fileName = "ItemData",order = int.MinValue +1)]
public class UseItemData : ItemData
{
    public override void UseItem(int ItemCode)
    {
        switch(ItemCode)
        {
            //hp����
            case 0:
                break;

            //mp����
            case 1:
                break;

        }
        
    }

    
}
