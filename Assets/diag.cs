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




public class diag : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("plz");
        //StartCoroutine(MakeRequests());
    }

    public void solar_start(string text){
        StartCoroutine(MakeRequests(text));
    }

    private IEnumerator MakeRequests(string question)
    {
        //GET
        // var getRequest = CreateRequest("https://dialogflow.clients6.google.com/v2/projects/dialogflow11-363401/agent/sessions/87106d06-a910-f202-4f14-cbd4ec7d7128:detectIntent");

        // AttachHeader(getRequest, "Authorization", "Bearer ya29.a0Aa4xrXNip8CB7fJlBAmoCkyNq_nDBQitKaGAXeOaqbk6KFz8GCkB_3CO6vGMEkY1QONRpsMMWJxyCweFDySkUNlZHM_QyygE7R3LL0D9IHcHSgOi2aJl6cIT0gq0DFwiIZ2qG8eammS6_E-iv7zzegA2IvAs0X5A6PSYRYQfTJppzvrbs5Tyb7pCJBTXnXtvAvVhAvwz7f3X5BH4e4xPq8ftm8OLF9a_eU_0h32FWJa6xjUaCgYKATASARISFQEjDvL9QgXPI8d_5iyWoePf55O3Pw0246");

        // yield return getRequest.SendWebRequest();

        // var deserializedGetData = JsonUtility.FromJson<Todo>(getRequest.downloadHandler.text);

        //POST
        dialogflow2 body = new dialogflow2();
        body.query_input = new dfText();
        body.query_input.text = new dfText2();
        body.query_input.text.text = question;
        body.query_input.text.language_code = "ko";

        var postRequest = CreateRequest("https://dialogflow.clients6.google.com/v2/projects/dialogflow11-363401/agent/sessions/87106d06-a910-f202-4f14-cbd4ec7d7128:detectIntent", RequestType.POST, body);
        AttachHeader(postRequest, "Authorization", "Bearer ya29.a0Aa4xrXOc9EWEyZ9i6MBRgwttFopGyEOVSVG8m6GYD5VSJiLgWYaoqQfeDOnlsWesEOeks2tig2nX8yAcsEdGqR3n5Y2mahQ1TiWj_pyM3BzYY_AWo8_Hwo6K7dUAS20p71PhQBHJMFlXbqX-u7P2gSVvUY4FE7k4lMOFO3ES-7wQN3t_TVGAB8rJUnG8p1eRIEWbqt5TYnhJoDALlTPnZ3sA82fIVuS0NeFRSVcGVoz_yMgaCgYKATASARISFQEjDvL94aluNL9bxozrorYm-25m4Q0246");
        yield return postRequest.SendWebRequest();
        Debug.Log(postRequest.downloadHandler.text);
        string split=postRequest.downloadHandler.text.Split("fulfillmentText\": \"", System.StringSplitOptions.RemoveEmptyEntries)[1];
        split=split.Split("\",",System.StringSplitOptions.RemoveEmptyEntries)[0];
        Debug.Log(split);
        // var deserializedPostData = JsonUtility.FromJson<postResult>(postRequest.downloadHandler.text);

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
