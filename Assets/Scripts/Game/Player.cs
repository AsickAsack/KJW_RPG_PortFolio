using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BattleSystem
{
    public bool OnDamage(int index);
}

public class Player : MonoBehaviour
{
    private Rigidbody _myRigid;
    public Rigidbody myRigid
    {
        get
        {
            _myRigid = GetComponent<Rigidbody>();
            return _myRigid;
        }
    }


    private Animator _myAnim;
    public Animator myAnim
    {
        get
        {
            _myAnim = GetComponent<Animator>();
            return _myAnim;
        }
    }

}
