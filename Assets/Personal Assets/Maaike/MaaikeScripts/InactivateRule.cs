using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InactivateRule : MonoBehaviour
{
    public GameObject RulebookCanvas;

    public Button overslaanButton;

    private void Start()
    {
        RulebookCanvas.SetActive(true);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            RulebookCanvas.SetActive(false);
        }
    }

    public void newMethods()
    {
        Debug.Log("ButtonWorks");
        RulebookCanvas.SetActive(false);
    }
    //*{
    //if (Input.GetKeyDown(KeyCode.Space))
    // {
    //     Rulebook_Canvas.Enabled = !Rulebook_Canvas.Enabled;
    //}
    //}
}

