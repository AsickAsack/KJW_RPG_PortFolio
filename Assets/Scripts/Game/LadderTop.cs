using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LadderTop : MonoBehaviour
{
    Knight player;
    public Button LadderTopBtn;
    public GameObject TargetPos;
    float ExitTime = 0.0f;

    private void Awake()
    {
        LadderTopBtn.onClick.AddListener(() => { GoUpLadder(); });
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (player.myAnim.GetInteger("LadderIndex") == 1)
            {
                player.myAnim.SetInteger("LadderIndex", 3);
                player.myAnim.SetBool("IsLadder", false);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log(other.transform);
            if (other.GetComponent<Knight>() != null)
            player = other.gameObject.GetComponent<Knight>();

            //타고 올라올때라면
            if (player.myAnim.GetBool("IsLadder") && player.myAnim.GetInteger("LadderIndex")==1)
            {
                player.myAnim.SetInteger("LadderIndex", 3);
                player.myAnim.SetBool("IsLadder", false);

            }
            else if(!player.myAnim.GetBool("IsLadder"))
            {
                GoUpLadder();
                player.myAnim.SetBool("IsWalk", false);
                //LadderTopBtn.gameObject.SetActive(true);

            }
        }
    }

    public void GoUpLadder()
    {
        ExitTime = 0.0f;
        LadderTopBtn.gameObject.SetActive(false);

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
        


        player.myAnim.SetTrigger("GoLadderDown");
        while (ExitTime < 0.75f)
        {
            
            ExitTime += Time.deltaTime;
            player.transform.position = Vector3.Lerp(player.transform.position, TargetPos.transform.position, Time.deltaTime * 5.0f);
            player.MyChar.transform.rotation = Quaternion.Slerp(player.MyChar.transform.rotation, this.transform.rotation, Time.deltaTime * 5.0f);



            yield return null;
        }
        
        player.myAnim.SetBool("LadderChange", false);
        
        
        yield return new WaitForSeconds(1.0f);
        player.ChangeState(Knight.State.Ladder);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
          
                if (LadderTopBtn.gameObject.activeSelf)
                    LadderTopBtn.gameObject.SetActive(false);

        }
    }

    

    IEnumerator GOladder(Transform tr)
    {
        Knight temp = tr.GetComponent<Knight>();

        temp.MyChar.transform.rotation = Quaternion.identity;
        tr.position += new Vector3(0, 0, -0.25f);

        temp.myAnim.SetBool("IsLadder", true);
        temp.myAnim.SetTrigger("GoLadder");
        temp.ChangeState(Knight.State.Ladder);

        yield return null;
    }
}
