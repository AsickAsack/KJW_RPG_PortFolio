using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public Quest[] myQuest;
    public static QuestManager instance;

    public GameObject QuestPanel;
    public Text QuestNametx;
    public Text QuestProcesstx;
    public Text P_QuestNametx;
    public Text P_QuestDescription;
    public bool QuestFinish = false;
    public Quest CurQuest;
    int process = 0;

    private void Awake()
    {
        instance = this;
        if (PlayerPrefs.HasKey("Process"))
            process = PlayerPrefs.GetInt("Process");
    }

    private void Start()
    {        
        if(GameData.Instance.playerdata.Quest)
        {
            SetQuest(10);
            
        }
    }

    public void SetQuest(int index)
    {
        for(int i=0; i < myQuest.Length; i++)
        {
            if (GameData.Instance.playerdata.StoryIndex + index == myQuest[i].Questid)
            {
                CurQuest = myQuest[i];
                SetQuestUi(i);
                break;
            }
            
        }
    }

    public void SetQuestUi(int QuestIndex)
    {
        QuestPanel.SetActive(true);
        QuestNametx.text = P_QuestNametx.text = myQuest[QuestIndex].Questname;
        QuestProcesstx.text = myQuest[QuestIndex].soldierDatas.SoldierName +" "+ process.ToString() + "/" + myQuest[QuestIndex].MonsterRequirement.ToString();
        P_QuestDescription.text = myQuest[QuestIndex].Questdescription;
    }

    public void SetRequireMent(SoldierData solider)
    {
        if(CurQuest != null && !QuestFinish)
        {
            if(CurQuest.soldierDatas == solider)
            {
                process++;
                PlayerPrefs.SetInt("Process", process);
                if (process >= CurQuest.MonsterRequirement)
                {
                    QuestFinish = true;
                    QuestProcesstx.text = "퀘스트 완료!";
                    GameData.Instance.playerdata.StoryIndex++;
                }
                else
                    QuestProcesstx.text = CurQuest.soldierDatas.SoldierName + " " + process.ToString() + "/" + CurQuest.MonsterRequirement.ToString();
            }
        }
      
    }

    public void getReward()
    {
        if(CurQuest != null && QuestFinish)
        {
            SoundManager.Instance.PlayEffect1Shot(15);
            QuestFinish = false;
            GameData.Instance.playerdata.Quest = false;
            GameData.Instance.playerdata.myItems.Add(new(CurQuest.Reward));
            GameData.Instance.SetNotify("퀘스트 완료!\n"+CurQuest.Reward.ItemName + "을 얻었습니다.");
            CurQuest = null;
            process = 0;
            QuestPanel.SetActive(false);
            if(PlayerPrefs.HasKey("Process"))
            PlayerPrefs.DeleteKey("Process");
        }
    }
}
