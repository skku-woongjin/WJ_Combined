using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talking_NPC_Area : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject TTS_Audio;
    public bool Jurassic;
    public bool Solar;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    string text;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enter area");
            if(Solar){
                text="나는 태양계로봇 솔라야. 나랑 태양계에 대해서 알아볼까?";
            }
            else if(Jurassic){
                text="나는 쥬라기로봇 다이노야. 어떤 주제에 대해 알고싶니?";
            }
            
            
            TTS_Audio.GetComponent<TTS>().setText(text);
            transform.parent.GetComponent<SaySomething>().say(text);
            
            
        }
        }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Exit area");
        }
    }
}
