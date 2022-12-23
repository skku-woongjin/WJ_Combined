using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Baby_talk : MonoBehaviour
{
    // Start is called before the first frame update

    [System.Serializable]
    public class baby
    {
        public string text;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void baby_audio(string text)
    {
        Debug.Log("babyscript로 넘어옴");
        StartCoroutine(GetBaby("http://13.124.152.251:5000/baby", text));
    }
    string baby_audiourl;
    AudioSource audioSource;
    IEnumerator GetBaby(string url, string babytalk)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {

            //baby 객체
            baby b = new baby();
            b.text = babytalk;
            string baby_data = JsonUtility.ToJson(b);


            var req = new UnityWebRequest(url, "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(baby_data);
            req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            yield return req.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError("error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    //Debug.Log(webRequest.downloadHandler.text);
                    baby_audiourl = webRequest.downloadHandler.text;
                    Debug.Log("getbaby webrequest받아옴");
                    //Debug.Log(baby_audiourl);
                    StartCoroutine(GetBabyAudio(baby_audiourl));
                    break;
            }
            webRequest.Dispose();
        }
        IEnumerator GetBabyAudio(string url)
        {
            using (UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
            {
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
                        Debug.Log("baby audio 받아옴");
                        Debug.Log(webRequest.downloadHandler.text);
                        AudioClip myclip = DownloadHandlerAudioClip.GetContent(webRequest);
                        audioSource = FindObjectOfType<AudioSource>();
                        audioSource.clip = myclip;
                        audioSource.Play();
                        break;
                }
                webRequest.Dispose();
            }
        }
    }
}
