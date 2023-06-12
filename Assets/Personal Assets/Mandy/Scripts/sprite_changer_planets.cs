using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sprite_changer_planets : MonoBehaviour
{

    public Sprite newSprite;

    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite;
    private float originalY;
    public bool playerInRange;
    private float randomizer;
    private float period;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
        originalY = this.transform.position.y;
        randomizer = Random.Range(0.4f, 1.2f);
        period = Random.Range(0.4f, 1.2f);

    }

    private void Update()
    {
        transform.position = new Vector2(transform.position.x, originalY + randomizer * ((float)Mathf.Cos(Time.time * period)));
    }
}
