using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuestionBackground : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float spriteDelay;
    public int index = 0;

    private Image image;
    void Start()
    {
        image = GetComponent<Image>();
    }
    public void WindowAnimation()
    {
        index = sprites.Length - 1;
        StartCoroutine(CloseWindow());
    }
    IEnumerator CloseWindow()
    {
        for (int i = index; i >= 0; i--)
        {
            image.sprite = sprites[index];
            if (index > 0)
            {
                index--;
            }
            yield return new WaitForSeconds(spriteDelay);
        }
        StartCoroutine(OpenWindow());
    }

    IEnumerator OpenWindow()
    {
        for (int j = 0; j < sprites.Length; j++)
        {
            image.sprite = sprites[index];
            if (index < sprites.Length - 1)
            {
                index++;
            }
            yield return new WaitForSeconds(spriteDelay);
        }
    }
}
