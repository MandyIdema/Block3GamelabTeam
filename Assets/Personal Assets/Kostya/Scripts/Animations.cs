using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Animations : MonoBehaviour
{

    public enum AnimationTypes
    {
        Hovering,
        Rotating,
        Fade
    }

    public AnimationTypes chosenAnimation;

    private float rotationSpeed;

    private float currentAlpha = 1;
    private bool alphaDecreasing;
    private float alphaVariation;

    private float originalY;
    private float randomYElement;
    [HideInInspector] public float intensity;
#if UNITY_EDITOR
    [CustomEditor(typeof(Animations))]
    public class AnimationsEditor: Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // Reference the variables in the script
            Animations animationScript = (Animations)target;

            if (animationScript.chosenAnimation == AnimationTypes.Hovering)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Intensity", GUILayout.MaxWidth(130));
                animationScript.intensity = EditorGUILayout.FloatField(animationScript.intensity);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
#endif
    void Start()
    {
        switch (chosenAnimation)
        {
            case AnimationTypes.Rotating:
                rotationSpeed = Random.Range(0.1f, 0.2f);
                break;
            case AnimationTypes.Fade:
                alphaVariation = Random.Range(0.00075f, 0.0015f);
                GetComponent<CanvasRenderer>().SetAlpha(currentAlpha);
                alphaDecreasing = true;
                    break;
            case AnimationTypes.Hovering:
                randomYElement = Random.Range(0.7f, 1.3f);
                this.originalY = this.transform.position.y;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (chosenAnimation)
        {
            case AnimationTypes.Rotating:
                RectTransform rectTransform = GetComponent<RectTransform>();
                rectTransform.Rotate(new Vector3(0, 0, rotationSpeed));
                break;
            case AnimationTypes.Fade:
                if (currentAlpha > 0 && currentAlpha <= 1 && alphaDecreasing)
                {
                    currentAlpha -= alphaVariation;
                }

                if (currentAlpha < 0)
                {
                    currentAlpha = 0;
                    alphaDecreasing = false;
                }
                if (!alphaDecreasing && currentAlpha <= 1)
                {
                    currentAlpha += alphaVariation;
                }
                if (currentAlpha >= 1 && !alphaDecreasing)
                {
                    alphaDecreasing = true;
                    currentAlpha = 1;
                }
                GetComponent<CanvasRenderer>().SetAlpha(currentAlpha);
                break;
            case AnimationTypes.Hovering:
                transform.position = new Vector2(transform.position.x, originalY + randomYElement *(Mathf.Abs((float)Mathf.Sin(Time.time) * intensity)));
                break;
        }
    }
}
