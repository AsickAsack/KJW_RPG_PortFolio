using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using System.IO;
using System;

[Serializable]
public class GameData : MonoBehaviour
{

    #region �̱���
    private static GameData _instance = null;

    public static GameData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameData>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "GameData";
                    _instance = obj.AddComponent<GameData>();
                    DontDestroyOnLoad(obj);
                    

                }
            }
            return _instance;
        }

    }
    #endregion

    public PlayerData playerdata = new PlayerData();
    public PlayerData playerdata2 = new PlayerData();
    public Queue<string> EventString = new Queue<string>();
    public UnityAction NotAction;
    private Coroutine Notify;
    TimeSpan CirPlayTime = new TimeSpan();
    public bool IsPlay = false;

    public void SetNotify(string not)
    {
        EventString.Enqueue(not);

        if (Notify == null)
            Notify = StartCoroutine(NotifyRoutine());
        

    }

    IEnumerator NotifyRoutine()
    {
        while(EventString.Count > 0)
        {
            NotAction?.Invoke();

            yield return null;
        }

        Notify = null;
    }

    public void Save()
    {
        SetSavePos();
        SetSaveTime();
        string data = JsonConvert.SerializeObject(playerdata);
        File.WriteAllText(Application.dataPath +"/GameData.json", data);
        Debug.Log(playerdata.PlayTime);
    }

    public void SetSavePos()
    {
        playerdata.SavePos[0] = MonsterSpawnManager.Instance.Player.position.x;
        playerdata.SavePos[1] = MonsterSpawnManager.Instance.Player.position.y;
        playerdata.SavePos[2] = MonsterSpawnManager.Instance.Player.position.z;
    }

    public void SetSaveTime()
    {
        CirPlayTime = DateTime.Now - playerdata.FirstTime;
        playerdata.PlayTime = CirPlayTime.Hours.ToString() + " �ð� " + CirPlayTime.Minutes.ToString() + " ��";
    }

    public void Load()
    {
        string data = File.ReadAllText(Application.dataPath + "/GameData.json");
        playerdata = JsonConvert.DeserializeObject<PlayerData>(data);
    }

    public void CheckDataLoad()
    {
        string data = File.ReadAllText(Application.dataPath + "/GameData.json");
        playerdata2 = JsonConvert.DeserializeObject<PlayerData>(data);
    }


    [System.Serializable]
    public class PlayerData
    {
        public enum Difficulty
        {
            Normal = 0, Hard, Insane
        }

        public Difficulty difficulty = Difficulty.Normal;
        public List<Item> myItems = new List<Item>();
        public Item Helmet = null;
        public Item Weapon = null;
        public Item Shoes = null;
        public Item[] QuickSlot = new Item[2];

        public float[] SavePos = new float[3];
        public DateTime FirstTime;
        public string PlayTime;

        #region �÷��̾� ����

        private int _Level = 1;
        public int Level
        {
            get => _Level;
            set
            {

                if (value != _Level)
                {
                    StatPoint += 3;
                    SkillPoint += 3;
                    MaxHP += 50;
                    CurHP = MaxHP;
                    MaxMP += 50;
                    CurMP = MaxMP;          
                }

                _Level = value;

                if (GameData.Instance.IsPlay)
                {
                    MaxEXP = Level * 100;
                    UIManager.Instance.SetAll();
                    GameData.Instance.EventString.Enqueue("���� " + Level + "�� �Ǿ����ϴ�!");
                }

            }

        }
        public int StatPoint = 0;
        public int SkillPoint = 1;
        public int _ATK = 50;
        public int ATK
        {
            get
            {
                if (Weapon != null)
                    return _ATK + (int)Weapon.itemData.value;
                else
                    return _ATK;
            }

            set => _ATK = value;
        }

        public int _DEF = 10;
        public int DEF
        {
            get
            {
                if (Helmet != null)
                    return _DEF + (int)Helmet.itemData.value;
                else
                    return _DEF;
            }

            set => _DEF = value;
        }
        public float _MoveSpeed = 3;
        public float MoveSpeed
        {
            get
            {
                if (Shoes != null)
                    return _MoveSpeed + (int)Shoes.itemData.value;
                else
                    return _MoveSpeed;
            }

            set => _MoveSpeed = value;
        }
        //�����ӿ� ����
        public float StatSpeed = 3;


        private float _CurHP = 200;
        public float CurHP
        {
            get => _CurHP;
            set {
                if (value > MaxHP)
                    _CurHP = MaxHP;
                else
                    _CurHP = value;
            }
        }
        private float _MaxHP = 200;
        public float MaxHP
        {
            get
            {
                if (Helmet != null)
                    return _MaxHP + (int)Helmet.itemData.value + 50;
                else
                    return _MaxHP;
            }
            set => _MaxHP = value;
        }
        private float _CurMP = 100;
        public float CurMP
        {
            get => _CurMP;
            set
            {
                if (value > _MaxMP)
                    _CurMP = _MaxMP;
                else
                    _CurMP = value;
            }
        }
        private float _MaxMP = 100;
        public float MaxMP
        {
            get => _MaxMP;
            set => _MaxMP = value;
        }

        private float _MaxSP = 100;
        public float MaxSP
        {
            get => _MaxSP;
            set => _MaxSP = value;
        }
        private float _CurEXP = 0;
        public float CurEXP
        {
            get => _CurEXP;
            set
            {
                _CurEXP = value;
                if (_CurEXP >= _MaxEXP)
                {
                    float temp = _CurEXP - _MaxEXP;
                    _CurEXP = temp;
                    Level += 1;
                 }
            }
        }
        private float _MaxEXP = 100;
        public float MaxEXP
        {
            get => _MaxEXP;
            set => _MaxEXP = value;
        }

        public int money = 0;
        public int StoryIndex = 0;
        public bool Quest = false;


        #endregion


        public float BgmVolume = 0.5f;
        public float EffectVolume = 0.5f;


    }


}

