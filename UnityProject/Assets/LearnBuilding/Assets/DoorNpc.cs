using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorNpc : MonoBehaviour
{
    public Player player;
    private bool isActive = false;
    public DoorUI doorUI;
    public GameObject doorObj;
    void Start()
    {

    }


    void Update()
    {
        float dis = Vector3.Distance(player.transform.position, transform.position);

        if (dis <= 0.6f && doorObj.activeSelf && !isActive)
        {
            //激活询问面板

            ShowPanel();
        }
        else if (dis > 0.6f)
        {

            isActive = false;
        }

    }

    private void ShowPanel()
    {
        Debug.Log("激活面板");
        isActive = true;
        doorUI.ShowSelf(true);

    }
}
