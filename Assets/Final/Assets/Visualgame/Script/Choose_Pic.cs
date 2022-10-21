using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using System;


public class Choose_Pic : MonoBehaviour
{
    // Start is called before the first frame update


    public TMP_Text description;
    public int REALAns;  //정답

    int curAns; //내가 고른 답


    public int count; //chatmanager에서 쓰는 count
    public bool checkAns;

    public Transform chatParent;

    public ChatManager chatmanager;

    public Transform hearts;

    public int wrongCount = 0;
    int i = 0;

    public Image large_img;

    public GameObject Back_Blur;

    public GameObject Score;
    public GameObject round;
    int round_num = 1;


    [System.Serializable]
    public class Game
    {//보내기 위한 질문
        public string level;
        public int combo;
    }

    int level;
    void Start()
    {
        //Debug.Log("start 부름");

        //Combo_script=comboUI.GetComponent<Combo>();

        //O_Btn.interactable=false;

        level = PlayerPrefs.GetInt("level");
        //Debug.Log("Game level: "+level);
        if (level == 0)
        {
            Debug.Log("easy");
        }
        else if (level == 1)
        {
            Debug.Log("hard");
        }
        //**Easy,Hard 둘중에 하나 띄우기
        //Debug.Log("getimgs 실행");
        //getImgs();

    }

    public void print_caption()
    {
        //Debug.Log("caption: "+caption);

        count = 0;
        description.text = caption;
        //Debug.Log("description: "+description.text);
        Canvas.ForceUpdateCanvases();
        //description.transform.parent.GetComponent<VerticalLayoutGroup>().enabled=false;
        //description.transform.parent.GetComponent<VerticalLayoutGroup>().enabled=true;


    }

    // Update is called once per frame
    void Update()
    {
        if (staging)
        {

            TotalSeconds -= Time.deltaTime;
            TimeSpan timespan = TimeSpan.FromSeconds(TotalSeconds);
            timer = string.Format("{0:00}:{1:00}", timespan.Minutes, timespan.Seconds);

            if (TotalSeconds <= 0)
            {
                timer = @"00:00";
                TotalSeconds = 0;
                //게임종료시 이벤트
                game_over.SetActive(true);
            }
            //Debug.Log("Timer: "+timer);
        }

        if (timeText)
        {
            timeText.text = timer;
            //timeText.transform.Translate(-Time.deltaTime*(float)(2.0),0,0);
        }
    }

    #region getimg

    //한 stage당 시간제한
    public float TotalSeconds = 4 * 60;


    public string timer;
    private bool staging = false;//stage에서 게임진행중인지
    public GameObject time_slide;

