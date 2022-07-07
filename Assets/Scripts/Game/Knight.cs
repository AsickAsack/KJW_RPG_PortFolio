using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Player
{
    [Header("[조이스틱 움직임]")]
    public JoySticPanel myJoystic;
    Vector3 Dir = Vector3.zero;
    public GameObject MyChar = null;
    Quaternion MyCharRotate = Quaternion.identity;

    [Header("[애니메이션 관련]")]

    public Transform RelaxSword;
    public Transform AttackSword;
    public Transform AttackSpot;
    public GameObject HitEffect;
    Collider[] myCol;
    Quaternion MyCamRot = Quaternion.identity;
    Vector3 myDirecTion;

    private void Update()
    {
        if (myJoystic.MoveOn && myJoystic.Dir != Vector3.zero && !myAnim.GetBool("IsAttack")) {

            MyCamRot = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);

            myDirecTion = MyCamRot * new Vector3(myJoystic.Dir.x,0.0f, myJoystic.Dir.z);

            MyCharRotate = Quaternion.LookRotation((this.transform.position + myDirecTion) - this.transform.position);

            MyChar.transform.rotation = Quaternion.Slerp(MyChar.transform.rotation, MyCharRotate, Time.deltaTime * 15.0f);
        }

    }

    private void FixedUpdate()
    {
        if (myJoystic.MoveOn && !myAnim.GetBool("IsAttack"))
        {
            myAnim.SetBool("IsWalk", true);
            myRigid.MovePosition(this.transform.position + myDirecTion * Time.deltaTime * 4.0f);
        }
        else
            myAnim.SetBool("IsWalk", false);
    }

    public void AttackButton()
    {
        myAnim.SetTrigger("Attack");
    }

    public void HitCheck()
    {
        
    }


    public void UpperHitCheck()
    {
        //HitEffect.gameObject.SetActive(true);
         myCol = Physics.OverlapSphere(AttackSpot.position, 1.0f,1<<LayerMask.NameToLayer("Water"));
        if(myCol.Length != 0)
        { 
            for(int i=0;i<myCol.Length;i++)
            {
                myCol[i].GetComponent<Rigidbody>().AddForce(this.transform.up * 12.5f,ForceMode.VelocityChange);
            }
            myAnim.SetBool("IsHit", true);
            myAnim.SetTrigger("Attack");
        }

        myCol = null;
    }

    public void JumpAttack()
    {
        this.GetComponent<Rigidbody>().AddForce(this.transform.up * 10.0f, ForceMode.VelocityChange);
        myAnim.SetBool("IsHit", false);

    }
    
    public void JumpAttackCheck()
    {
        //HitEffect.gameObject.SetActive(true);
        myCol = Physics.OverlapSphere(AttackSpot.position, 1.0f, 1 << LayerMask.NameToLayer("Water"));
        if (myCol != null)
        {
            for (int i = 0; i < myCol.Length; i++)
            {
                myCol[i].GetComponent<Rigidbody>().AddForce((myDirecTion + (-this.transform.up)).normalized * 30.0f, ForceMode.VelocityChange);
            }
        }
        myCol = null;
        //HitEffect.gameObject.SetActive(false);
    }
    


    public void FootSound()
    {

    }
}
