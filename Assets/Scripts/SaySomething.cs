using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaySomething : MonoBehaviour
{
    public GameObject bubble;
    Transform camTransform;
    public bool censor;
    public bool ispet;
    Image bubbleImg;
    public bool isTTS;
    public GameObject TTS_Audio;

    void Start()
    {
        camTransform = GameManager.Instance.cam;
        bubbleImg = bubble.GetComponentInChildren<Image>();
        if (GetComponentInChildren<Animator>() != null)
        {
            GetComponentInChildren<Animator>().SetFloat("ConvSpeed", Random.Range(0.2f, 1f));
        }
    }

    IEnumerator fadeCor = null;
    public void say(string line)
    {
        //Debug.Log(bubbleImg + "dsav");
        bubble.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.9f);
        if (fadeCor != null)
            StopCoroutine(fadeCor);
        fadeCor = fadeout();
        bubble.SetActive(false);
        if (!ispet && censor)
            transform.parent.GetComponent<ConvGroup>().totalChat += 1;
        if (censor)
        {
            StartCoroutine(GameManager.Instance.req.Upload((returnval) =>
            {
                if (returnval)
                {
                    transform.parent.GetComponent<ConvGroup>().hateChat += 1;
                    bubble.GetComponentInChildren<TMP_Text>().text = line;

                }
                else
                {
                    bubble.GetComponentInChildren<TMP_Text>().text = line;
                }
                if (!ispet)
                {
                    transform.parent.GetComponent<ConvGroup>().changeSphere();
                }
                StartCoroutine(rebuild(bubble.GetComponent<RectTransform>()));
                bubble.SetActive(true);
                StartCoroutine(fadeCor);
            }, line));
        }
        else
        {
            bubble.GetComponentInChildren<TMP_Text>().text = line;
            StartCoroutine(rebuild(bubble.GetComponent<RectTransform>()));
            bubble.SetActive(true);
            StartCoroutine(fadeCor);

        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(bubble.GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases();
        bubble.GetComponent<RectTransform>().GetChild(0).GetComponent<LayoutGroup>().enabled = false;
        bubble.GetComponent<RectTransform>().GetChild(0).GetComponent<LayoutGroup>().enabled = true;


    }

    IEnumerator rebuild(RectTransform obj)
    {
        yield return new WaitForEndOfFrame();
        LayoutRebuilder.ForceRebuildLayoutImmediate(obj.GetChild(0).GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases();
        obj.GetChild(0).GetComponent<LayoutGroup>().enabled = false;
        yield return new WaitForEndOfFrame();
        obj.GetChild(0).GetComponent<LayoutGroup>().enabled = true;
    }

    private void Update()
    {
        if (bubble.activeSelf)
        {
            bubble.transform.rotation = Quaternion.LookRotation(bubble.transform.position - camTransform.position);
        }
        if (isTTS)
        {

        }
    }

    IEnumerator fadeout()
    {
        if (!ispet)
        {
            Color c;
            yield return new WaitForSecondsRealtime(4f);
            while (bubbleImg.color.a > 0.01f)
            {
                c = bubbleImg.color;
                c.a -= 0.02f;
                bubbleImg.color = c;
                yield return new WaitForFixedUpdate();
            }
            bubble.SetActive(false);
            bubbleImg.color = Color.white;

        }
        else yield return null;
    }

    public IEnumerator petFadeOut()
    {

        Color c;
        yield return new WaitForSecondsRealtime(2f);
        while (bubbleImg.color.a > 0.01f)
        {
            c = bubbleImg.color;
            c.a -= 0.02f;
            bubbleImg.color = c;
            yield return new WaitForFixedUpdate();
        }
        bubble.SetActive(false);
        bubbleImg.color = Color.white;


    }

}
