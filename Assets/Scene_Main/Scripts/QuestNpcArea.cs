using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class QuestNpcArea : MonoBehaviour
{
    bool check3Second = true;
    bool checkIN = false;
    public GameObject questMakeBtn;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator checkWait()
    {
        yield return new WaitForSeconds(3);
        check3Second = true;
    }
    public void questByQnpc()
    {
        questMakeBtn.SetActive(false);
        GameManager.Instance.questGiver = transform.parent.GetComponent<SaySomething>();
        //TODO 추천하기 
        // GameManager.Instance.recAgent.recommend();
        StartCoroutine(checkWait());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && check3Second && checkIN == false)
        {
            questMakeBtn.SetActive(true);
            checkIN = true;
            check3Second = false;
            transform.parent.GetComponent<SaySomething>().say("안녕! 난 퀘스트 봇!");
            transform.parent.rotation = Quaternion.LookRotation(other.transform.GetComponentInChildren<Camera>().transform.position - transform.parent.position);


        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && check3Second && checkIN == true)

        {
            questMakeBtn.SetActive(true);
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
