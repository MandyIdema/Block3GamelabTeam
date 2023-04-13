using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionScript : MonoBehaviour
{
    //The text for feedback
    public TMP_Text awnserText;


    private void Start()
    {
        //The text will be disabled if nothing is clicked yet
        awnserText.enabled = false;
    }

    public void wrongAwnser()
    {
        //If the awnser is wrong, state in the feedback that it is wrong
        awnserText.enabled = true;
        awnserText.GetComponent<TextMeshProUGUI>().text = "Fout! probeer het nog een keer";
        Debug.Log("This is the wrong awnser");
    }

    public void rightAwnser()
    {
        //If the awnser is right, state in the feedback that it is right and close the panel
        awnserText.enabled = true;
        awnserText.GetComponent<TextMeshProUGUI>().text = "Goed!";
        Debug.Log("This is the right awnser");
        StartCoroutine(closeWindow());
    }

    IEnumerator closeWindow()
    {
        //After two seconds, close the panel
        Debug.Log("Closed window");

        yield return new WaitForSeconds(2);
        this.gameObject.SetActive(false);
    }



}
