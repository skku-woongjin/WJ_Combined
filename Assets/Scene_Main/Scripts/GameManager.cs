using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
        GameManager.Instance.req.NewQuest();
    }

    public void recommend(int meanVis, float meanDist, float meanTime)
    {
        recAgent.generateLog(meanVis, meanDist, meanTime);
        recAgent.RequestDecision();
    }

    public int txtIdx;
    public int[] curQuests;

    public void nextRec()
    {
        txtIdx++;
        if (txtIdx == 1)
        {
            recAgent.targetText = Type1;
            recommend(0, recAgent.maxdist, 0);
        }
        else if (txtIdx == 2)
        {
            recAgent.targetText = Type2;
            recommend(recAgent.maxcount, 0, recAgent.maxdist);
        }

    }


    public void setCurQuest(int idx)
    {
        curQuest = curQuests[idx];
        if (idx == 0)
        {
            Type1.transform.parent.GetChild(2).gameObject.SetActive(true);
            Type2.transform.parent.GetComponent<Button>().interactable = false;
            Type3.transform.parent.GetComponent<Button>().interactable = false;
        }
        else if (idx == 1)
        {
            Type2.transform.parent.GetChild(2).gameObject.SetActive(true);
            Type1.transform.parent.GetComponent<Button>().interactable = false;
            Type3.transform.parent.GetComponent<Button>().interactable = false;
        }
        else if (idx == 2)
        {
            Type3.transform.parent.GetChild(2).gameObject.SetActive(true);
            Type2.transform.parent.GetComponent<Button>().interactable = false;
            Type1.transform.parent.GetComponent<Button>().interactable = false;
        }
        GameManager.Instance.questGiver = idleAgent.GetComponent<SaySomething>();
        GameManager.Instance.req.NewQuest(curQuest);
        GameManager.Instance.idleAgent.stopStart();
        GameManager.Instance.idleAgent.transform.rotation = Quaternion.LookRotation(-idleAgent.removY(transform.position - GameManager.Instance.owner.position));
        idleAgent.state = IdleAgent.States.say;
        GameManager.Instance.idleAgent.Invoke("lead", 5f);

    }

    int flagCount = 8;
    private void Awake()
    {
        gm = this;
        destinations = destParent.GetComponentsInChildren<Transform>();
        flags = new Flag[flagCount];
        curQuests = new int[3];
        curQuests[2] = 7;

        for (int i = 0; i < flagCount; i++)
        {
            flags[i] = new Flag();
            flags[i].id = i;
            flags[i].pos = destinations[i + 1].position;
        }
        recAgent = GetComponent<RecommendAgent>();
    }

    public void updateFlagDist()
    {
        string debug = "";
        foreach (Flag flag in flags)
        {
            flag.dist = Vector3.Distance(flag.pos, owner.transform.position);
            debug += placeNames[flag.id] + "\n";
            debug += "dist: " + flag.dist + "\n";
            debug += "posd: " + flag.pos + "\n";
            debug += "vis:" + flag.visited + "\n\n";
        }

        Debug.Log(debug);

    }

    public void OffRecView()
    {
        if (chkRecView == 0)
        {
            Type1.transform.parent.GetChild(2).gameObject.SetActive(false);
            Type2.transform.parent.GetChild(2).gameObject.SetActive(false);
            Type3.transform.parent.GetChild(2).gameObject.SetActive(false);
            Type3.transform.parent.GetComponent<Button>().interactable = true;
            Type2.transform.parent.GetComponent<Button>().interactable = true;
            Type1.transform.parent.GetComponent<Button>().interactable = true;
            RecViewObj.SetActive(true);
            txtIdx = 0;
            nextRec();
            chkRecView = 1;
        }
        else
        {
            RecViewObj.SetActive(false);
            chkRecView = 0;
        }

    }
}
