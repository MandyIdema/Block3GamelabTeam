using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class QuestionRandomizer : NetworkBehaviour
{
    public List<GameObject> QuestionList = new List<GameObject>();
    public int RandomObjectSetActive;
    public static bool isActive;
    public GameObject localPlayer;
    public GameObject questionPromptUI;
    public bool enteredDoors;

    private void Start()
    {
        isActive = false;
        foreach (GameObject question in GameObject.FindGameObjectsWithTag("Question"))
        {

            QuestionList.Add(question);
        }

        localPlayer = NetworkClient.localPlayer.gameObject;
    }
    public void ActivateQuestion()
    {
        if (!isActive)
        {
            RandomObjectSetActive = Random.Range(0, QuestionList.Count);
            QuestionList[RandomObjectSetActive].SetActive(true);
            QuestionList.Remove(QuestionList[RandomObjectSetActive]);
            isActive = true;
            enteredDoors = true;
        }

    }
}
