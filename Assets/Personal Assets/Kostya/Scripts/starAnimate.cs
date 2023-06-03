using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GM
{
    public class starAnimate : MonoBehaviour
    {
        public float rotateTime = 1.5f;
        private float actualRotateTime;
        public bool canRotate = false;

        private void Start()
        {
            actualRotateTime = rotateTime;
        }
        private void Update()
        {
            //float lerpSpeed = 0.125f; // A random number
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Vector3.zero), lerpSpeed);
            if (canRotate)
            {
                RectTransform rectTransform = GetComponent<RectTransform>();
                rectTransform.Rotate(new Vector3(0, 0, 1));
                actualRotateTime -= Time.deltaTime;
            }

            if (actualRotateTime < 0)
            {
                actualRotateTime = rotateTime;
                canRotate = false;
            }

        }
    }

}