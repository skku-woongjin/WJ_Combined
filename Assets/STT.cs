using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using UnityEngine.Networking;
using UnityEngine.UI;

public class STT : MonoBehaviour
{
    public Button mic;
    public GameObject chatmanager;
    

    [System.Serializable]
    public class STT_Request{
        public SetConfig config;
        public SetAudio audio;
    }

    [System.Serializable]
    public class SetConfig{
        //public string encoding;
        //public int sampleRateHertz;
        public string languageCode;
    }

    [System.Serializable]
    public class SetAudio{
        public string content;
        //public string uri;

    }

    [System.Serializable]
    public class ResponseBody{
        public List<Result> results;
        
    }
    [System.Serializable]
    public class Result{
        public List<Alternatives> alternatives;
        public string languageCode;
    }
    [System.Serializable]
    public class Alternatives{
        public string transcript;
        public float confidence;
    }
    
    AudioSource aud;
    private int _recordingLengthSec=3;
    private int _recoringHZ=22050;

    bool record=false;

    STT_Request stt=new STT_Request();
    
    // Start is called before the first frame update
    void Start()
    {
       
        aud=GetComponent<AudioSource>();
        Debug.Log(Microphone.devices[0]);
        
    }

    public void play(){
        aud.Play();
    }
    public void startRecording(){
        Debug.Log("start recording");
        aud.clip=Microphone.Start(Microphone.devices[0],false,_recordingLengthSec,_recoringHZ);
        record=true;
    }

    public void stopRecording(){
        /*if(Microphone.IsRecording(Microphone.devices[0])){
            Microphone.End(Microphone.devices[0]);
            Debug.Log("stop recording");
            if(aud.clip==null){
                Debug.LogError("nothing recorded");
                return;
            }
            SetConfig cf=new SetConfig();
            cf.languageCode="ko-KR";
            cf.encoding="LINEAR16";
            cf.sampleRateHertz=16000;
            stt.config=cf;

            SetAudio sa=new SetAudio();
            //sa.content=
            stt.audio=sa;
            

            byte[] byteData=getByteFromAudioClip(aud.clip);

            string base64=Convert.ToBase64String(byteData);
            StartCoroutine(GoogleSpeechToText(base64));
        }*/
        Debug.Log("stop recording");
        mic.interactable=true;
        if(aud.clip==null){
                Debug.LogError("nothing recorded");
                return;
            }
            SetConfig cf=new SetConfig();
            cf.languageCode="ko-KR";
            //cf.encoding="LINEAR16";
            //cf.sampleRateHertz=16000;
            stt.config=cf;

            SetAudio sa=new SetAudio();
           
            
            
            
            byte[] byteData=getByteFromAudioClip(aud.clip);

            string base64=Convert.ToBase64String(byteData);
            sa.content=base64;
            //sa.uri="gs://cloud-samples-tests/speech/brooklyn.flac";
            stt.audio=sa;
            StartCoroutine(GoogleSpeechToText(base64));
    }

    const int BlockSize_16Bit=2;
    private byte[] getByteFromAudioClip(AudioClip audioClip)
{
    MemoryStream stream = new MemoryStream();
    const int headerSize = 44;
    ushort bitDepth = 16;

    int fileSize = audioClip.samples * BlockSize_16Bit + headerSize;

    // audio clip의 정보들을 file stream에 추가(링크 참고 함수 선언)
    WriteFileHeader(ref stream, fileSize);
    WriteFileFormat(ref stream, audioClip.channels, audioClip.frequency, bitDepth);
    WriteFileData(ref stream, audioClip, bitDepth);
    
    // stream을 array형태로 바꿈
    byte[] bytes = stream.ToArray();

    return bytes;
}

    

     string requestURI="https://speech.googleapis.com/v1p1beta1/speech:recognize?key=AIzaSyB1y-pFHQbmuSHk4vLqejyR8B-Jzl_hhFE";


    IEnumerator GoogleSpeechToText(string base64content){
        //WWWForm form=new WWWForm();
        string requestBody=JsonUtility.ToJson(stt);
        Debug.Log("[SpeechToText] request body json: "+requestBody);

       
        using (UnityWebRequest uwr=UnityWebRequest.Post(requestURI,UnityWebRequest.kHttpVerbPOST)){
            uwr.uploadHandler=new UploadHandlerRaw(Encoding.UTF8.GetBytes(requestBody));
            yield return uwr.SendWebRequest();

            if(uwr.result==UnityWebRequest.Result.ConnectionError || uwr.result==UnityWebRequest.Result.ProtocolError){
                Debug.Log("여기서 에러생김");
                Debug.Log(uwr.error);
            }
            else{
                string responsebody=uwr.downloadHandler.text;
                //Debug.Log("[SpeechToText] response body json: "+responsebody);
                ResponseBody sttResponse=JsonUtility.FromJson<ResponseBody>(responsebody);
                //Debug.Log("text: "+sttResponse.results[0].alternatives[0].transcript);
                string text=sttResponse.results[0].alternatives[0].transcript;
                Debug.Log("say: "+text);
                chatmanager.GetComponent<ChatManager>().Chat(true,text,"");
                GameManager.Instance.owner.GetComponent<SaySomething>().say(text);
            }
        }
        
    }
    

