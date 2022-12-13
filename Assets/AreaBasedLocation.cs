using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AreaBasedLocation : MonoBehaviour
{
    public string realLocation;
    public int locID;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.locationUI.text = realLocation;
            GameManager.Instance.RecVisited[locID] += 1;
        }
        GameManager.Instance.req.CheckQuestSuccess();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.locationUI.text = "복도";
            for(int i = 0 ; i < 8 ;i++)
            {
                Debug.Log(GameManager.Instance.RecVisited[i]);
            }
        }
        GameManager.Instance.req.CheckQuestSuccess();
        GameManager.Instance.recAgent.updateFlags(locID);
    }
}
