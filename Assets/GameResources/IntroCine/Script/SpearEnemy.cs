using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearEnemy : MonoBehaviour
{
    public AudioSource Mysource;
    public AudioClip[] myClips;

    public void PlayEffect1(int index)
    {
        Mysource.PlayOneShot(myClips[index]);
    }
}
