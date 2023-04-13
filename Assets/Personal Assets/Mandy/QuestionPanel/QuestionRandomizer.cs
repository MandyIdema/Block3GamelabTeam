using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionRandomizer : MonoBehaviour
{
    public List<GameObject> QuestionList = new List<GameObject>();
    public int RandomObjectSetActive;
    public static bool isActive;

    private void Start()
    {
        isActive = false;

        foreach (GameObject question in GameObject.FindGameObjectsWithTag("Question"))
        {

            QuestionList.Add(question);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isActive)
        {

        
        if (collision.gameObject.tag == "Player")
        {
            RandomObjectSetActive = Random.Range(0, QuestionList.Count);
          
            QuestionList[RandomObjectSetActive].SetActive(true);
            QuestionList.Remove(QuestionList[RandomObjectSetActive]);
                isActive = true;
        }

        }

    }
}
