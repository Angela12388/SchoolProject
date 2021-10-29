using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Book : MonoBehaviour
{
    private List<string> book_1 = new List<string>();
    private List<string> book_2 = new List<string>();
    private List<string> book_3 = new List<string>();

    private Dictionary<string, List<string>> books = new Dictionary<string, List<string>>();
    public Player player;
    public BookUI bookUI;
    void Start()
    {
        InitBook();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }



        float dis = Vector3.Distance(transform.position,player.transform.position);

        if (dis < 0.6f&&Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            bool t = Physics.Raycast(ray, out hit);
            if (t)
            {

                Book b = hit.collider.GetComponent<Book>();
                

                if (b != null)
                {
                    HitBook(b);
                }
            }

        }
    }

    private void InitBook()
    {
        book_1.Add("大自然_1：好卡激活的公交卡，案件管理卡机，阿罗汉果，来和大家努力开发，骄傲立刻就咖喱空间的，今安徽的空间里噶！");
        book_1.Add("大自然_2：案件管理公交卡，案件管理卡机好卡激活的卡机，阿罗汉果，来和大家努力开发，骄傲立刻就咖喱空间的，今安徽的空间里噶！");
        book_1.Add("大自然_3：好卡激活卡机好卡激活的的公交卡，案件管理卡机，阿罗汉果，来和大家努力开发，骄傲空间的，今安徽的空间里噶！");
        books.Add("大自然", book_1);

        book_2.Add("宇宙物体_1：好卡激活的公交卡，案件管理卡机，阿罗汉果，来和大家努力开发，骄傲立刻就咖喱空间的，今安徽的空间里噶！");
        book_2.Add("宇宙物体_2:案件管理公交卡，案件管理卡机好卡激活的卡机，阿罗汉果，来和大家努力开发，骄傲立刻就咖喱空间的，今安徽的空间里噶！");
        book_2.Add("宇宙物体_3:好卡激活卡机好卡激活的的公交卡，案件管理卡机，阿罗汉果，来和大家努力开发，骄傲空间的，今安徽的空间里噶！");
        books.Add("宇宙物体", book_2);

        book_3.Add("山海经_1：好卡激活的公交卡，案件管理卡机，阿罗汉果，来和大家努力开发，骄傲立刻就咖喱空间的，今安徽的空间里噶！");
        book_3.Add("山海经_2：案件管理公交卡，案件管理卡机好卡激活的卡机，阿罗汉果，来和大家努力开发，骄傲立刻就咖喱空间的，今安徽的空间里噶！");
        book_3.Add("山海经_3：好卡激活卡机好卡激活的的公交卡，案件管理卡机，阿罗汉果，来和大家努力开发，骄傲空间的，今安徽的空间里噶！");
        books.Add("山海经", book_3);




    }


    private void HitBook(Book book)
    {
        Debug.LogWarning("点击中书本");
        string K="";
        int index = Random.Range(0,3);
        switch (index)
        {
            case 0:
                K = "大自然";
                break;
            case 1:
                K = "宇宙物体";
                break;
            case 2:
                K = "山海经";
                break;

            default:
                break;
        }


        bookUI.OnInit(books[K]);
    }

}
