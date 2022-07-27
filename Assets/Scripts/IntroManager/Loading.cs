using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Text LoadingText;
    public Text TipText;
    public Image BackGround;
    public Sprite[] Images;
    int Rand = 0;
    public GameObject LoadingCircle;

    private void Awake()
    {
        Rand = Random.Range(0, 4);

        BackGround.sprite = Images[Rand];

        switch (Rand)
        {
            case 0:
                TipText.text = "주인공도 전에는 왕국의 병사였습니다.";
                break;
            case 1:
                TipText.text = "왕국의 주민들은 대부분이 병사입니다.";
                break;
            case 2:
                TipText.text = "왕국에는 약초를 관리하는 약초병도 있습니다.";
                break;
            case 3:
                TipText.text = "병사들은 생선을 즐겨 먹습니다.";
                break;

        }

    }

    private void Update()
    {
        LoadingCircle.transform.Rotate(-Vector3.forward * Time.deltaTime * 75.0f);
    }

}
