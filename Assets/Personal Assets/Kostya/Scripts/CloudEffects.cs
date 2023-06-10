using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudEffects : MonoBehaviour
{
    private float cloudAlpha = 0;
    private bool alphaDescending = false;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, cloudAlpha);
        GetComponent<SpriteRenderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf && !alphaDescending)
        {
            cloudAlpha += 0.005f;
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, cloudAlpha);
        }
        if (cloudAlpha >= 1)
        {
            alphaDescending = true;
        }
        if (alphaDescending && cloudAlpha >= 0)
        {
            cloudAlpha -= 0.005f;
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, cloudAlpha);
        }
        if (cloudAlpha < 0 && alphaDescending)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
