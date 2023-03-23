using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Questions : NetworkBehaviour
{
    public List<string> questions = new List<string>();
    // Start is called before the first frame update
    public GameObject canvasPrefab;
    [HideInInspector] public GameObject canvas;
    public TextMeshProUGUI questionText;

    private void Awake()
    {
        canvas = canvasPrefab;
        //canvas.SetActive(false);
        
    }

    private void Start()
    {
        questions.Add("I am dutch but I hate orange. \n What is the subject of the sentence?");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Movement>() != null)
        {
            InitializeQuestion(true);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Movement>() != null)
        {
            InitializeQuestion(false);
        }
    }

    [Client]
    private void InitializeQuestion(bool activationStatus)
    {
        // Currently uses this structure to get the text object, might change later
        if (NetworkClient.active)
        {
            Debug.Log("fsgsgs");
            questionText.text = questions[0].ToString();
            canvas.SetActive(activationStatus);
        }
    }
}
