using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
public class OptionManager : MonoBehaviour
{
    public Text PlayTime;
    public Text SaveDetail;
    public GameObject SavePopup;
    public GameObject NoTouchPanel;

    public Slider[] AudioSlider;
    public Text[] SliderV_Text;
   

    private void Awake()
    {
        GameData.Instance.playerdata.FirstTime = System.DateTime.Now;
    }


    public void SaveBtn()
    {
        if (File.Exists(Application.dataPath + "/GameData.json"))
        {
            SavePopup.SetActive(true);
            NoTouchPanel.SetActive(true);
        }
        else
        {
            GameData.Instance.Save();
            OpenOption();
        }
    }

    public void ReAskSave()
    {
        SoundManager.Instance.PlayEffect1Shot(10);
        GameData.Instance.Save();
        OpenOption();
        SavePopup.SetActive(false);
        NoTouchPanel.SetActive(false);
    }



    public void OpenOption()
    {
        

        if (File.Exists(Application.dataPath+"/GameData.json"))
        {
            GameData.Instance.CheckDataLoad();
            PlayTime.gameObject.SetActive(true);
            PlayTime.text = "총 플레이타임  " + GameData.Instance.playerdata2.PlayTime;
            SaveDetail.text = "Level  " + GameData.Instance.playerdata2.Level + "\n보유 골드   " + GameData.Instance.playerdata2.money.ToString("N0") + " 골드";
        }
        else
        {
            PlayTime.gameObject.SetActive(false);
            SaveDetail.text = "저장 파일이 없습니다.";
        }

        SetVSlider();
    }

    public void SetVSlider()
    {
        AudioSlider[0].value = SoundManager.Instance.BgmVolume;
        SliderV_Text[0].text = (SoundManager.Instance.BgmVolume * 100).ToString("N0");
        AudioSlider[1].value = SoundManager.Instance.EffectVolume;
        SliderV_Text[1].text = (SoundManager.Instance.EffectVolume * 100).ToString("N0");
        //버튼 클릭소리
    }

    public void MoveOptionSlider(int index)
    {
        switch (index)
        {
            case 0:
                SoundManager.Instance.BgmVolume = AudioSlider[0].value;
                SliderV_Text[0].text = (SoundManager.Instance.BgmVolume * 100).ToString("N0");
                break;
            case 1:
                SoundManager.Instance.EffectVolume = AudioSlider[1].value;
                SliderV_Text[1].text = (SoundManager.Instance.EffectVolume * 100).ToString("N0");
                break;
        }

    }

    public void ClickSliderBtn(int index)
    {
        SoundManager.Instance.PlayEffect1Shot(10);
        switch (index)
        {
            case 0:
                AudioSlider[0].value -= 0.01f;
                SoundManager.Instance.BgmVolume = AudioSlider[0].value;
                SliderV_Text[0].text = (SoundManager.Instance.BgmVolume * 100).ToString("N0");
                break;
            case 1:
                AudioSlider[0].value += 0.01f;
                SoundManager.Instance.BgmVolume = AudioSlider[0].value;
                SliderV_Text[0].text = (SoundManager.Instance.BgmVolume * 100).ToString("N0");
                break;
            case 2:
                AudioSlider[1].value -= 0.01f;
                SoundManager.Instance.EffectVolume = AudioSlider[1].value;
                SliderV_Text[1].text = (SoundManager.Instance.EffectVolume * 100).ToString("N0");
                break;
            case 3:
                AudioSlider[1].value += 0.01f;
                SoundManager.Instance.EffectVolume = AudioSlider[1].value;
                SliderV_Text[1].text = (SoundManager.Instance.EffectVolume * 100).ToString("N0");
                break;

        }
        //버튼 클릭 소리
    }



}
