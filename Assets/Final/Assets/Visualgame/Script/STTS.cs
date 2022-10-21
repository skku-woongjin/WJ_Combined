using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class STTS : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource _audio;
    void Awake() {
        _audio=GetComponent<AudioSource>();
        _audio.mute=false;
    }
    void Start()
    {
        _audio.clip=Microphone.Start(null,false,5,44100);
        while(!(Microphone.GetPosition(null)>0)){
            
        }
        //SavWav.Save("C:/Users/82105/unity/source 폴더/voice",_audio.clip);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void play(){
        _audio.Play();
    }
}
