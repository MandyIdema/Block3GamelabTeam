using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite_changer : MonoBehaviour
{
    public Sprite newSprite; 

    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite;

    public GameObject CurrentLight;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;

        //CurrentLight.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            spriteRenderer.sprite = newSprite;
            StartCoroutine(ChangeBackAfterDelay(3f));
            //CurrentLight.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.sprite = newSprite;  
            StartCoroutine(ChangeBackAfterDelay(1f));
            //CurrentLight.SetActive(true);
        }
    }

    private System.Collections.IEnumerator ChangeBackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        spriteRenderer.sprite = originalSprite;
        //CurrentLight.SetActive(false);
    }
}