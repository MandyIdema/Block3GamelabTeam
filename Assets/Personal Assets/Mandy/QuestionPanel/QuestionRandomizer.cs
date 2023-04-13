using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionRandomizer : MonoBehaviour
{
    public GameObject[] QuestionList;
    public int RandomObjectSetActive;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            RandomObjectSetActive = Random.Range(0, QuestionList.Length);

            QuestionList[RandomObjectSetActive].SetActive(true);
        }
    }
}
