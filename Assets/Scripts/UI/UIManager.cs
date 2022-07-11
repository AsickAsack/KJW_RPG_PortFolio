using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Awake()
    {
        Instance = this;
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


}
