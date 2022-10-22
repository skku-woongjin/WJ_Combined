using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaBasedScript : MonoBehaviour
{
    List<Dictionary<string, object>> data_Dialog;
    public string[] lines;
    private void Start()
    {
        StartCoroutine(startConv());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enter area");
            foreach (Canvas bubble in transform.parent.GetComponentsInChildren<Canvas>())
            {
                bubble.gameObject.layer = 5;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Exit Area");
            foreach (Canvas bubble in transform.parent.GetComponentsInChildren<Canvas>())
            {
                bubble.gameObject.layer = 3;
            }
        }
    }

    IEnumerator startConv()
    {
        while (true)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                transform.parent.GetComponentsInChildren<SaySomething>()[i % 2].say(lines[i]);
                yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(1, 2.5f));
            }
        }
    }
}
