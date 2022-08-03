using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class IntroManager : MonoBehaviour
{
    [Header("[옵션]")]
    public Slider[] AudioSlider;
    public Text[] SliderV_Text;

    public Text PlayTime;
    public Text SaveDetail;
    public GameObject SavePopup;

    public GameObject ExitPopup;

    public AudioSource AudioSource;
    public Animator animator;
    public GameObject IntroCanvas;

    private void Awake()
    {
        SoundManager.Instance.BgmAudio.clip = SoundManager.Instance.myBgmClip[0];
        SoundManager.Instance.AddEffectSource(AudioSource);
      
       
    }

    private void Start()
    {
        StartCoroutine(IntroEnd());
    }

    IEnumerator IntroEnd()
    {
        yield return new WaitForSeconds(2.0f);
        IntroCanvas.SetActive(false);
        SoundManager.Instance.BgmAudio.Play();
    }



    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ButtonClick();
            ExitPopup.SetActive(true);
        }
    }

    public void LoadBtn()
    {
        if (File.Exists(Application.persistentDataPath + "/GameData.json"))
        {
            SoundManager.Instance.DeleteEffectSource(AudioSource);
            GameData.Instance.Load();
            GameData.Instance.IsPlay = true;
            SceneLoader.Instance.Loading_LoadScene(1);
        }
        else
        {
            animator.SetTrigger("Shake");
            IsBadSound();
        }
    }

    public void OpenLoadPopup()
    {
        SavePopup.SetActive(true);

        if (File.Exists(Application.persistentDataPath + "/GameData.json"))
        {
            GameData.Instance.CheckDataLoad();
            PlayTime.gameObject.SetActive(true);
            PlayTime.text = "총 플레이타임  "+GameData.Instance.playerdata2.PlayTime;
            SaveDetail.text = "Level  " + GameData.Instance.playerdata2.Level + "\n보유 골드   " + GameData.Instance.playerdata2.money.ToString("N0") + " 골드";
        }
        else
        {
            PlayTime.gameObject.SetActive(false);
            SaveDetail.text = "저장 파일이 없습니다.";
        }

    }

    public void IsBadSound()
    {
        AudioSource.PlayOneShot(SoundManager.Instance.myEffectClip[11]);
    }


    public void ButtonClick()
    {
        AudioSource.PlayOneShot(SoundManager.Instance.myEffectClip[10]);
    }

    public void StartNewGame()
    {
        ButtonClick();
        SoundManager.Instance.DeleteEffectSource(AudioSource);
        GameData.Instance.playerdata.FirstTime = System.DateTime.Now;
        GameData.Instance.IsPlay = true;
        SceneLoader.Instance.Loading_LoadScene(3);
    }

    public void LoadGame(int index)
    {
        ButtonClick();
        SoundManager.Instance.DeleteEffectSource(AudioSource);
        switch (index)
        {
            case 0:
                break;
            case 1:
                break;

        }
    }

    public void OpenOption()
    {
        AudioSlider[0].value = SoundManager.Instance.BgmVolume;
        SliderV_Text[0].text = (SoundManager.Instance.BgmVolume * 100).ToString("N0");
        AudioSlider[1].value = SoundManager.Instance.EffectVolume;
        SliderV_Text[1].text = (SoundManager.Instance.EffectVolume * 100).ToString("N0");
        ButtonClick();
    }

    public void MoveOptionSlider(int index)
    {
        switch(index)
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
        ButtonClick();
    }



    public void ExitGame()
    {
        Application.Quit();
    }
}
