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
                TipText.text = "���ΰ��� ������ �ձ��� ���翴���ϴ�.";
                break;
            case 1:
                TipText.text = "�ձ��� �ֹε��� ��κ��� �����Դϴ�.";
                break;
            case 2:
                TipText.text = "�ձ����� ���ʸ� �����ϴ� ���ʺ��� �ֽ��ϴ�.";
                break;
            case 3:
                TipText.text = "������� ������ ��� �Խ��ϴ�.";
                break;

        }

    }

    private void Update()
    {
        LoadingCircle.transform.Rotate(-Vector3.forward * Time.deltaTime * 75.0f);
    }

}
