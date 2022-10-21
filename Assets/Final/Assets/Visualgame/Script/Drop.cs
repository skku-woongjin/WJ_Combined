using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drop : MonoBehaviour,IDropHandler
{
    // Start is called before the first frame update
     
    public GameObject container;
    public GameObject O;
    public GameObject X;
    public GameObject pic;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void OnDrop(PointerEventData eventData){
        

        int idx;
        idx=transform.GetSiblingIndex();
        Debug.Log("idx: "+idx);
        if(container.GetComponent<Image>().sprite==O.transform.GetChild(0).GetComponent<Image>().sprite){
            Debug.Log("it is O");
            transform.GetChild(1).gameObject.SetActive(true);//O표시를 active시킴
            transform.GetChild(2).gameObject.SetActive(true);//O표시를 active시킴
            pic.GetComponent<Choose_Pic>().check_answer(idx);
        }
        else{
            transform.GetChild(3).gameObject.SetActive(true);//O표시를 active시킴
            transform.GetChild(4).gameObject.SetActive(true);//O표시를 active시킴
        }

        

        //맞으면 reload, 틀리면 틀렸다고 말해주기-> choose() 함수불러들이기
        
        
}
}
