using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    #region 싱글톤

    private static SoundManager _instance = null;

    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SoundManager>();
                if (_instance == null)
                {
                    GameObject obj = Instantiate(Resources.Load("SoundManager")) as GameObject;
                    //obj.name = "SoundManager";
                    _instance = obj.GetComponent<SoundManager>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }

    #endregion

    #region 오디오클립들

    public AudioClip[] myBgmClip;
    public AudioClip[] myEffectClip;

    #endregion

    private void Start()
    {
        //SelectBGM();
    }

    #region BGM 사운드 기능

    private AudioSource _BgmAudio = null;

    public AudioSource BgmAudio
    {
        get
        {
            //항상 카메라에 달린 오디오 소스를 가져옴
            if(_BgmAudio == null)
            _BgmAudio = Camera.main.GetComponent<AudioSource>();

            return _BgmAudio;
        }
    }

    private float _BgmVolum = 0.5f;

    //BgmVolume을 수정하면 BGM오디오 소스 볼륨이 수정되도록
    public float BgmVolume
    {
        get => _BgmVolum;
        set
        {
            _BgmVolum = value;
            BgmAudio.volume = _BgmVolum;
        }
    }

    public AudioSource mainEffectSource;

    public void SetBGM(AudioClip clip , bool loop = false)
    {
        BgmAudio.clip = clip;
        BgmAudio.loop = loop;
        BgmAudio.Play();
    }

    public void SelectBGM()
    {

    }

    #endregion


    #region 이펙트 오디오소스 기능들

    //몬스터,캐릭터,NPC등 이펙트 오디오소스를 관리할 리스트
    public List<AudioSource> EffectSource = new List<AudioSource>();
        
    private float _EffectVolume = 0.5f;

    //이 프로퍼티로 접근하여 수정 하면 모든 이펙트오디오 소스들 볼륨이 수정됨
    public float EffectVolume
    {
        get => _EffectVolume;
        set
        {
            _EffectVolume = value;
            ChangeEffectVolume(_EffectVolume);
        }
    }

    //이펙트 오디오 소스를 리스트에 넣어줄 함수
    public void AddEffectSource(AudioSource myAudio)
    {
        EffectSource.Add(myAudio);
        EffectSource[EffectSource.Count-1].volume = _EffectVolume;
    }

         
    //이펙트 오디오 소스를 리스트에서 삭제시킬 함수
    public void DeleteEffectSource(AudioSource myAudio) => EffectSource.Remove(myAudio);

    //씬을 이동하거나 할 때 클리어 시킬 함수
    public void ClearEffectSource() => EffectSource.Clear();

    //일괄적으로 리스트에 있는 이펙트소스 볼륨을 바꾸는 함수
    public void ChangeEffectVolume(float V)
    {
        foreach(AudioSource Audio in EffectSource)
        {
            Audio.volume = V;
        }
    }

    public void PlayEffect1Shot(int index)
    {
        mainEffectSource.PlayOneShot(myEffectClip[index]);
    }

    #endregion
}
