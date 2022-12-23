using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RoomPlayer : NetworkRoomPlayer
{
    //Game Room 에서 상호작용하기 위한 prefab
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {

    }
    new void Start()
    {
        base.Start();
        if (isServer)
        {
            Waiting_RoomUI.instance.ActiveStartButton();
        }
    }
}
