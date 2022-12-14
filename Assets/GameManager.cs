using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public Flag[] flags;
    public Flag[] flagFitness;

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

    public GameObject RecViewObj; // UI on off 용도
    public TMP_Text Type1; //새로운 곳 탐험 추천 결과
    public TMP_Text Type2; //늘 가던곳 추천
    public TMP_Text Type3; //오픈런형 추천 결과
    public int chkRecView = 0; //UI on off 용
    public int[] RecVisited; // 각각 방문 횟수 저장
    //인덱스 번호 : "쥐라기 파크" - 0, "거북선" - 1, "게임기"-2,
    //"자동차"-3, "교실"-4, "도서관"-5, "놀이터"-6, "태양계"-7 
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
        //TODO 추천하기 
        // GameManager.Instance.recAgent.recommend();
    }

    int flagCount = 8;
    private void Awake()
    {
        gm = this;
        destinations = destParent.GetComponentsInChildren<Transform>();
        flags = new Flag[flagCount];
        for (int i = 0; i < flagCount; i++)
        {
            flags[i] = new Flag();
        }
    }
    public void OffRecView()
    {
        if (chkRecView == 0)
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
