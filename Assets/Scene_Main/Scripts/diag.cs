using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

[System.Serializable]
public class dialogflow2
{
    public dfText query_input;

}

[System.Serializable]
public class dfText
{
    public dfText2 text;

}
[System.Serializable]
public class dfText2
{
    public string text;
    public string language_code;
}

[System.Serializable]
public class comfort_question
{
    public string question;
}




public class diag : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject wiki_area;
    public GameObject comfort_area;
    private void Start()
    {

    }

    public void solar_start(string text)
    {
        StartCoroutine(SolarRequests(text));
    }

    public void comfort_Start(string text)
    {
        StartCoroutine(comfortRequests(text));

    }
    private IEnumerator comfortRequests(string question)
    {
        comfort_question cq = new comfort_question();
        cq.question = question;
        string question_data = JsonUtility.ToJson(cq);
        string url = GameManager.Instance.req.comfortURL;
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, question_data))
        {



            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(question_data);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    //Debug.Log("webrequest.downloadhandler.text: "+webRequest.downloadHandler.text);
                    string result = webRequest.downloadHandler.text;
                    comfort_area.GetComponent<Talking_NPC_Area>().NPC_Talking(result);
                    break;
            }
        }
    }

    private IEnumerator SolarRequests(string question)
    {

        comfort_question cq = new comfort_question();
        cq.question = question;
        string question_data = JsonUtility.ToJson(cq);
        string url = GameManager.Instance.req.wikiURL;
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, question_data))
        {



            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(question_data);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    //Debug.Log("webrequest.downloadhandler.text: "+webRequest.downloadHandler.text);
                    string result = webRequest.downloadHandler.text;
                    wiki_area.GetComponent<Talking_NPC_Area>().NPC_Talking(result);
                    break;
            }
        }
    }

    /*private IEnumerator comfortRequests(string text){

    }*/

    IEnumerator sayTwo(string s1, string s2)
    {
        wiki_area.GetComponent<Talking_NPC_Area>().NPC_Talking(s1);
        yield return new WaitForSecondsRealtime(6f);
        wiki_area.GetComponent<Talking_NPC_Area>().NPC_Talking(s2);
    }

    private UnityWebRequest CreateRequest(string path, RequestType type = RequestType.GET, dialogflow2 data = null)
    {
        var request = new UnityWebRequest(path, "POST");

        if (data != null)
        {
            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
            Debug.Log(data.query_input.text.text);
            Debug.Log(JsonUtility.ToJson(data));


            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        return request;

    }

    private void AttachHeader(UnityWebRequest request, string key, string value)
    {
        request.SetRequestHeader(key, value);
    }


}
