using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomainInformation : MonoBehaviour
{

    public string[] domainDescription;
    public GameObject canvasPrefab;
    [HideInInspector] public GameObject canvas;

    private void Awake()
    {
        canvas = Instantiate(canvasPrefab);
        var child1 = canvas.transform.GetChild(0).gameObject;
        child1.SetActive(false);
        var child2 = canvas.transform.GetChild(1).gameObject;
        child2.SetActive(false);
    }

    private void Start()
    {

    }
}
