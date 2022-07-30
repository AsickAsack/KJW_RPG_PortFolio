using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KingMeetDialogue : MonoBehaviour
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
        Dialogue_Dic.Add(0, new string[] { "0", "��", "���ɵ� ������� �����Ա�. ����." });
        Dialogue_Dic.Add(1, new string[] { "1", "����", "���ܿ� ��¦�� �������� ���±���." });
        Dialogue_Dic.Add(2, new string[] { "2", "��", "���� �̹� �� �ؾ���.\n ������ �� �ձ��� �պ���." });
        Dialogue_Dic.Add(3, new string[] { "3", "����", "����! �� ���̰� ��� ���ϰڴ�!" });
        
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
