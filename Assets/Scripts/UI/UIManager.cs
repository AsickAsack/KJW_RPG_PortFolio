using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Slider HPbar;
    public Slider MPbar;
    public Slider EXPbar;
    public Text HPText;
    public Text MPText;
    public Text EXPText;
    public Text LevelText;

    [Header("[스텟 창 기능]")]

    public Text StatLeveltx;
    public Text StatATKtx;
    public Text StatDeftx;
    public Text StatSpeedTx;
    public Text StatPointTx;
    public GameObject[] Arrow;

    public RectTransform[] NotifyPanel;
    public Text[] NotifyText;
    bool Notflag = true;
    int Notindex = 0;

    [Header("[스텟 창 기능]")]

    public UnityAction[] ButtonFunc = new UnityAction[4];
    public Sprite[] ButtonImage = new Sprite[4];
    public UnityAction PickUp;
    public UnityAction PotionConsume;

    [Header("[워프UI 기능]")]

    public Image WarpImage;
    public GameObject WarpNotouch;
    public Transform WarpPos;
    public GameObject WarpEffect;
    GameObject WarpDoor;
    public Vector3 OrgPos = Vector3.zero;

    [Header("[종료 기능]")]

    public GameObject ExitPopup;

    [Header("[아이템]")]
    public Sprite[] ItemIcon;




    private void Awake()
    {
        Debug.Log(GameData.Instance.playerdata.Helmet);
        Instance = this;
        GameData.Instance.NotAction += Notify;

        SetHP();
        SetMp();
        SetExp();
        SetLevel();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            WarpNotouch.SetActive(true);
            ExitPopup.SetActive(true);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }


    // 점프,블락,체인지,어택..
    public void GetButtonFunc(UnityAction func1, UnityAction func2, UnityAction func3, UnityAction func4)
    {
        ButtonFunc[0] += func1;
        ButtonFunc[1] += func2;
        ButtonFunc[2] += func3;
        ButtonFunc[3] += func4;
    }

    #region 스텟창 기능들

    //스텟 텍스트로찍기
    public void SetStatPopup()
    {
        StatLeveltx.text = GameData.Instance.playerdata.Level.ToString() + " LEVEL";
        StatATKtx.text = GameData.Instance.playerdata.ATK.ToString();
        StatDeftx.text = GameData.Instance.playerdata.DEF.ToString();
        if (GameData.Instance.playerdata.Shoes != null)
            StatSpeedTx.text = (GameData.Instance.playerdata.StatSpeed+ GameData.Instance.playerdata.Shoes.itemData.value).ToString();
        else
            StatSpeedTx.text = GameData.Instance.playerdata.StatSpeed.ToString();

        StatPointTx.text = GameData.Instance.playerdata.StatPoint.ToString();

        if (GameData.Instance.playerdata.StatPoint > 0)
        {
            for(int i=0;i<Arrow.Length;i++)
            Arrow[i].SetActive(true);
        }
    }

    //스텟 올렸을때
    public void UpStatPoint(int index)
    {
        SoundManager.Instance.PlayEffect1Shot(10);

        switch (index)
        {
            //공격력 찍었을때
            case 0:
                GameData.Instance.playerdata._ATK += 1;
                StatATKtx.text = GameData.Instance.playerdata.ATK.ToString();
                break;

                //방어력 찍었을때
            case 1:
                GameData.Instance.playerdata._DEF += 1;
                StatDeftx.text = GameData.Instance.playerdata.DEF.ToString();
                break;
            case 2:
                GameData.Instance.playerdata._MoveSpeed += 0.1f;
                GameData.Instance.playerdata.StatSpeed++;
                StatSpeedTx.text = GameData.Instance.playerdata.StatSpeed.ToString();
                break;
        
        }

        GameData.Instance.playerdata.StatPoint--;

        if (GameData.Instance.playerdata.StatPoint == 0)
        {
            for (int i = 0; i < Arrow.Length; i++)
                Arrow[i].SetActive(false);
        }

        StatPointTx.text = GameData.Instance.playerdata.StatPoint.ToString();

    }

    #endregion

 

    #region 기본  UI 텍스트들

    public void SetAll()
    {
        SetHP();
        SetMp();
        SetExp();
        SetLevel();
    }


    public void SetHP()
    {
        HPbar.value = GameData.Instance.playerdata.CurHP / GameData.Instance.playerdata.MaxHP;
        HPText.text = GameData.Instance.playerdata.CurHP + " / " + GameData.Instance.playerdata.MaxHP;
    }

    public void SetMp()
    {
        MPbar.value = GameData.Instance.playerdata.CurMP / GameData.Instance.playerdata.MaxMP;
        MPText.text = GameData.Instance.playerdata.CurMP + " / " + GameData.Instance.playerdata.MaxMP;
    }

    public void SetExp()
    {
        EXPbar.value = GameData.Instance.playerdata.CurEXP / GameData.Instance.playerdata.MaxEXP;
        EXPText.text = GameData.Instance.playerdata.CurEXP + " / " + GameData.Instance.playerdata.MaxEXP;
    }

    public void SetLevel()
    {
        LevelText.text = "Lv." + GameData.Instance.playerdata.Level + "\nKnight";
    }

    #endregion

    #region 알림기능

    public void Notify()
    {
        if(Notflag)
        {
           Notflag = false;
           StartCoroutine(MoveNoitfy(Notindex++ % 2));
        }
    }

    IEnumerator MoveNoitfy(int index)
    {
        NotifyPanel[index].gameObject.SetActive(true);
        NotifyText[index].text = GameData.Instance.EventString.Dequeue();
        while (NotifyPanel[index].anchoredPosition.y > -50)
        {
            NotifyPanel[index].anchoredPosition += Vector2.down * Time.deltaTime * 200.0f;
            yield return null;
        }
        while (NotifyPanel[index].anchoredPosition.y < 60)
        {
            NotifyPanel[index].anchoredPosition += Vector2.up * Time.deltaTime * 100.0f;
            yield return null;
        }
        NotifyPanel[index].gameObject.SetActive(false);
        Notflag = true;
    }

    #endregion

    public void WellRecovery()
    {
        GameData.Instance.playerdata.CurHP = GameData.Instance.playerdata.MaxHP;
        SetHP();
    }


    public void WarpUI(Transform tr)
    {
        WarpNotouch.SetActive(true);
        WarpEffect.SetActive(true);
        StartCoroutine(WaitWarp(tr));
        WarpDoor = ObjectPool.Instance.Effects[0].Get();
        WarpDoor.transform.position = WarpPos.transform.position + new Vector3(0, 0.5f, 2.0f);
        OrgPos = tr.transform.position;
    }

    IEnumerator WaitWarp(Transform tr)
    {
        yield return new WaitForSeconds(1.5f);
        WarpImage.gameObject.SetActive(true);

        while (WarpImage.fillAmount < 1.0f)
        {
            WarpImage.fillAmount += Time.deltaTime;

            yield return null;
        }

        tr.transform.position = WarpPos.transform.position;

        while(WarpImage.fillAmount > 0.0f)
        {
            WarpImage.fillAmount -= Time.deltaTime;

            yield return null;
        }

        WarpImage.gameObject.SetActive(false);
        WarpNotouch.SetActive(false);
        WarpEffect.SetActive(false);
        WarpImage.fillAmount = 0.0f;
    }

}
