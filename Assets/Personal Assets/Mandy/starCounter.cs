using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class starCounter : MonoBehaviour
{
    //----- STARS ----

    public TMP_Text starCount;
    public float countStars;


    //----- STARS ----



    // Start is called before the first frame update
    void Start()
    {
        countStars = 0;
        starCount.text = countStars.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0, 0, 50 * Time.deltaTime); //rotates 50 degrees per second around z axis
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            countStars = countStars + 1;
            Destroy(this.gameObject);
            starCount.text = countStars.ToString();

            Debug.Log("touched star");
        }
    }
}
