using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InactivateRule : MonoBehaviour
{
    public GameObject prompt;

    private void Awake()
    {
        gameObject.SetActive(true);
    }
    private void Start()
    {
        Invoke(nameof(ActivatePrompt), 2.0f);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            gameObject.SetActive(false);
        }
        if (prompt.activeSelf)
        {
            prompt.transform.GetComponentInChildren<TextMeshProUGUI>().canvasRenderer.SetAlpha(Mathf.Abs((float)Mathf.Cos(Time.time)));
            prompt.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Abs((float)Mathf.Cos(Time.time)));
        }
    }

    void ActivatePrompt()
    {
        prompt.SetActive(true);
    }
}