    // Update is called once per frame
    void Update()
    {
        if(record && !Microphone.IsRecording(Microphone.devices[0])){
            stopRecording();
            record=false;
        }
    }
    private static int WriteFileHeader (ref MemoryStream stream, int fileSize)
	{
		int count = 0;
		int total = 12;

		// riff chunk id
		byte[] riff = Encoding.ASCII.GetBytes ("RIFF");
		count += WriteBytesToMemoryStream (ref stream, riff, "ID");

		// riff chunk size
		int chunkSize = fileSize - 8; // total size - 8 for the other two fields in the header
		count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (chunkSize), "CHUNK_SIZE");

		byte[] wave = Encoding.ASCII.GetBytes ("WAVE");
		count += WriteBytesToMemoryStream (ref stream, wave, "FORMAT");

		// Validate header
		Debug.AssertFormat (count == total, "Unexpected wav descriptor byte count: {0} == {1}", count, total);

		return count;
	}

	private static int WriteFileFormat (ref MemoryStream stream, int channels, int sampleRate, UInt16 bitDepth)
	{
		int count = 0;
		int total = 24;

		byte[] id = Encoding.ASCII.GetBytes ("fmt ");
		count += WriteBytesToMemoryStream (ref stream, id, "FMT_ID");

		int subchunk1Size = 16; // 24 - 8
		count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (subchunk1Size), "SUBCHUNK_SIZE");

		UInt16 audioFormat = 1;
		count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (audioFormat), "AUDIO_FORMAT");

		UInt16 numChannels = Convert.ToUInt16 (channels);
		count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (numChannels), "CHANNELS");

		count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (sampleRate), "SAMPLE_RATE");

		int byteRate = sampleRate * channels * BytesPerSample (bitDepth);
		count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (byteRate), "BYTE_RATE");

		UInt16 blockAlign = Convert.ToUInt16 (channels * BytesPerSample (bitDepth));
		count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (blockAlign), "BLOCK_ALIGN");

		count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (bitDepth), "BITS_PER_SAMPLE");

		// Validate format
		Debug.AssertFormat (count == total, "Unexpected wav fmt byte count: {0} == {1}", count, total);

		return count;
	}

	private static int WriteFileData (ref MemoryStream stream, AudioClip audioClip, UInt16 bitDepth)
	{
		int count = 0;
		int total = 8;

		// Copy float[] data from AudioClip
		float[] data = new float[audioClip.samples * audioClip.channels];
		audioClip.GetData (data, 0);

		byte[] bytes = ConvertAudioClipDataToInt16ByteArray (data);

		byte[] id = Encoding.ASCII.GetBytes ("data");
		count += WriteBytesToMemoryStream (ref stream, id, "DATA_ID");

		int subchunk2Size = Convert.ToInt32 (audioClip.samples * BlockSize_16Bit); // BlockSize (bitDepth)
		count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (subchunk2Size), "SAMPLES");

		// Validate header
		Debug.AssertFormat (count == total, "Unexpected wav data id byte count: {0} == {1}", count, total);

		// Write bytes to stream
		count += WriteBytesToMemoryStream (ref stream, bytes, "DATA");

		// Validate audio data
		Debug.AssertFormat (bytes.Length == subchunk2Size, "Unexpected AudioClip to wav subchunk2 size: {0} == {1}", bytes.Length, subchunk2Size);

		return count;
	}

    private static byte[] ConvertAudioClipDataToInt16ByteArray (float[] data)
	{
		MemoryStream dataStream = new MemoryStream ();

		int x = sizeof(Int16);

		Int16 maxValue = Int16.MaxValue;

		int i = 0;
		while (i < data.Length) {
			dataStream.Write (BitConverter.GetBytes (Convert.ToInt16 (data [i] * maxValue)), 0, x);
			++i;
		}
		byte[] bytes = dataStream.ToArray ();

		// Validate converted bytes
		Debug.AssertFormat (data.Length * x == bytes.Length, "Unexpected float[] to Int16 to byte[] size: {0} == {1}", data.Length * x, bytes.Length);

		dataStream.Dispose ();

		return bytes;
	}
    private static int WriteBytesToMemoryStream (ref MemoryStream stream, byte[] bytes, string tag = "")
	{
		int count = bytes.Length;
		stream.Write (bytes, 0, count);
		//Debug.LogFormat ("WAV:{0} wrote {1} bytes.", tag, count);
		return count;
	}

    private static int BytesPerSample (UInt16 bitDepth)
	{
		return bitDepth / 8;
	}


    
}
