using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionControl : MonoBehaviour
{
    public Transform content;
    public GameObject prefabMission;
    
    int i = 0;

   //content.getchild(0~3).gameObject -> mission1
    // private Queue<GameObject> missions;

    // Start is called before the first frame update
    public void SetMissionName(int n, string newName){

    }
    public void AddMission(string name){
        //content.GetChild(0).gameObject.GetComponentsInChildren<TMP_Text>()[1].text = name;
        GameObject newMission = Instantiate<GameObject>(prefabMission, content);
        newMission.gameObject.GetComponentsInChildren<TMP_Text>()[1].text = name;
    }
    public void DeleteMission(){
        Destroy(content.GetChild(0).gameObject);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
