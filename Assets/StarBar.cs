using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GM{
    public class StarBar : MonoBehaviour
    {

        public StarManager sm;
        public Image BarFilled;

        private float valueStars;

        private void Start()
        {
        }

        private void Update()
        {

            Debug.Log(sm);
            Debug.Log(valueStars);

            valueStars = (float)sm.starsTaken / (float)sm.starsNeeded;

            BarFilled.fillAmount = valueStars;
        }
    }
}
