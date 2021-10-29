using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Player player;
    private bool isActive = false;
    public QuestionPanel queUI;
    public QuestionUI queUIObj;
    void Start()
    {
        
    }

    
    void Update()
    {
        float dis = Vector3.Distance(player.transform.position, transform.position);

        if (dis <= 0.6f&& !isActive&& !queUIObj.gameObject.activeSelf)
        {
            //����ѯ�����

            ShowPanel();
        }
        else if(dis > 0.6f) {

            isActive = false;
            queUI.ShowSelf(false);
        }
        
    }

    private void ShowPanel()
    {
        Debug.Log("�������");
        isActive = true;
        queUI.ShowSelf(true);

    }
}
