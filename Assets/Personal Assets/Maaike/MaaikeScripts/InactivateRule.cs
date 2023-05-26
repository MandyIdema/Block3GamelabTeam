using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InactivateRule : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            gameObject.SetActive(false);
        }
    }
}

