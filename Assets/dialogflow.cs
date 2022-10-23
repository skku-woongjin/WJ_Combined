using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;




public class dialogflow2{
    public dfText query_input;

}

public class dfText {
   public dfText2 text;

}

public class dfText2 {
    public string text;
    public string language_code;
}


// public class dialogflow : MonoBehaviour {

//     private string pubkey;
// 	class CertPublicKey : CertificateHandler
// 	{
// 	    public string PUB_KEY;

// 	    // Encoded RSAPublicKey
// 	    protected override bool ValidateCertificate(byte[] certificateData)
// 	    {
// 	        X509Certificate2 certificate = new X509Certificate2(certificateData);
// 	        string pk = certificate.GetPublicKeyString();

// 	        if (pk.ToLower().Equals(PUB_KEY.ToLower()))
// 	            return true;
// 	        else
// 	            return false;
// 	    }
// 	}

//     string data = string.Empty;


//     void Start () {
//         //인증에 필요한 PublicKey 생성
// 		TextAsset tx = Resources.Load<TextAsset>("certificateFile");
// 		byte[] by = Encoding.UTF8.GetBytes(tx.ToString());

// 		X509Certificate2 certificate = new X509Certificate2(by);
// 		pubkey = certificate.GetPublicKeyString();

// 	    StartCoroutine("https://dialogflow.clients6.google.com/v2/projects/dialogflow11-363401/agent/sessions/87106d06-a910-f202-4f14-cbd4ec7d7128:detectIntent", "POST");
//     }


// 	private IEnumerator Post(string url, byte[] data)
// 	{
// 		//byte로 데이터를 전송 해야하는데 UnityWebRequest.POST는 string만 가능 하여 Put으로 넣은뒤 POST로 변경 해서 전송

// 	    UnityWebRequest request = UnityWebRequest.Put("https://dialogflow.clients6.google.com/v2/projects/dialogflow11-363401/agent/sessions/87106d06-a910-f202-4f14-cbd4ec7d7128:detectIntent", data);
// 	    {
// 	        request.method = "POST";
// 	        request.certificateHandler = new CertPublicKey{ PUB_KEY = pubkey };
// 	        //request.SetRequestHeader("Content-Type", "application/json"); //json전송이 필요하다면

// 	        yield return request.SendWebRequest();

// 	        if (request.isNetworkError)
// 	        {
// 	            Debug.Log(request.error + " / " + request.responseCode);
// 	        }
// 	        else
// 	        {
// 	            Debug.Log(request.downloadHandler.text);
// 	        }
// 	    }
// 	}




//     IEnumerator Upload() {

//         dialogflow2 body = new dialogflow2();
//         body.query_input = new dfText();
//         body.query_input.text = new dfText2();
//         body.query_input.text.text = "나랑 놀자!";
//         body.query_input.text.language_code = "ko";        

//         string bodyData = JsonUtility.ToJson(body);
//         Debug.Log(bodyData);

//         var req = new UnityWebRequest("https://dialogflow.clients6.google.com/v2/projects/dialogflow11-363401/agent/sessions/87106d06-a910-f202-4f14-cbd4ec7d7128:detectIntent", "POST");

//         byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(bodyData);
//         req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
//         req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
//         req.SetRequestHeader("Authorization", "Bearer ya29.a0Aa4xrXNip8CB7fJlBAmoCkyNq_nDBQitKaGAXeOaqbk6KFz8GCkB_3CO6vGMEkY1QONRpsMMWJxyCweFDySkUNlZHM_QyygE7R3LL0D9IHcHSgOi2aJl6cIT0gq0DFwiIZ2qG8eammS6_E-iv7zzegA2IvAs0X5A6PSYRYQfTJppzvrbs5Tyb7pCJBTXnXtvAvVhAvwz7f3X5BH4e4xPq8ftm8OLF9a_eU_0h32FWJa6xjUaCgYKATASARISFQEjDvL9QgXPI8d_5iyWoePf55O3Pw0246");

//         yield return req.SendWebRequest();

//         if (req.result == UnityWebRequest.Result.ConnectionError)
//         {
//             Debug.Log("Error While Sending: " + req.error);
//         }
//         else 
//         {
//             Debug.Log(req.result);
//         }
//     }
// }



public class dialogflow : MonoBehaviour {


    public void start() {
        Debug.Log("plz");
    StartCoroutine(MakeRequests());
    }

    private IEnumerator MakeRequests() {
        //GET
        var getRequest = CreateRequest("https://dialogflow.clients6.google.com/v2/projects/dialogflow11-363401/agent/sessions/87106d06-a910-f202-4f14-cbd4ec7d7128:detectIntent");

        AttachHeader(getRequest, "Authorization", "Bearer ya29.a0Aa4xrXNip8CB7fJlBAmoCkyNq_nDBQitKaGAXeOaqbk6KFz8GCkB_3CO6vGMEkY1QONRpsMMWJxyCweFDySkUNlZHM_QyygE7R3LL0D9IHcHSgOi2aJl6cIT0gq0DFwiIZ2qG8eammS6_E-iv7zzegA2IvAs0X5A6PSYRYQfTJppzvrbs5Tyb7pCJBTXnXtvAvVhAvwz7f3X5BH4e4xPq8ftm8OLF9a_eU_0h32FWJa6xjUaCgYKATASARISFQEjDvL9QgXPI8d_5iyWoePf55O3Pw0246");

        yield return getRequest.SendWebRequest();

        var deserializedGetData = JsonUtility.FromJson<Todo>(getRequest.downloadHandler.text);

        //POST
        dialogflow2 body = new dialogflow2();
        body.query_input = new dfText();
        body.query_input.text = new dfText2();
        body.query_input.text.text = "안녕";
        body.query_input.text.language_code = "ko";

        var postRequest = CreateRequest("https://dialogflow.clients6.google.com/v2/projects/dialogflow11-363401/agent/sessions/87106d06-a910-f202-4f14-cbd4ec7d7128:detectIntent", RequestType.POST, body);

        yield return postRequest.SendWebRequest();
        Debug.Log(postRequest.downloadHandler.text);
        var deserializedPostData = JsonUtility.FromJson<postResult>(postRequest.downloadHandler.text);

    }

    private UnityWebRequest CreateRequest(string path, RequestType type = RequestType.GET, object data = null) {
        var request = new UnityWebRequest(path, "GET");

        if(data != null) {
            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        return request;

    }

    private void AttachHeader(UnityWebRequest request, string key, string value) {
    request.SetRequestHeader(key, value);
    }


}

public enum RequestType {
    GET = 0,
    POST = 1,
    PUT = 2
}

public class Todo {
    public int uesrId;
    public int id;
    public string title;
    public bool completed;

}



public class PostData {
    public string Hero;
    public int PowerLevel;
}

public class postResult {
    public string success {get; set; }
}