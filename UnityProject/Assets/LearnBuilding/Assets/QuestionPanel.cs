using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionPanel : MonoBehaviour
{
    public Button learnBtn;
    public Button backBtn;
    public GameObject queObj;

    public QuestionUI questionUI;
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
        questionUI.Init();
        questionUI.ShowSelf(true);
       // queObj.SetActive(true);
        ShowSelf(false);

    }

    private void OnclickExit()
    {
        Debug.Log("·ÅÆú");
        ShowSelf(false);
        questionUI.ShowSelf(false);
        // queObj.SetActive(false);
    }

    public void ShowSelf(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
