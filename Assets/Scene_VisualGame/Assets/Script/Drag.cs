using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IDragHandler,IBeginDragHandler,IEndDragHandler
{

    
 
    public GameObject picture;

    bool isDragging=false;

    public GameObject container;
    
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //드래그시작
    public void OnBeginDrag(PointerEventData eventData){
        Debug.Log("Start Dragging");
        

        //Activate Container
        container.SetActive(true);
        //set Data
        
        
        container.GetComponent<Image>().sprite=transform.GetChild(0).GetComponent<Image>().sprite;
        isDragging=true;
        
    }

    //드래그중
    public void OnDrag(PointerEventData eventData){
        Debug.Log("Draging");
        if(isDragging){
            container.transform.position=eventData.position;
        }
        
    }

    //드래그끝
    public void OnEndDrag(PointerEventData eventData){
        Debug.Log("EndDrag");

        
        
        isDragging=false;
        container.GetComponent<Image>().sprite=null;
        container.SetActive(false);
        //transform.GetComponent<Image>().raycastTarget=true;
        
        
    }

    
}
