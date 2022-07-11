using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using System.IO;

[System.Serializable]
public class GameData : MonoBehaviour
{

    #region 싱글톤
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
    public Queue<string> EventString = new Queue<string>();


    public void Save(object SaveData,string SaveFileName)
    {
        string data = JsonConvert.SerializeObject(SaveData);
        File.WriteAllText(Application.persistentDataPath + SaveFileName, data);
    }

    public void Load(string LoadFileName)
    {
        string data = File.ReadAllText(Application.persistentDataPath + LoadFileName);
        playerdata = JsonConvert.DeserializeObject<PlayerData>(data);
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


        #region 플레이어 스텟

        private int _Level = 1;
        public int Level
        {
            get => _Level;
            set
            {

                if(value != _Level)
                {
                    StatPoint += 3;
                    SkillPoint += 3;
                }

                _Level = value;

                GameData.Instance.EventString.Enqueue("레벨 " + Level + "이 되었습니다!");
                //맥스 HP,MP,SP업
            }

        }
        public int StatPoint = 0;
        public int SkillPoint = 1;
        private int _ATK = 50;
        public int ATK
        {
            get
            {
                
                return _ATK + ATK_Point;
            }

            set => _ATK = value;
        }

        private int _DEF = 10;
        public int DEF
        {
            get
            {
                
                return _DEF + DEF_Point;
            }

            set => _DEF = value;
        }
        private float _MoveSpeed = 3;
        public float MoveSpeed
        {
            get
            {
                
                return _MoveSpeed+(float)(Speed_Point * 0.05f);
            }

            set => _MoveSpeed = value;
        }
        //눈속임용 스텟
        public float StatSpeed
        {
            get
            {
                return _MoveSpeed + Speed_Point;
            }
            private set => _MoveSpeed = value;
        }



        public int ATK_Point = 0;
        public int DEF_Point = 0;
        public int Speed_Point = 0;


        private float _CurHP = 200;
        public float CurHP
        {
            get => _CurHP;
            set => _CurHP = value;
        }
        private float _MaxHP = 200;
        public float MaxHP
        {
            get => _MaxHP;
            set => _MaxHP = value;
        }
        private float _CurMP = 100;
        public float CurMP
        {
            get => _CurMP;
            set => _CurMP = value;
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
        private float _CurEXP = 50;
        public float CurEXP
        {
            get => _CurEXP;
            set
            {
                _CurEXP = value;
                if (_CurEXP > _MaxEXP)
                {
                    float temp = _CurEXP - _MaxEXP;
                    _CurEXP = temp;
                    Level += 1;
                    // 최대 EXP어떻게할지 _MaxEXP = 
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



        #endregion


        public float BgmVolume = 0.5f;
        public float EffectVolume = 0.5f;


    }


}

