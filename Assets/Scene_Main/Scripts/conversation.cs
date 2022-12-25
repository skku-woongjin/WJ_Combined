using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class conversation : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_InputField input;
    public GameObject chatmanager;
    public GameObject chat_log;
    //public GameObject textChatPrefab;
    //public Transform parentcontent;
    void Start()
    {
        GameManager.Instance.userText_set = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void sudden_chat_log()
    {

        if (chat_log.activeSelf)
        {
            chat_log.SetActive(false);
        }
        else
        {
            chat_log.SetActive(true);
        }

    }

    public void OnEndEditEventMethod()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {

            UpdateChat();
        }
    }

    public void UpdateChat()
    {
        if (input.text.Equals("")) return;
        //GameManager.Instance.checkUserSpeak = true;
        GameManager.Instance.userText_set = true;

        GameManager.Instance.userText = input.text;
        GameManager.Instance.owner.GetComponent<SaySomething>().say(input.text);

        chatmanager.GetComponent<ChatManager>().Chat(true, input.text, "");

        //GameObject clone = Instantiate(textChatPrefab, parentcontent);
        //clone.GetComponent<TextMeshProUGUI>().text = $"나: {input.text}";
        input.text = "";

        GUI.FocusControl(null);
    }
}
