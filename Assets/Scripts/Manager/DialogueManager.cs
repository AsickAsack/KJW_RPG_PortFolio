using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public Canvas Dialogue_Canavs;
    public Text Dialouge_NameTx;
    public Text DialogueTx;
    public GameObject chatArrow;
    public Dictionary<int, string[]> Dialogue_Dic = new Dictionary<int, string[]>();
    string[] Dialogue = new string[5];

    int FirstIndex = 2;
    int SecondIndex = 0;
    bool IsDialogue = false;
    public bool IsQuest = false;
    public GameObject QuestButtons;
    public Animator tempanim;

    private void Awake() {

        Instance = this;

        TextAsset OriginalDialogue = Resources.Load("Dialogue") as TextAsset;

        string[] CutRow = OriginalDialogue.text.Split('\n');

        for (int i = 1; i < CutRow.Length - 1; i++)
        {
            string[] CutColumn = CutRow[i].Split(',');
            Dialogue_Dic.Add(int.Parse(CutColumn[0]), CutColumn);

        }

    }


    public void SetDialogue(int index)
    {
        FirstIndex = 2;
        SecondIndex = 0;
        Dialogue = Dialogue_Dic[index];
        Dialouge_NameTx.text = Dialogue[1];
        Dialogue_Canavs.enabled = true;
        StartDialogue();
        //내일삭제
        tempanim.SetBool("IsTalk", true);
    }
    string temp = null;

    void StartDialogue()
    {
        if (Dialogue.Length == FirstIndex)
        {
            Dialogue_Canavs.enabled = false;
            CancelInvoke();
            //내일삭제
            tempanim.SetBool("IsTalk", false);
            return;
        }
        
        IsDialogue = true;

        if (Dialogue[FirstIndex][SecondIndex] == '*')
            temp += ' ';
        else
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
        if (Dialogue[FirstIndex].Contains('*'))
        {
            IsQuest = true;
            QuestButtons.SetActive(true);
            //버튼누르면 quest false
            temp = Dialogue[FirstIndex].Replace('*', ' ');
        }
        else
            temp = Dialogue[FirstIndex];

        DialogueTx.text = temp;
        IsDialogue = false;
        chatArrow.SetActive(true);
        temp = null;
        SecondIndex = 0;
        FirstIndex++;
    }

    public void ClickCheck()
    {
        if(IsDialogue)
        {
            CancelInvoke();
            EndDialogue();
        }
        else
        {   if(!IsQuest)
            { 
                StartDialogue();
                chatArrow.SetActive(false);
            }
        }
    }   

    public void QuestBtn(int index)
    {
        switch(index)
        {
            case 0:
                {
                    IsQuest = false;
                    SetDialogue(3);
                }
                break;
            case 1:
                IsQuest = false;
                ClickCheck();
                break;
        }
        QuestButtons.SetActive(false);
    }
  
}
