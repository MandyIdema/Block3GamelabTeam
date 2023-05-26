using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeScene : MonoBehaviour
{
   public string sceneName;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //Loads new scene named when "space" is pressed on the keeyboard
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    public void ChangeScene() // changes scene when ""overslaan " button is pressed. 
    {
        SceneManager.LoadScene(sceneName);
    }
}