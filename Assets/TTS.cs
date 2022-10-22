using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Net;

public class TTS : MonoBehaviour
{
    // Start is called before the first frame update
    [System.Serializable]
    public class SetTextToSpeech{
        public SetInput input;
        public SetVoice voice;
        public SetAudioConfig audioConfig;
    }
    [System.Serializable]
    public class SetInput
    {
        public string text;
    }

    [System.Serializable]
    public class SetVoice
    {
        public string languageCode;
        public string name;
        public string ssmlGender;
    }

    [System.Serializable]
    public class SetAudioConfig
    {
        public string audioEncoding;
        public float speakingRate;
        public int pitch;
        public int volumeGainDb;

    }
    [System.Serializable]
    public class GetContent
    {
        public string audioContent;
    }

    private string apiURL = "https://texttospeech.googleapis.com/v1/text:synthesize?key=AIzaSyB1y-pFHQbmuSHk4vLqejyR8B-Jzl_hhFE";
    //새로운 객체 생성
    SetTextToSpeech tts = new SetTextToSpeech();

    void Start()
    {
        Init();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    void Init()
    {

       
        SetAudioConfig sa=new SetAudioConfig();
        sa.audioEncoding="LINEAR16";
        sa.speakingRate=0.8f;
        sa.pitch=0;
        sa.volumeGainDb=0;
        tts.audioConfig=sa;
        
        SetVoice sv=new SetVoice();
        sv.languageCode="ko-KR";
        sv.name="ko-KR-Standard-A";
        sv.ssmlGender="FEMALE";
        tts.voice=sv;

        

        
    }
    

     AudioSource audioSource;
     public void setText(string text){
        SetInput si=new SetInput();
        si.text=text;
        tts.input=si;
        CreateAudio();
     }

    public void CreateAudio()
    {
        //HttpWebRequest 요청 후 반환된 string 값을 저장
        var str = TTS_Post(tts);

        //string 값을 JsonUtility를 사용하여 JSON 데이터 형식으로 다시 저장
        GetContent info = JsonUtility.FromJson<GetContent>(str);

        //JSON 형식으로 저장된 값을 byte array로 반환
        var bytes = Convert.FromBase64String(info.audioContent);
        //byte array를 float array로 변환
        var f = ConvertByteToFloat(bytes);
        //create audio clip
        AudioClip audioClip = AudioClip.Create("audioContent", f.Length, 1, 44100, false);

        audioClip.SetData(f, 0);
 

        audioSource=transform.GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public static float[] ConvertByteToFloat(byte[] array)
    {
        float[] floatArr = new float[array.Length / 2];
        for (int i = 0; i < floatArr.Length; i++)
        {
            floatArr[i] = BitConverter.ToInt16(array, i * 2) / 32768.0f;
        }
        return floatArr;
    }


     public string TTS_Post(object data)
    {
        //JsonUtility 사용.string을 보내기 위한 byte로 변환
        string str = JsonUtility.ToJson(data);
        var bytes = System.Text.Encoding.UTF8.GetBytes(str);

        //요청을 보낼 주소와 세팅
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiURL);
        request.Method = "POST";
        request.ContentType = "application/json";
        request.ContentLength = bytes.Length;

        //Stream 형식으로 데이터를 보냄 request
        try
        {

            using (var stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
                stream.Close();
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //StreamReader로 stream 데이터를 받음
            StreamReader reader = new StreamReader(response.GetResponseStream());
            //stream 데이터를 string로 변환
            string json = reader.ReadToEnd();

            return json;
        }
        catch (WebException e)
        {
            //log
            Debug.Log(e);
            return null;
        }

    }

}
