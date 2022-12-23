using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class picButton : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Back_Blur;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void clicked(){
        transform.parent.GetComponent<Choose_Pic>().can_press(transform.GetSiblingIndex());
    }

   

    public void X_Clicked(){
        Back_Blur.SetActive(false);
    }
}
