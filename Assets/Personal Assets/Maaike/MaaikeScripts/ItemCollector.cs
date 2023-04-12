using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCollector : MonoBehaviour
{
        private int stars = 0;
        [SerializeField] private Text starsText;
    public TMP_Text starCount;
    public float countStars;


    private void Start()
    {
        countStars = 0;
        starCount.text = countStars.ToString();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Star")
        {
            Destroy(collision.gameObject);
            stars++;
            starCount.text = countStars.ToString();
            countStars = countStars + 1;
            Debug.Log("collided");
        }
    }
}
