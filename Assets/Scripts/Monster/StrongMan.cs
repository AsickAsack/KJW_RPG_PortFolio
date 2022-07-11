using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongMan : Soldier
{

    public override void ChangeState(S_State s)
    {
        if (myState.Equals(s)) return;

        myState = s;

        switch (myState)
        {
            case S_State.Patrol:
                myAnim.SetTrigger("GoIdle");
                break;

            case S_State.Battle:
                myAnim.SetTrigger("GoBattle");
                StartCoroutine(GoBattle(1.7f));
                break;
            case S_State.OnAir:
                Time.timeScale = 0.5f;
                break;
            case S_State.Stun:

                Move = false;
                if (StunCo == null)
                    StunCo = StartCoroutine(Stun(3.0f));
                else
                {
                    StopCoroutine(StunCo);
                    StunCo = StartCoroutine(Stun(3.0f));
                }

                break;
            case S_State.Death:
                Move = false;
                break;

        }
    }
}