using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionScript : MonoBehaviour
{
    //The text for feedback
    public TMP_Text awnserText;

    // public static bool isEnabled;
    public static bool QuestionAwnsered;
    public TeleportationScript _tsObject;

    private void Start()
    {
        //The text will be disabled if nothing is clicked yet
        awnserText.enabled = false;
        // isEnabled = false;
        QuestionAwnsered = false;
    }

    private void FixedUpdate()
    {
        if (this.gameObject)
        {
           // isEnabled = true;
        }
        
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
        if (_tsObject.currentPuzzleStatus == TeleportationScript.puzzleStatus.Unsolved && 
            (_tsObject.otherDoorConvert == null || 
            _tsObject.otherDoorConvert.GetComponent<TeleportationScript>().currentPuzzleStatus 
            == TeleportationScript.puzzleStatus.Unsolved))
        {
            awnserText.GetComponent<TextMeshProUGUI>().text = "Goed!";
        }
        else
        {
            awnserText.GetComponent<TextMeshProUGUI>().text = "Goed! Maar te laat!";
        }
        Debug.Log("This is the right awnser");
        StartCoroutine(closeWindow());
    }

    IEnumerator closeWindow()
    {
        //After two seconds, close the panel
        Debug.Log("Closed window");
        QuestionRandomizer.isActive = false;
        QuestionAwnsered = true;
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }
    


}
