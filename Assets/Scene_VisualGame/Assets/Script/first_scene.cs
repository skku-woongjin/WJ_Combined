using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class first_scene : MonoBehaviour
{
    // Start is called before the first frame update
    public Button easy;
    public Button hard;
    public TMP_InputField nickname;
    public GameObject singleUI;
    public GameObject multiUI;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void scene_change()
    {
        SceneManager.LoadScene(2); //Solo_Game

    }
    public void go_main()
    {
        SceneManager.LoadScene(1); //Level Scene
    }
    public void enter_1play()
    {
        if (nickname.text != "")
        {
            PlayerSetting.nickname = nickname.text;
            gameObject.SetActive(false);
            singleUI.SetActive(true);
            easy.image.color = Color.white;
            hard.image.color = Color.white;
        }
        else
        {
            nickname.GetComponent<Animator>().SetTrigger("on");
        }

    }
    public void enter_2play()
    {
        if (nickname.text != "")
        {
            PlayerSetting.nickname = nickname.text;
            gameObject.SetActive(false);
            multiUI.SetActive(true);

        }
        else
        {
            nickname.GetComponent<Animator>().SetTrigger("on");
        }

    }
}
