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
        book_1.Add("����Ȼ_1���ÿ�����Ĺ����������������������޺��������ʹ��Ŭ���������������̾Ϳ�ିռ�ģ��񰲻յĿռ������");
        book_1.Add("����Ȼ_2���������������������������ÿ�����Ŀ��������޺��������ʹ��Ŭ���������������̾Ϳ�ିռ�ģ��񰲻յĿռ������");
        book_1.Add("����Ȼ_3���ÿ�������ÿ�����ĵĹ����������������������޺��������ʹ��Ŭ�������������ռ�ģ��񰲻յĿռ������");
        books.Add("����Ȼ", book_1);

        book_2.Add("��������_1���ÿ�����Ĺ����������������������޺��������ʹ��Ŭ���������������̾Ϳ�ିռ�ģ��񰲻յĿռ������");
        book_2.Add("��������_2:�������������������������ÿ�����Ŀ��������޺��������ʹ��Ŭ���������������̾Ϳ�ିռ�ģ��񰲻յĿռ������");
        book_2.Add("��������_3:�ÿ�������ÿ�����ĵĹ����������������������޺��������ʹ��Ŭ�������������ռ�ģ��񰲻յĿռ������");
        books.Add("��������", book_2);

        book_3.Add("ɽ����_1���ÿ�����Ĺ����������������������޺��������ʹ��Ŭ���������������̾Ϳ�ିռ�ģ��񰲻յĿռ������");
        book_3.Add("ɽ����_2���������������������������ÿ�����Ŀ��������޺��������ʹ��Ŭ���������������̾Ϳ�ିռ�ģ��񰲻յĿռ������");
        book_3.Add("ɽ����_3���ÿ�������ÿ�����ĵĹ����������������������޺��������ʹ��Ŭ�������������ռ�ģ��񰲻յĿռ������");
        books.Add("ɽ����", book_3);




    }


    private void HitBook(Book book)
    {
        Debug.LogWarning("������鱾");
        string K="";
        int index = Random.Range(0,3);
        switch (index)
        {
            case 0:
                K = "����Ȼ";
                break;
            case 1:
                K = "��������";
                break;
            case 2:
                K = "ɽ����";
                break;

            default:
                break;
        }


        bookUI.OnInit(books[K]);
    }

}
