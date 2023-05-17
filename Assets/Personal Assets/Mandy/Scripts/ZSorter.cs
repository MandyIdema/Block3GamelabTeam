using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZSorter : MonoBehaviour
{
    [SerializeField]
    private int sortingOrderBase = 5000;

    [SerializeField]
    private int offset = 0;

    [SerializeField]
    private bool runOnlyOnce = false;

    private Renderer myRenderer;

    private float timer;
    private float timerMax = .1f;

    private void Awake()
    {
        myRenderer = gameObject.GetComponent<Renderer>();
    }

    private void LateUpdate()
    {
        timer  -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = timerMax;

            myRenderer.sortingOrder = (int)(sortingOrderBase - transform.position.y - offset);
            if (runOnlyOnce)
            {
                Destroy(this);
            }
        }
    }


}
