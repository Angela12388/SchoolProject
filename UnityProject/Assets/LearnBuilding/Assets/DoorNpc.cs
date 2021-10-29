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
            //����ѯ�����

            ShowPanel();
        }
        else if (dis > 0.6f)
        {

            isActive = false;
        }

    }

    private void ShowPanel()
    {
        Debug.Log("�������");
        isActive = true;
        doorUI.ShowSelf(true);

    }
}
