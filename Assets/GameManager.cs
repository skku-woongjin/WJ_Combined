using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public IdleAgent idleAgent;
    public Transform owner;
    public ConvGroup curGroup;
    public Transform cam;
    public request req;
    public bool ingroup;
    public GameObject okno;
    public GameObject oknoQuest;
    private static GameManager gm;
    public TMP_Text hateRateText;
    public GameObject hatePercentUI;
    public TMP_Text locationUI;
    public string Solar_response;

    public Transform destParent;

    [HideInInspector]
    public Transform[] destinations;
    public int curQuest = -1;
    public int starReward;
    public string[] placeNames = { "쥐라기 파크", "거북선", "게임기", "자동차", "교실", "도서관", "놀이터", "태양계" };
    public RecommendAgent recAgent;
    public SaySomething questGiver;
    public MissionControl missionManager;
    public GameObject QuestUI;
    public TMP_Text Coin;
    public int coinVal;
    public PlayAnims dogPlay;
    public bool userText_set;
    public string userText;
    public bool checkReach;
    public bool checkJump;

    public GameObject RecViewObj;
    public TMP_Text Type1;
    public TMP_Text Type2;
    public TMP_Text Type3;
    public int chkRecView=0;
    public int[] RecVisited;
    public static GameManager Instance
    {
        get
        {
            if (gm == null) Debug.LogError("Game Manager is null!");
            return gm;
        }
    }
    // Start is called before the first frame update

    public void petQuest()
    {
        idleAgent.GetComponent<SaySomething>().say("퀘스트가 나올거야!");
        GameManager.Instance.questGiver = idleAgent.GetComponent<SaySomething>();
        GameManager.Instance.recAgent.recommend();
    }
    private void Awake()
    {
        gm = this;
        destinations = destParent.GetComponentsInChildren<Transform>();
    }
    public void OffRecView()
    {
        if(chkRecView == 0)
        {
            RecViewObj.SetActive(true);
            chkRecView = 1;
        }
        else
        {
            RecViewObj.SetActive(false);
            chkRecView = 0;
        }
        
    }
}
