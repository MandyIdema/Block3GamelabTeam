using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HomeScreen : MonoBehaviour
{

    public GameObject prompt;
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.activeSelf)
        {
            Invoke(nameof(ActivatePrompt), 1.5f);
        }

    }

    // Update is called once per frame
    void Update()
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
