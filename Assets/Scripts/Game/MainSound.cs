using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSound : MonoBehaviour
{
    public void PlayEffects(int index)
    {
        SoundManager.Instance.PlayEffect1Shot(index);
    }

}
