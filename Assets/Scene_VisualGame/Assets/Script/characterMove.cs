using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class characterMove : NetworkBehaviour
{
    public bool isMoveable;

    [SyncVar]
    public float speed=2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate() {
        Move();
    }
    public void Move(){
        if(hasAuthority&& isMoveable){
            Vector3 dir=Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"),0f,0f),1f);
            
            //player 좌우반전
            if(dir.x<0f){
                transform.localScale=new Vector3(-0.5f,0f,0f);
            }
            else if(dir.x>0f){
                transform.localScale=new Vector3(0.5f,0f,0f);
            }
            transform.position+=dir*speed*Time.deltaTime;
        }
    }
}
