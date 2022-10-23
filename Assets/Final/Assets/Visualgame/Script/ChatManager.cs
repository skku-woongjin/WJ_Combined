using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using TMPro;
using System;
using System.IO;
using System.Net;

public class ChatManager : MonoBehaviour
{
    public GameObject YellowArea, WhiteArea;
    public RectTransform ContentRect;
    public Scrollbar scrollbar;

    AreaScript LastArea;

    void Start()
    {
        //babyscript=GameObject.Find("GameManager").transform.GetComponent<Baby_talk>(); //babyscript로 가서 baby목소리 가져오려는 작업
        //StartCoroutine(GetBaby("http://13.124.152.251:5000/baby"));

    }



    // Update is called once per frame
    void Update()
    {

    }






    int question_count = 0;

    public void Chat(bool isSend, string text, string user)
    {
        if (text.Trim() == "") return;
        bool isBottom = scrollbar.value <= 0.00001f;

        AreaScript Area = Instantiate(isSend ? YellowArea : WhiteArea, ContentRect.transform).GetComponent<AreaScript>();
        // Area.transform.SetParent(ContentRect.transform, false);
        // Area.BoxRect.sizeDelta = new Vector2(300, Area.BoxRect.sizeDelta.y);
        if (text.Length > 15)
        {
            text = text.Insert(14, "\n");
        }
        if (text.Length > 30)
        {
            text = text.Insert(29, "\n");
        }
        Area.TextRect.GetComponent<TMP_Text>().text = text;
        // Fit(Area.BoxRect);

        StartCoroutine(rebuild(ContentRect));




        //사용자가 질문할 때




        // 두 줄 이상이면 크기를 줄여가면서, 한 줄이 아래로 내려가면 바로 전 크기를 대입 
        float X = Area.TextRect.sizeDelta.x + 42;
        float Y = Area.TextRect.sizeDelta.y;
        if (Y > 49)
        {
            for (int i = 0; i < 200; i++)
            {
                Area.BoxRect.sizeDelta = new Vector2(X - i * 2, Area.BoxRect.sizeDelta.y);
                Fit(Area.BoxRect);

                if (Y != Area.TextRect.sizeDelta.y) { Area.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
            }
        }

        else Area.BoxRect.sizeDelta = new Vector2(X, Y);

        Fit(Area.BoxRect);
        Fit(Area.AreaRect);
        Fit(ContentRect);
        LastArea = Area;

        if (!isSend && !isBottom) return;
        Invoke("ScrollDelay", 0.03f);



    }




    void Fit(RectTransform Rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);
    void ScrollDelay() => scrollbar.value = 0;

    IEnumerator rebuild(RectTransform obj)
    {
        yield return new WaitForEndOfFrame();
        LayoutRebuilder.ForceRebuildLayoutImmediate(obj);

    }

}



