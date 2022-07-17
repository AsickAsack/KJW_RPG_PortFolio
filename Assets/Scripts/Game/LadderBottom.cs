using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LadderBottom : MonoBehaviour
{
    Knight player;
    Vector3 TargetPos;
    float ExitTime = 0.0f;

    private void Awake()
    {
        TargetPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z-0.1f);
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log(other.transform);
            if (other.gameObject.GetComponent<Knight>() != null)
                player = other.gameObject.GetComponent<Knight>();

            if (!player.myAnim.GetBool("IsLadder"))
            {

                //LadderBottonBtn.gameObject.SetActive(true);
                player.myAnim.SetBool("IsWalk", false);
                GoUpLadder();

            }
        }
    }

    public void GoUpLadder()
    {
        ExitTime = 0.0f;

        if (player.myState == Knight.State.Battle)
        {
            player.ChangeState(Knight.State.Relax);
            StartCoroutine(LaderProcess(true));
            return;
        }

        StartCoroutine(LaderProcess(false));



    }

    IEnumerator LaderProcess(bool check)
    {
        player.myAnim.SetBool("IsLadder", true);
        player.myAnim.SetBool("LadderChange", true);

        if (check)
        {
            yield return new WaitForSeconds(0.5f);
        }
        player.myAnim.SetTrigger("GoLadder");
        while (ExitTime < 0.75f)
        {
            ExitTime += Time.deltaTime;
            player.transform.position = Vector3.Lerp(player.transform.position, TargetPos, Time.deltaTime * 5.0f);
            player.MyChar.transform.rotation = Quaternion.Slerp(player.MyChar.transform.rotation,this.transform.rotation, Time.deltaTime * 5.0f);

            

            yield return null;
        }
        player.myAnim.SetBool("LadderChange", false);
        player.myAnim.SetBool("IsRWalk", false);
        
        //yield return new WaitForSeconds(1.0f);
        player.ChangeState(Knight.State.Ladder);
    }




}
