using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFall : MonoBehaviour
{
    public Knight player;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            player.JumpDistance = 0.0f;

            if (GameData.Instance.playerdata.CurHP > 10.0f)
            {
                GameData.Instance.playerdata.CurHP -= 10.0f;
                UIManager.Instance.SetHP();
                if(player.transform.position.z > 45.0f)
                    player.LoadPos(true);
                else
                    player.LoadPos(false);
            }
            else
                player.ChangeState(Knight.State.Death);


        }
    }
}
