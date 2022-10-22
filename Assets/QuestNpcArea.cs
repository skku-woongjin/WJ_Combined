using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNpcArea : MonoBehaviour
{
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
        if (other.gameObject.CompareTag("Player"))
        {
            transform.parent.GetComponent<SaySomething>().say("퀘스트가 나올거야!");
            GameManager.Instance.questGiver = transform.parent.GetComponent<SaySomething>();
            GameManager.Instance.recAgent.recommend();
            transform.parent.rotation = Quaternion.LookRotation(other.transform.GetComponentInChildren<Camera>().transform.position - transform.parent.position);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.parent.GetComponent<SaySomething>().say("퀘스트 성공!");
            GameManager.Instance.starReward += 1;
            Debug.Log(GameManager.Instance.starReward);
            transform.parent.rotation = Quaternion.LookRotation(transform.parent.position - other.transform.GetComponentInChildren<Camera>().transform.position);
        }
    }
}
