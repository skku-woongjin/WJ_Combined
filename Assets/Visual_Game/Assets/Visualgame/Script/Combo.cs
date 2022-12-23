using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator; //combo animator
    public GameObject combo_background; //combo_Background
    void Start()
    {
        animator=GetComponent<Animator>();
       
    }

    public void combo_ani(){
        StartCoroutine(load_combo());
    }

    public IEnumerator load_combo(){
        animator.SetTrigger("combo_close");
        yield return new WaitForSeconds(1.0f);
        
        animator.ResetTrigger("combo_close");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
