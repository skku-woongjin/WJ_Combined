using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class QuestNpcArea : MonoBehaviour
{
    bool check3Second = true;
    bool checkIN = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator checkWait(){
        yield return new WaitForSeconds(3);
        check3Second = true;
    }
    private void questByQnpc(){
        GameManager.Instance.questGiver = transform.parent.GetComponent<SaySomething>();
        GameManager.Instance.recAgent.recommend();
        StartCoroutine(checkWait());
    } 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && check3Second && checkIN == false)
        {

            checkIN = true;
            check3Second = false;
            transform.parent.GetComponent<SaySomething>().say("안녕! 나는 퀘스트 봇이야");
            transform.parent.rotation = Quaternion.LookRotation(other.transform.GetComponentInChildren<Camera>().transform.position - transform.parent.position);
            
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && check3Second && checkIN == true)

        {
            checkIN = false;
            check3Second = false;
            //transform.parent.GetComponent<SaySomething>().say("퀘스트 성공!");
            //GameManager.Instance.starReward += 1;
            //Debug.Log(GameManager.Instance.starReward);
            transform.parent.rotation = Quaternion.LookRotation(transform.parent.position - other.transform.GetComponentInChildren<Camera>().transform.position);
            StartCoroutine(checkWait());
        }
    }
}
