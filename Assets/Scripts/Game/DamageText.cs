using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public TMPro.TMP_Text myText;

    private void Update()
    {
        if(this.gameObject.activeSelf)
        this.transform.position += Vector3.up * Time.deltaTime;
    }


    public void ReleaseText()
    {
        ObjectPool.Instance.ObjectManager[3].Release(this.gameObject);
    }

    public void SetText(Transform tr,string text,int color)
    {
        this.transform.position = tr.transform.position + new Vector3(0, 1.5f, 0.0f);
        this.transform.rotation = Camera.main.transform.rotation;

        switch (color)
        {
            case 0:
                myText.color = Color.red;
                break;
            case 1:
                myText.color = Color.gray;
                break;
            case 2:
                myText.color = Color.yellow;
                break;
        }

        myText.text = text;

    }


    public void SetTextP(Transform tr, string text, int color)
    {
        this.transform.position = tr.transform.position + new Vector3(0, 2.0f, 0.0f);
        this.transform.rotation = Camera.main.transform.rotation;

        switch (color)
        {
            case 0:
                myText.color = Color.red;
                break;
            case 1:
                myText.color = Color.gray;
                break;
            case 2:
                myText.color = Color.yellow;
                break;

        }

        myText.text = text;

    }

}
