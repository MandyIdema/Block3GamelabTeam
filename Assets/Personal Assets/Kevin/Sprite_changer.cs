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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.sprite = newSprite;  
            StartCoroutine(ChangeBackAfterDelay(3f));
        }
    }

    private System.Collections.IEnumerator ChangeBackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        spriteRenderer.sprite = originalSprite; 
    }
}