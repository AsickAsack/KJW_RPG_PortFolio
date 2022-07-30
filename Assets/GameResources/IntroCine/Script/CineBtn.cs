using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineBtn : MonoBehaviour
{
    public void GameStart()
    {
        SceneLoader.Instance.Loading_LoadScene(1);
    }
}
