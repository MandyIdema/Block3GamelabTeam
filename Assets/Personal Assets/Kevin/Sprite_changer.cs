using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite_changer : MonoBehaviour
{
    public Sprite newSprite; 

    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            spriteRenderer.sprite = newSprite;
            StartCoroutine(ChangeBackAfterDelay(3f));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.sprite = newSprite;  
            StartCoroutine(ChangeBackAfterDelay(1f));
        }
    }

    private System.Collections.IEnumerator ChangeBackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        spriteRenderer.sprite = originalSprite; 
    }
}