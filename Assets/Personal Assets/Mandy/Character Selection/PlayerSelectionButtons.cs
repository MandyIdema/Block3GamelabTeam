using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSelectionButtons : MonoBehaviour
{
    public GameObject Prefab1;
    public GameObject Prefab2;
    public GameObject Prefab3;
    public GameObject Prefab4;


    public SpriteRenderer spriteRenderer;
    public Sprite newSpritePlayer1;
    public Sprite newSpritePlayer2;
    public Sprite newSpritePlayer3;
    public Sprite newSpritePlayer4;

    public void ButtonPressP1()
    {
        Prefab1.SetActive(true);
        Prefab2.SetActive(false);
        Prefab3.SetActive(false);
        Prefab4.SetActive(false);

        spriteRenderer.sprite = newSpritePlayer1;
    }

    public void ButtonPressP2()
    {
        Prefab1.SetActive(false);
        Prefab2.SetActive(true);
        Prefab3.SetActive(false);
        Prefab4.SetActive(false);

        spriteRenderer.sprite = newSpritePlayer2;
    }

    public void ButtonPressP3()
    {
        Prefab1.SetActive(false);
        Prefab2.SetActive(false);
        Prefab3.SetActive(true);
        Prefab4.SetActive(false);

    }

    public void ButtonPressP4()
    {
        Prefab1.SetActive(false);
        Prefab2.SetActive(false);
        Prefab3.SetActive(false);
        Prefab4.SetActive(true);
    }




}
