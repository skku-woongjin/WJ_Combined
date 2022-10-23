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
    public void SetMissionName(int n, string newName)
    {

    }
    public void AddMission(string name)
    {
        //content.GetChild(0).gameObject.GetComponentsInChildren<TMP_Text>()[1].text = name;
        GameObject newMission = Instantiate<GameObject>(prefabMission, content);
        newMission.gameObject.GetComponentsInChildren<TMP_Text>()[1].text = name;
        ChangeMissionColor();
        totalIndex += 1;
        transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(2000, 800 + totalIndex * 700);
    }
    public void DeleteMission()
    {
        if (content.childCount > 1)
        {
            Debug.Log("www");
            content.GetChild(1).GetComponent<Image>().color = Color.white;
        }
        Destroy(content.GetChild(0).gameObject);
        totalIndex = totalIndex - 1;
        transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(2000, 800 + totalIndex * 700);

        // ChangeMissionColor();
        // content.GetChild(0).GetComponent<Image>().color = Color.white;

    }
    private void ChangeMissionColor()
    {
        content.GetChild(0).GetComponent<Image>().color = Color.white;
        for (int i = 1; i <= totalIndex; i++)
        {
            content.GetChild(i).GetComponent<Image>().color = new Color(56 / 255f, 56 / 255f, 56 / 255f);
        }
    }
    private void RearrangeMission()
    {
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
