using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionUI : MonoBehaviour
{
    public Button A_Btn;
    public Button B_Btn;
    public Text queText;
    public Text score;
    private int curScore=10;

    void Start()
    {
        A_Btn.onClick.AddListener(OnclickBtn_A);
        B_Btn.onClick.AddListener(OnclickBtn_B);


    }


    void Update()
    {

    }

    public void Init()
    {
        string q = "";

        int index = Random.Range(0, 3);

        switch (index)
        {
            case 0:
                q = "����10ֻ��һ��20�����";
                break;
            case 1:
                q = "������������һ���һ��2���";
                break;
            case 2:
                q = "�Թ��Ӳ�������Ƥ������ɧ����ô��";
                break;
            default:
                break;
        }

        queText.text = q;
    }

    private void OnclickBtn_A()
    {
        //��һ��

        ShowSelf(false);
        curScore++;

        score.text = "��ң�" + curScore;
    }

    private void OnclickBtn_B()
    {
        //���ӷ�
        ShowSelf(false);
        curScore--;
        score.text = "��ң�" + curScore;
    }

    public void ShowSelf(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
