using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Pororo : MonoBehaviour
{
    // Start is called before the first frame update

    [System.Serializable]
    public class chat
    {
        public string question;
    }
    void Start()
    {

    }
    string url = "http://13.125.98.9:5000/";

    public void clicked()
    {
        chat example = new chat();
        example.question = "안녕하세요";
        string json = JsonUtility.ToJson(example);

        StartCoroutine(Upload(url, json));
    }

    IEnumerator Upload(string URL, string json)
    {
        using (UnityWebRequest request = UnityWebRequest.Post(URL, json))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("received successfully");
                Debug.Log(request.downloadHandler.text);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
