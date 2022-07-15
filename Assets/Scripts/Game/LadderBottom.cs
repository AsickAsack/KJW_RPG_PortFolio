using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LadderBottom : MonoBehaviour
{
    public Button LadderBottonBtn;
    Knight player;
    Vector3 TargetPos;
    float ExitTime = 0.0f;

    private void Awake()
    {
        LadderBottonBtn.onClick.AddListener(() => { GoUpLadder(); });
        TargetPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 0.7f);
    }



    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.gameObject.GetComponent<Knight>() != null)
                player = other.gameObject.GetComponent<Knight>();

            if (!player.myAnim.GetBool("IsLadder"))
            {
                
                LadderBottonBtn.gameObject.SetActive(true);


            }
        }
    }

    public void GoUpLadder()
    {
        ExitTime = 0.0f;
        LadderBottonBtn.gameObject.SetActive(false);

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
            player.AnimationRig.weight = 0.0f;
            yield return new WaitForSeconds(0.5f);
        }

        while (ExitTime < 0.75f)
        {
            ExitTime += Time.deltaTime;
            player.transform.position = Vector3.Lerp(player.transform.position, TargetPos, Time.deltaTime * 5.0f);
            player.MyChar.transform.rotation = Quaternion.Slerp(player.MyChar.transform.rotation, Quaternion.identity, Time.deltaTime * 5.0f);

            

            yield return null;
        }
        player.myAnim.SetBool("LadderChange", false);
        player.myAnim.SetBool("IsRWalk", false);
        player.myAnim.SetTrigger("GoLadder");
        yield return new WaitForSeconds(1.0f);
        player.ChangeState(Knight.State.Ladder);
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!player.myAnim.GetBool("IsLadder"))
            {
                if(LadderBottonBtn.gameObject.activeSelf)
                LadderBottonBtn.gameObject.SetActive(false);

            }
        }
    }

}
