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
            //激活询问面板

            ShowPanel();
        }
        else if(dis > 0.6f) {

            isActive = false;
            queUI.ShowSelf(false);
        }
        
    }

    private void ShowPanel()
    {
        Debug.Log("激活面板");
        isActive = true;
        queUI.ShowSelf(true);

    }
}
