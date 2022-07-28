using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "아이템 데이터",fileName = "ItemData",order = int.MinValue +1)]
public class UseItemData : ItemData
{
    public override void UseItem(int ItemCode)
    {
        UIManager.Instance.PotionConsume?.Invoke();
        switch (ItemCode)
        {
            //hp포션
            case 0:
                
                GameData.Instance.playerdata.CurHP += this.value;
                UIManager.Instance.SetHP();

                break;

            //mp포션
            case 1:
                GameData.Instance.playerdata.CurMP += this.value;
                UIManager.Instance.SetMp();
                break;

        }

        SoundManager.Instance.PlayEffect1Shot(12);
    }

    
}
