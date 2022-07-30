using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutCineDialogue : MonoBehaviour
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
        Dialogue_Dic.Add(0, new string[] { "0", "로저", "...오랜만이오 사라.." });
        Dialogue_Dic.Add(1, new string[] { "1", "사라(여왕)", "...보고싶었어요..흑흑" });
        Dialogue_Dic.Add(2, new string[] { "2", "로저", "울지마시오.." });
        Dialogue_Dic.Add(3, new string[] { "3", "로저", "10년동안 그대를 구하기 위..흡" });
        Dialogue_Dic.Add(4, new string[] { "4", "로저", "어..째..서.." });
        Dialogue_Dic.Add(5, new string[] { "5", "사라(여왕)", "난 이제 왕을 더 사랑해요.\n 잘가 로저." });
        Dialogue_Dic.Add(6, new string[] { "6", "사라", "여보!! 여보!! 왜 여기서 자고있어?" });
        Dialogue_Dic.Add(7, new string[] { "7", "로저", "아.. 깜빡 잠이 들었어.." });
        Dialogue_Dic.Add(8, new string[] { "8", "로저", "(끔찍한 꿈이였어.)" });
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
}
