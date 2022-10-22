using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MissionControl : MonoBehaviour
{
    public Transform content;
    public GameObject prefabMission;
    public int totalIndex = 0;
    
    

   //content.getchild(0~3).gameObject -> mission1
    // private Queue<GameObject> missions;

    // Start is called before the first frame update
    public void SetMissionName(int n, string newName){

    }
    public void AddMission(string name){
        //content.GetChild(0).gameObject.GetComponentsInChildren<TMP_Text>()[1].text = name;
        GameObject newMission = Instantiate<GameObject>(prefabMission, content);
        newMission.gameObject.GetComponentsInChildren<TMP_Text>()[1].text = name+totalIndex;
        ChangeMissionColor();
        totalIndex+=1;
    }
    public void DeleteMission(){
        Destroy(content.GetChild(0).gameObject);
        totalIndex = totalIndex - 1;
        ChangeMissionColor();
        
    }
    private void ChangeMissionColor(){
        
        for(int i = 0;i<=totalIndex;i++)
        {
            if(i==0){
                content.GetChild(i).gameObject.GetComponent<Image>().color = new Color(255f,255f,255f);
            }
            else content.GetChild(i).gameObject.GetComponent<Image>().color = new Color(56 / 255f, 56 / 255f, 56 / 255f);
        }
    }
    private void RearrangeMission(){
        for (int i = 0; i < totalIndex; i++)
        {
            //content.GetChild(i) = content.GetChild(i+1);
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
