using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sprite_changer_planets : MonoBehaviour
{

    public Sprite newSprite;

    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite;




    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;

    }

    private void Update()
    {
      
    }

    public void UpdatePlanet()
    {
        spriteRenderer.sprite = newSprite;
        StartCoroutine(ChangeBackAfterDelay(1f));
        
    }

    private System.Collections.IEnumerator ChangeBackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        spriteRenderer.sprite = originalSprite;
        
    }
}
