using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public class Referencer : MonoBehaviour
    {

        public GameObject MainMenuBackground;
        public GameObject MainMenuPanelButtons;
        public GameObject ExitMenuPanel;
        public GameObject EndGamePanelButtons;
        public GameObject UsernameInputField;
        public GameObject UsernameInputText;

        void Start(){
            Debug.Log(MainMenuPanelButtons.transform.GetChild(4).name);
        }

    }
}
