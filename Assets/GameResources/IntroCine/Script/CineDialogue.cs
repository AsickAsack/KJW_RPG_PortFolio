using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CineDialogue : MonoBehaviour
{
    public Canvas Dialogue_Canavs;
    public Text Dialouge_NameTx;
    public Text DialogueTx;
    public Dictionary<int, string[]> Dialogue_Dic = new Dictionary<int, string[]>();
    string[] Dialogue = new string[5];

    int FirstIndex = 2;
    int SecondIndex = 0;
    int DialougeIndex = 0;

    private void OnEnable()
    {
        SetDialogue(DialougeIndex++);
    }


    private void Awake()
    {
        Dialogue_Dic.Add(0, new string[] { "0", "로저", "후.. 거의 다해가는군." });
        Dialogue_Dic.Add(1, new string[] { "1", "사라", "여보, 그만하고 들어와서 밥먹어요." });
        Dialogue_Dic.Add(2, new string[] { "2", "왕국 장교", "오랜만이군 로저." });
        Dialogue_Dic.Add(3, new string[] { "3", "로저", "반갑지는 않군.\n무슨 일이지?" });
        Dialogue_Dic.Add(4, new string[] { "4", "왕국 장교", "왕께서 당신의 아내를 원하시는군..큭큭" });
        Dialogue_Dic.Add(5, new string[] { "5", "로저", "뭐라고? 가까이오면 죽여버리겠다!" });
        Dialogue_Dic.Add(6, new string[] { "6", "왕국 장교", "가서 죽여!" });
        Dialogue_Dic.Add(7, new string[] { "7", "왕국 장교", "멍청한 녀석..\n여자는 다치지않게 잘 데려와." });
        Dialogue_Dic.Add(8, new string[] { "8", "로저", "드디어 때가 왔군.\n기다려 사라.. " });
    }



    public void SetDialogue(int index)
    {
        FirstIndex = 2;
        SecondIndex = 0;
        Dialogue = Dialogue_Dic[index];
        Dialouge_NameTx.text = Dialogue[1];
        Dialogue_Canavs.enabled = true;
        StartDialogue();

    }

    string temp = null;

    void StartDialogue()
    {
        if (Dialogue.Length == FirstIndex)
        {
            Dialogue_Canavs.gameObject.SetActive(false);
            CancelInvoke();

            return;
        }
            temp += Dialogue[FirstIndex][SecondIndex];

        DialogueTx.text = temp;

        if (Dialogue[FirstIndex].Length == SecondIndex + 1)
            EndDialogue();
        else
        {
            SecondIndex++;
            Invoke("StartDialogue", 0.08f);
        }

    }

    void EndDialogue()
    {   
        temp = Dialogue[FirstIndex];
        DialogueTx.text = temp;
        temp = null;
        SecondIndex = 0;
        FirstIndex++;

        Invoke("StartDialogue", 0.3f);
        
    }

    public void GameStart()
    {
        SceneLoader.Instance.Loading_LoadScene(1);
    }


}
