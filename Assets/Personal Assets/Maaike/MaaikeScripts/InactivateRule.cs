using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InactivateRule : MonoBehaviour
{
    public GameObject prompt;
    private void Start()
    {
        gameObject.SetActive(true);
        Invoke(nameof(ActivatePrompt), 3.0f);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            gameObject.SetActive(false);
        }
    }

    void ActivatePrompt()
    {
        prompt.SetActive(true);
    }
}

