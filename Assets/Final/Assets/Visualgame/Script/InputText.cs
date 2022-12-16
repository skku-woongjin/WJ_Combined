using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class InputText : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_InputField inputField;

    public ChatManager chatmanager;

    [System.Serializable]
    public class Question
    {//보내기 위한 질문
        public string question;
    }
    [System.Serializable]
    public class Answer
    {//받기 위한 answer
        public string answer;
    }


    string text;
    void Start()
    {

    }



    // Update is called once per frame
    void Update()
    {

    }

    int count;


    //VQA 질문을 대화창에 띄우기
    public void GetText()
    {
        if (inputField.text.Length > 0)
        {
            //문장이 너무 짧은지 아닌지 판단(3개 이상의 단어)
            text = inputField.text;
            count = 0;
            string sp_text = text.Trim();//공백제거
            for (int i = 0; i < sp_text.Length; i++)
            {
                if (sp_text[i] == ' ')
                {
                    count++;
                }
            }
            count++;//실제 문장의 단어개수는 +1을 해줘야됨
            //Debug.Log("question: "+text);
            chatmanager.Chat(true, text, "나");
            StartCoroutine(webRequestGet());
            //문장안의 단어개수가 3개이상일 때
            /*if(count>=3){
                StartCoroutine(detect_verb());//문장에 VERB가 포함되는지를 판단
                
                
                
            }
            //단어개수가 3개보다 적을 때
            else{
                StartCoroutine(Too_Short());
                //StartCoroutine(detect_verb());               
            }*/





            inputField.text = "";

        }


    }


    IEnumerator detect_verb()
    {
        Debug.Log("detecting verb");
        string url = "http://3.37.129.107:5000/verb";
        Question q = new Question();
        q.question = text;
        string question_data = JsonUtility.ToJson(q);
        var req = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(question_data);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        //응답 데이터를 StreamReader로 받음
        yield return req.SendWebRequest();
        Debug.Log(req.downloadHandler.text);
        if (req.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error while Sending: " + req.error);
        }
        else
        {
            Debug.Log(req.downloadHandler.text);
            if (req.downloadHandler.text.Equals("No"))
            {
                chatmanager.Chat(false, "Please make a sentence with VERB!", "AI");

            }
            else
            {
                StartCoroutine(webRequestGet());
            }


        }
        req.Dispose();


    }

    //1초 기다리기
    IEnumerator Too_Short()
    {
        yield return new WaitForSecondsRealtime(1);
        chatmanager.Chat(false, "Please write at least three words!", "AI");

    }

    //VQA 대답
    IEnumerator webRequestGet()
    {
        //웹서버 url
        string url = "http://3.37.129.107:5000/vqa";//url 생성
        Question q = new Question();
        q.question = text;
        string question_data = JsonUtility.ToJson(q);
        var req = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(question_data);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");




        //응답 데이터를 StreamReader로 받음
        yield return req.SendWebRequest();
        if (req.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error while Sending: " + req.error);
        }
        else
        {
            chatmanager.Chat(false, req.downloadHandler.text, "타인");


        }
        req.Dispose();


    }


}
