using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    public GameObject setting_background;
    void Start()
    {
        animator=GetComponent<Animator>();
    }

    // Update is called once per frame
    public void yes(){
        StartCoroutine(GoHome());
    }
    public void No(){
        StartCoroutine(CloseAfterDelay());
    }

    public IEnumerator GoHome(){
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Level");
    }
    public IEnumerator CloseAfterDelay(){
        animator.SetTrigger("close");
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
        setting_background.SetActive(false);
        animator.ResetTrigger("close");
    }
}
