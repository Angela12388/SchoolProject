using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorUI : MonoBehaviour
{
    public Button learnBtn;
    public Button backBtn;
    public GameObject doorObj;


    void Start()
    {
        learnBtn.onClick.AddListener(OnclickLearnBtn);
        backBtn.onClick.AddListener(OnclickExit);


    }

    
    void Update()
    {
        
    }


    private void OnclickLearnBtn()
    {
        doorObj.SetActive(false);
        ShowSelf(false);
    }

    private void OnclickExit()
    {
        Debug.Log("·ÅÆú");
        ShowSelf(false);
    }

    public void ShowSelf(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
