using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Level_Manager : MonoBehaviour
{
    // Start is called before the first frame update
   
    bool easy_button=false;
    bool hard_button=false;
    public Button easy;
    public Button hard;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //프로그램종료
    public void Exit(){
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying=false;
        #else
        Application.Quit();
        #endif

    }

    //돌아가는 버튼누를 경우 easybutton,hardbutton 눌렀던 기록 초기화
    public void Return(){
        easy_button=false;
        hard_button=false;
    }
    
    public void Easy(){
        Debug.Log("Easy");
    }


    //Level 선택
    public void click_button(){
        string level=EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("level: "+level);
        if(level.Equals("Easy")){
            //Debug.Log("easy");
            PlayerPrefs.SetInt("level",0);
            easy_button=true;
            //easy.image.color=Color.green;
            //hard.image.color=Color.white;
            
        }
        else if(level.Equals("Hard")){
            //Debug.Log("hard");
            PlayerPrefs.SetInt("level",1);
            hard_button=true;
            //easy.image.color=Color.white;
            //hard.image.color=Color.green;
            
        }
        transform.GetComponent<first_scene>().scene_change();

        

        
    }

    //1인용게임 시작
    public void Play_Solo(){
        if(easy_button || hard_button){
            transform.GetComponent<first_scene>().scene_change();
        }
        else{
            Debug.Log("Select Level");
        }
    }
}
