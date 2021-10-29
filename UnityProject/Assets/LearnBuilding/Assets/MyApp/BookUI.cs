using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BookUI : MonoBehaviour
{
    public Button closeBtn;
    public Button preBtn;
    public Button nextBtn;
    List<string> book;


    public Text contentBook;
    private int curPage = 0;
    void Start()
    {
        closeBtn.onClick.AddListener(OnclickCloseBtn);
        preBtn.onClick.AddListener(OnclickPreBtn);
        nextBtn.onClick.AddListener(OnclickNextBtn);
    }

    
    void Update()
    {
        
    }
    public void OnInit(List<string>b)
    {
        book = b;
        curPage = 0;
        UpdatePage();

        ShowSelf(true);
    }


    private void OnclickCloseBtn()
    {
        ShowSelf(false);
    }

    private void OnclickPreBtn()
    {
        curPage --;

        if (curPage <= 0) curPage = 0;

        UpdatePage();
    }

    private void OnclickNextBtn()
    {
        curPage++;

        if (curPage >= book.Count) curPage = book.Count - 1;

        UpdatePage();
    }

    private void UpdatePage()
    {
        contentBook.text = book[curPage];
    }

    public void ShowSelf(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