    public TMP_Text timeText; //Time 표시할 text
    public void getImgs()
    {
        //Debug.Log("get_image 부름");
        TotalSeconds = 4 * 60;
        staging = true;
        round.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Round " + round_num.ToString();

        time_slide.transform.GetComponent<Slider>().value = 240;
        time_slide.transform.GetComponent<TimeSlider>().staging = true;
        StartCoroutine(GetRequest_reload("http://13.124.152.251:5000/reload"));
        Debug.Log("reload");
        StartCoroutine(GetRequest("http://13.124.152.251:5000/caption"));



    }



    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {


            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("webrequest.downloadhandler.text: " + webRequest.downloadHandler.text);
                    decode(webRequest.downloadHandler.text);
                    break;
            }
            webRequest.Dispose();
        }
    }

    IEnumerator GetRequest_reload(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            //level 지정
            Game G = new Game();
            if (level == 0)
            {
                G.level = "Easy";
            }
            else if (level == 1)
            {
                G.level = "Hard";
            }
            G.combo = combo;
            string question_data = JsonUtility.ToJson(G);
            var req = new UnityWebRequest(uri, "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(question_data);
            req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            // Request and wait for the desired page.
            yield return req.SendWebRequest();


            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("reload: Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("reload: HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    //Debug.Log("reload: " + webRequest.downloadHandler.text);
                    break;
            }
            webRequest.Dispose();
        }

    }

    public string caption;

    // Start is called before the first frame update
    void decode(string str)
    {
        Regex regex = new Regex(@"^[\w/\:.-]+;base64,");
        string[] sep = str.Split('*');
        REALAns = Int32.Parse(sep[0]);

        caption = sep[1];
        Debug.Log("caption: " + caption);
        print_caption();
        str = sep[2];
        str = regex.Replace(str, " ");
        str = str.Replace("b'", "");
        str = str.Replace("'", "");
        string[] imgs = str.Split('\n', ' ', '\r');

        int i = 0;

        foreach (string img in imgs)
        {
            if (img.Length < 3)
                continue;
            byte[] imageBytes = Convert.FromBase64String(img);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imageBytes);
            GetComponent<Pictures>().pics[i] = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            i++;
        }
        Shuffle(GetComponent<Pictures>().pics);
        GetComponent<Pictures>().setPics();
    }
    private System.Random _random = new System.Random();
    void Shuffle(Sprite[] array)
    {
        int p = array.Length;
        for (int n = p - 1; n > 0; n--)
        {
            //swap r and n
            int r = _random.Next(0, n);
            Sprite t = array[r];
            array[r] = array[n];
            array[n] = t;
            if (r == REALAns)
            {
                REALAns = n;
            }
            else if (n == REALAns)
            {
                REALAns = r;
            }
        }
        Debug.Log("RealAns: " + REALAns);
    }

    #endregion



    public void can_press(int idx)
    {
        /*transform.GetChild(curAns).GetChild(1).gameObject.SetActive(false);
        curAns = idx;
        if (btn.interactable == false)
        {
            btn.interactable = true;
           
        }*/
        curAns = idx;
        Debug.Log(curAns);
        //large_img.GetComponent<Image>().sprite=transform.GetChild(curAns).GetComponent<Image>().sprite;
        Back_Blur.transform.GetChild(0).GetComponent<Image>().sprite = transform.GetChild(curAns).GetComponent<Image>().sprite;
        Back_Blur.SetActive(true);
    }
    public int score = 0;
    public int combo = 1;
    int correct_one;
    //Combo Combo_script; //combo script
    //public GameObject comboUI;//ComboUI
    //public GameObject combo_Background;
    public GameObject game_over;
    public void check_answer(int idx)
    {

        Debug.Log("RealAns: " + REALAns);

        //정답일경우
        if (idx == REALAns)
        {
            Debug.Log("정답이야~");
            combo++; // combo 올려주기
            StartCoroutine(ReloadScene());
            round_num++;
            if (combo < 12)
            {
                score += (combo - 1) * 10;
            }

            else
            {
                score += 100;
            }

            //comboUI.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text=combo.ToString()+"combo";
            Debug.Log("score 오름");
            //comboUI.SetActive(true);
            //combo_Background.SetActive(true);
            Debug.Log("comboUI");
            //Combo_script.combo_ani();
            //comboUI.SetActive(false);
            //combo_Background.SetActive(false);
            //Debug.Log("comboUI 사라짐");
            Debug.Log("score: " + score);
            correct_one = idx;
            Invoke("waitforscoring", 1f);

        }
        //정답이 아닐경우
        else
        {
            Debug.Log("틀렸어~");
            combo = 1;

            //하트 깎이기
            if (hearts.GetChild(wrongCount).gameObject.activeSelf)
            {
                {
                    hearts.GetChild(wrongCount).gameObject.SetActive(false);
                    wrongCount++;
                    if (wrongCount == 3)
                    {
                        //chatmanager.Chat(false, "Game Over", "타인");
                        game_over.SetActive(true);
                        staging = false;
                        return;
                    }
                }
            }
            transform.GetChild(idx).GetChild(1).gameObject.SetActive(false);
            transform.GetChild(idx).GetChild(2).gameObject.SetActive(false);



        }


    }
    public void waitforscoring()
    {
        Debug.Log("1초 기다립니다");

        Score.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }


    //정답일거같은 이미지 pick
    /*public void choose()
    {
        //정답맞는지 확인
        if (answer.text.Equals("이게 정답 맞지??"))
        {
            chatmanager.Chat(true, "이게 정답 맞지??", "나");
            if (curAns == REALAns)
            {
                chatmanager.Chat(false, "YES!!!!!", "타인");

                //다음 라운드로 이동(새로운 랜덤이미지들)
                StartCoroutine(ReloadScene());
            }
            else
            {
                chatmanager.Chat(false, "No", "타인");
                if (hearts.GetChild(wrongCount).gameObject.activeSelf)
                {
                    hearts.GetChild(wrongCount).gameObject.SetActive(false);
                    wrongCount++;
                    if (wrongCount == 3)
                    {
                        chatmanager.Chat(false, "Game Over", "타인");
                        SceneManager.LoadScene(0);
                        return;
                    }
                }
                //틀렸을 떄 X로 바꾸기
                transform.GetChild(curAns).transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(curAns).GetChild(0).gameObject.SetActive(true);
            }

            btn.interactable = true;
            answer.text = "정답";
            checkAns = false;
            i = 1;
        }
    }*/

    IEnumerator ReloadScene()
    {
        staging = false;//한 stage 끝남
        time_slide.GetComponent<TimeSlider>().staging = false;

        yield return new WaitForSecondsRealtime(1);
        foreach (Transform child in chatParent)
        {
            Destroy(child.gameObject);
            //transform.GetChild(curAns).transform.GetChild(1).gameObject.SetActive(false);

        }
        foreach (Transform child in transform)
        {
            child.GetChild(1).gameObject.SetActive(false);
            child.GetChild(2).gameObject.SetActive(false);
            child.GetChild(3).gameObject.SetActive(false);
            child.GetChild(4).gameObject.SetActive(false);
        }
        getImgs();
    }



}

