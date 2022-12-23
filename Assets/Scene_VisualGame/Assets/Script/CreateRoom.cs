using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mirror;

public class CreateRoom : MonoBehaviour
{
    // Start is called before the first frame update
    public Button easy;
    public Button hard;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void click_button(){
        string selected=EventSystem.current.currentSelectedGameObject.name;
        if(selected=="Easy"){
            easy.image.color=Color.green;
            hard.image.color=Color.white;
        }
        else if(selected=="Hard"){
            hard.image.color=Color.green;
            easy.image.color=Color.white;
        }

    }

    //방만들때 버튼초기화
    public void reset_button(){
        easy.image.color=Color.white;
        hard.image.color=Color.white;
    }
    public void Create_Room(){
        var manager=VisualRoomManager.singleton;
        manager.StartHost();
    }
}

    