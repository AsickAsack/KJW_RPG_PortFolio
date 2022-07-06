using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using JsonU


public class Intro : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string ppChar = PlayerPrefs.GetString("ppChar", string.Empty);

        if( string.IsNullOrEmpty(ppChar) == true)
        {
            Common.myChar = new CharInfo()
            {
                cid = "0000",
                Name = "Taek",
                curr_lev = 1,
                next_lev = 2,
                exp_now = 0,
                exp_max = 10,
                status = new StatusInfo()
                {
                    STR = 10,
                    DEX = 5,
                    CON = 100
                },
                hasItems = new ItemInfo[1]
                
            };

            string tempJson = JsonUtility.ToJson(Common.myChar);

            print(tempJson);

            PlayerPrefs.SetString("ppChar", tempJson);
            PlayerPrefs.Save();

            tempJson = null;
        }
        else
        {
            Common.myChar = JsonUtility.FromJson<CharInfo>(ppChar);
        }
             
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
