using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeSlider : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider time_slide;
    public bool staging;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(staging){
            
            time_slide.value-=Time.deltaTime;
        }
        //게임종료
        if(time_slide.value<=0){
            transform.Find("Fill Area").gameObject.SetActive(false);
            SceneManager.LoadScene("Game Over");
        }
        else{
            transform.Find("Fill Area").gameObject.SetActive(true);
        }
    }
}
