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
                q = "树上10只鸟，一共20个翅膀？";
                break;
            case 1:
                q = "树上骑个猴，地上一个猴，一共2个猴？";
                break;
            case 2:
                q = "吃瓜子不吐葡萄皮是正常骚操作么？";
                break;
            default:
                break;
        }

        queText.text = q;
    }

    private void OnclickBtn_A()
    {
        //加一分

        ShowSelf(false);
        curScore++;

        score.text = "金币：" + curScore;
    }

    private void OnclickBtn_B()
    {
        //不加分
        ShowSelf(false);
        curScore--;
        score.text = "金币：" + curScore;
    }

    public void ShowSelf(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
