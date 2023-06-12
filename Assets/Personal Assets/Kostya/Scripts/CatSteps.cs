using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class CatSteps : MonoBehaviour
{
    // Start is called before the first frame update
    private bool stepTriggered;
    public float delayTime;
    public Sprite[] sprites;
    private int index = 0;
    private bool animationOngoing;

    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stepTriggered)
        {
            stepTriggered = false;
            index = 0;
            StartCoroutine(DoSteps());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!animationOngoing)
            {
                stepTriggered = true;
                animationOngoing = true;
            }
        }
    }

    IEnumerator DoSteps()
    {
        for (int j = 0; j < sprites.Length; j++)
        {
            spriteRenderer.sprite = sprites[index];
            if (index < sprites.Length - 1)
            {
                index++;
            }
            yield return new WaitForSeconds(delayTime);
        }
        animationOngoing = false;
    }
}
