using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Waiting_RoomUI : MonoBehaviour
{

    // Start is called before the first frame update
    public static Waiting_RoomUI instance;
    public Button StartButton;
    void Start()
    {
        instance=this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ActiveStartButton(){
        StartButton.gameObject.SetActive(true);
    }

    public void SetInteractableStartButton(bool isInteractable){
        StartButton.interactable=isInteractable;
    }
    public void ExitGameRoom(){
        var manager=VisualRoomManager.singleton;
        if(manager.mode==Mirror.NetworkManagerMode.Host){
            manager.StopHost();
        }
        else if(manager.mode==Mirror.NetworkManagerMode.ClientOnly){
            manager.StopClient();
        }
    }
    public void OnClickStartButton(){
        var players=FindObjectOfType<RoomPlayer>();
        
    }
}
