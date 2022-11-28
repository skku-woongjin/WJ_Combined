using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talking_NPC_Area : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject TTS_Audio;
    public GameObject dialogflow;
    public bool Jurassic;
    public bool Solar;
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
            Debug.Log("영역안에 있음");
            dialogflow.GetComponent<diag>().solar_start(GameManager.Instance.userText);
            
            GameManager.Instance.userText_set=false;
        }
    }


    string text;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            In_Area=true;
            chat_log.SetActive(true);
            Debug.Log("Enter area");
            if (Solar)
            {
                text = "나는 태양계로봇 솔라야. 나랑 태양계에 대해서 알아볼까?";
            }
            else if (Jurassic)
            {
                text = "나는 쥬라기로봇 다이노야. 무엇이든지 물어봐!";
            }
            NPC_Talking(text);
            In_Area = true;


        }
    }


    public void NPC_Talking(string text)
    {
        chatmanager.GetComponent<ChatManager>().Chat(false, text, "");
        TTS_Audio.GetComponent<TTS>().setText(text);
        transform.parent.GetComponent<SaySomething>().say(text);
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
