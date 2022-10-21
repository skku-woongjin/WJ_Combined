using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class VisualRoomManager : NetworkRoomManager
{
    //서버에서 새로운 client감지했을때 발생
    public override void OnRoomServerConnect(NetworkConnectionToClient conn)
    {
        base.OnRoomServerConnect(conn);
        var player=Instantiate(spawnPrefabs[0]);
        NetworkServer.Spawn(player,conn);
    }
}
