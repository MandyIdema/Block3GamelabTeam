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

    public void wrongAwnserWerkwoord()
    {
        //If the awnser is wrong, state in the feedback that it is wrong
        awnserText.enabled = true;
        awnserText.GetComponent<TextMeshProUGUI>().text = "Fout! Tip: Wat zijn ze aan het doen?";
        Debug.Log("This is the wrong awnser");

        Analytics.AnalyticsWrong += 1;
    }

    public void wrongAwnserLidwoord()
    {
        //If the awnser is wrong, state in the feedback that it is wrong
        awnserText.enabled = true;
        awnserText.GetComponent<TextMeshProUGUI>().text = "Fout! Tip: De man, het water, een knoop";
        Debug.Log("This is the wrong awnser");

        Analytics.AnalyticsWrong += 1;
    }

    public void wrongAwnserOnderwerp()
    {
        //If the awnser is wrong, state in the feedback that it is wrong
        awnserText.enabled = true;
        awnserText.GetComponent<TextMeshProUGUI>().text = "Fout! Tip: Wie?";
        Debug.Log("This is the wrong awnser");

        Analytics.AnalyticsWrong += 1;
    }

    public void wrongAwnserVoorzetsel()
    {
        //If the awnser is wrong, state in the feedback that it is wrong
        awnserText.enabled = true;
        awnserText.GetComponent<TextMeshProUGUI>().text = "Fout! Tip: Op de kast, voor de kast, naast de kast";
        Debug.Log("This is the wrong awnser");

        Analytics.AnalyticsWrong += 1;
    }

    public void wrongAwnserPersoonsvorm()
    {
        //If the awnser is wrong, state in the feedback that it is wrong
        awnserText.enabled = true;
        awnserText.GetComponent<TextMeshProUGUI>().text = "Fout! Tip: Maak de zin een vraag! Wat is het eerste woord?";
        Debug.Log("This is the wrong awnser");

        Analytics.AnalyticsWrong += 1;
    }

    public void wrongAwnserPersoonlijkVoorwerp()
    {
        //If the awnser is wrong, state in the feedback that it is wrong
        awnserText.enabled = true;
        awnserText.GetComponent<TextMeshProUGUI>().text = "Fout! Tip: Wie/wat + het onderwerp + werkwoord";
        Debug.Log("This is the wrong awnser");

        Analytics.AnalyticsWrong += 1;
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
            Analytics.AnalyticsRight += 1;
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
