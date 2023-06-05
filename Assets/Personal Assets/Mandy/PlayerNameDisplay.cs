using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;



public class PlayerNameDisplay : MonoBehaviour
{
    private string input;

    public void ReadStringInput(string name)
    {

            input = name;
            Debug.Log("Username set to:" + input);

    }

}


