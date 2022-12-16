using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talking_NPC_Area : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject TTS_Audio;
    public GameObject dialogflow;
    public bool Jurassic;
    public bool comfort;
    bool In_Area = false;
    public GameObject chatmanager;
    public GameObject chat_log;
    
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //NPC 영역안에 있고 유저가 텍스트를 전송했을 경우
        if (In_Area && GameManager.Instance.userText_set)
        {
            if(Jurassic){
                Debug.Log("영역안에 있음");
                dialogflow.GetComponent<diag>().solar_start(GameManager.Instance.userText);
            }
            else if(comfort){
                dialogflow.GetComponent<diag>().comfort_Start(GameManager.Instance.userText);
            }
            
            
            GameManager.Instance.userText_set=false;
        }
    }


    string text;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            In_Area=true;
            //chat_log.SetActive(true);
            Debug.Log("Enter area");
            if (comfort)
            {
                text = "저는 위로로봇 힐링입니다. 고민거리가 있으신가요?";
            }
            else if (Jurassic)
            {
                text = "나는 질의응답로봇 위키야. 무엇이든지 물어봐!";
            }
            NPC_Talking(text);
            In_Area = true;


        }
    }


    public void NPC_Talking(string text)
    {
        chatmanager.GetComponent<ChatManager>().Chat(false, text, "");
        transform.parent.GetComponent<SaySomething>().say(text);
        if(text.Contains(")")){
            int startIdx = text.IndexOf(")");

            string splitStr = text.Substring(startIdx + 1); // google.com
            text=splitStr;
        }
        
        TTS_Audio.GetComponent<TTS>().setText(text);
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            In_Area=false;
            Debug.Log("Exit area");
            chat_log.SetActive(false);
        }
    }
}
