using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnterRoom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickEnterGameRoomButton(){
        var manager=VisualRoomManager.singleton;
        manager.StartClient();
    }
}
