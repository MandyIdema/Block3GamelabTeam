using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEditor;



public class TeleportationScript : NetworkBehaviour
{
    [Header("Teleportation")]
    public GameObject teleportationDestination;
    private GameObject localPlayer;
    private bool teleported = false;

    [Space]

    [Header("Door Destruction")]
    public float destructionTime = 3.0f;

    [Space]

    public bool destroyAnotherDoor = false;
    [HideInInspector] public Object otherDoor;

#if UNITY_EDITOR
    // [K] This part allows you to drag the door to be destroyed ONLY if you need it
    // It's mostly for portfolio experimentation
    [CustomEditor(typeof(TeleportationScript))]
    public class MyScriptEditor: Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            TeleportationScript script = (TeleportationScript)target;

            if (script.destroyAnotherDoor)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("Chosen Door", GUILayout.MaxWidth(120));

                script.otherDoor = EditorGUILayout.ObjectField(script.otherDoor, typeof(GameObject), true);

                EditorGUILayout.EndHorizontal();
            }
        }
    }
#endif
    void Start()
    {
        localPlayer = NetworkClient.localPlayer.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(QuestionScript.QuestionAwnsered && GetComponent<QuestionRandomizer>().enteredDoors && !teleported){
            Teleport(localPlayer);
            // QuestionScript.isEnabled = false;
        }
    }

/*     private void OnCollisionEnter2D(Collision2D collision)
    {
        if (localPlayer)
        {
            if (QuestionScript.QuestionAwnsered)
            {
                if (collision.gameObject == localPlayer)
                {
                    localPlayer.transform.position = teleportationDestination.transform.position;
                    DestroyObject();
                }
            }
        }

    } */

    private void Teleport(GameObject _player){
        if (_player == localPlayer)
        {
            if (QuestionScript.QuestionAwnsered)
            {
                teleported = true;
                localPlayer.transform.position = teleportationDestination.transform.position;
                DestroyObject();
            }
        } 
    }

    // This allows to disable the door for everyone
    [Command(requiresAuthority = false)]
    void DestroyObject()
    {
        if (destroyAnotherDoor)
        {
            Destroy(otherDoor, destructionTime);
        }
        Destroy(gameObject, destructionTime);
    }
}
