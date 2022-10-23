using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
public class ConvGroup : MonoBehaviour
{
    public bool isbad;
    public Material UDangTangMat;
    public Material CommonMat;
    public GameObject Sphere;
    public SaySomething[] users;
    public string convFilename;
    public Canvas canvas;
    public int totalChat;
    public int hateChat;
    public int hatePercent;
    private Image hateBar;

    List<Dictionary<string, object>> data_Dialog;

    void Start()
    {

        canvas.worldCamera = GameManager.Instance.cam.GetComponent<Camera>();
        hateBar = GameManager.Instance.hatePercentUI.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        if (totalChat > 0)
            changeSphere();
        if (isbad)
        {
            Sphere.GetComponent<Renderer>().material = UDangTangMat;
        }
        else
        {
            Sphere.GetComponent<Renderer>().material = CommonMat;
        }
        users = new SaySomething[5];
        if (convFilename.Length > 4)
            data_Dialog = CSVReader.Read(convFilename);
        int i = 0;
        foreach (Transform u in transform)
        {
            if (u.GetComponent<SaySomething>() == null)
                break;
            users[i] = u.GetComponent<SaySomething>();
            i++;

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

        }
    }
    public void changeSphere()
    {
        hatePercent = (hateChat * 100 / totalChat);
        //Debug.Log(hateChat);
        GameManager.Instance.hateRateText.text = hatePercent.ToString() + "%";
        hateBar.fillAmount = (hatePercent + 0.0f) / 100;
        if (hatePercent < 30)
        {
            hateBar.color = Color.blue;
        }
        else if (hatePercent < 60)
        {
            hateBar.color = Color.yellow;
        }
        else
        {
            hateBar.color = Color.red;
        }

        if (hatePercent >= 60)
        {
            isbad = true;
        }
        else
        {
            isbad = false;
        }

        if (isbad)
        {
            Sphere.GetComponent<Renderer>().material = UDangTangMat;
            GetComponent<Collider>().isTrigger = false;
        }
        else
        {
            Sphere.GetComponent<Renderer>().material = CommonMat;
            GetComponent<Collider>().isTrigger = true;
        }
    }
    public void hideSphere()
    {
        StartCoroutine("hideSphereCoroutine");
    }

    IEnumerator hideSphereCoroutine()
    {
        yield return new WaitForSecondsRealtime(1);
        Sphere.SetActive(false);

    }


    public void join()
    {
        GameManager.Instance.hatePercentUI.SetActive(true);
        StartCoroutine(startConv());
    }

    IEnumerator startConv()
    {
        if (data_Dialog != null)
        {
            for (int i = 0; i < data_Dialog.Count; i++)
            {
                users[Int32.Parse(data_Dialog[i]["user"].ToString())].say(data_Dialog[i]["line"].ToString());
                yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(2, 3.5f));
            }
        }
    }

    public void clickButton()
    {
        if (isbad)
            GameManager.Instance.owner.GetComponent<JoinGroup>().warn();
        else
            GameManager.Instance.owner.GetComponent<JoinGroup>().join();
    }
}
