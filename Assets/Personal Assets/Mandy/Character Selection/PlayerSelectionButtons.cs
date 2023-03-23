using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionButtons : MonoBehaviour
{
    public GameObject Prefab1;
    public GameObject Prefab2;
    public GameObject Prefab3;
    public GameObject Prefab4;

    public void ButtonPressP1()
    {
        Prefab1.SetActive(true);
        Prefab2.SetActive(false);
        Prefab3.SetActive(false);
        Prefab4.SetActive(false);
    }

    public void ButtonPressP2()
    {
        Prefab1.SetActive(false);
        Prefab2.SetActive(true);
        Prefab3.SetActive(false);
        Prefab4.SetActive(false);
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
