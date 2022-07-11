using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "아이템 데이터",fileName = "ItemData",order = int.MinValue +1)]
public class UseItemData : ItemData
{
    public override void UseItem(int ItemCode)
    {
        switch(ItemCode)
        {
            //hp포션
            case 0:
                break;

            //mp포션
            case 1:
                break;

        }
        
    }

    
}
