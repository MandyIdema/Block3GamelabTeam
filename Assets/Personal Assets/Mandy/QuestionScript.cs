using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionScript : MonoBehaviour
{
    public TMP_Text awnserText;


    private void Start()
    {
        awnserText.enabled = false;
    }

    public void wrongAwnser()
    {
        awnserText.enabled = true;
        awnserText.GetComponent<TextMeshProUGUI>().text = "Fout! probeer het nog een keer";
        Debug.Log("This is the wrong awnser");
    }

    public void rightAwnser()
    {
        awnserText.enabled = true;
        awnserText.GetComponent<TextMeshProUGUI>().text = "Goed!";
        Debug.Log("This is the right awnser");
        StartCoroutine(closeWindow());
    }

    IEnumerator closeWindow()
    {
        Debug.Log("Closed window");

        yield return new WaitForSeconds(2);
        this.gameObject.SetActive(false);
    }



}
