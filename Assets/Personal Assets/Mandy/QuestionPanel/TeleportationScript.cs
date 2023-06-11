using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEditor;



public class TeleportationScript : NetworkBehaviour
{
    [Header("Teleportation")]
    public GameObject teleportationDestination;
    public enum puzzleStatus
    {
        Unsolved,
        Solved
    }
    [SyncVar] public puzzleStatus currentPuzzleStatus;
    [SyncVar] public GameObject solverUser;
    private GameObject localPlayer;

    public GameObject disappearanceCloud;
    [SyncVar] private GameObject spawnedCloud;
    public QuestionBackground questionBackground;

    [Space]

    [Header("Door Destruction")]
    public float destructionTime = 3.0f;

    [Space]

    public bool destroyAnotherDoor = false;
    [HideInInspector] public Object otherDoor;
    [HideInInspector] public GameObject otherDoorConvert;

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
        if (otherDoor != null)
        {
            otherDoorConvert = (GameObject)otherDoor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (QuestionScript.QuestionAwnsered && GetComponent<QuestionRandomizer>().enteredDoors /*&& !teleported*/)
        {
            if (currentPuzzleStatus == puzzleStatus.Unsolved && 
                (otherDoorConvert == null || otherDoorConvert.GetComponent<TeleportationScript>().currentPuzzleStatus == puzzleStatus.Unsolved))
            {
                solverUser = localPlayer;
                currentPuzzleStatus = puzzleStatus.Solved;
                Teleport(localPlayer);
            }
        }
    }
    private void Teleport(GameObject _player){
        if (_player == localPlayer)
        {
            if (QuestionScript.QuestionAwnsered)
            {
                if (isClient)
                {
                    TeleportPlayer(_player);
                }
                DestroyObject();
            }
        } 
    }

    [Client]
    void TeleportPlayer(GameObject teleportedPlayer)
    {
        CmdTeleportPlayer(teleportedPlayer);
    }
    // This allows to disable the door for everyone
    [Command(requiresAuthority = false)]
    void DestroyObject()
    {
        //teleported = true;

        if (destroyAnotherDoor)
        {
            Destroy(otherDoorConvert, destructionTime);
        }
        Destroy(gameObject, destructionTime);
    }

    [Command(requiresAuthority = false)]
    void CmdTeleportPlayer(GameObject teleportedPlayer)
    {
        RpcTeleportPlayer(teleportedPlayer);
    }

    [ClientRpc]
    void RpcTeleportPlayer(GameObject teleportedPlayer)
    {
        spawnedCloud = Instantiate(disappearanceCloud, teleportedPlayer.transform.position, teleportedPlayer.transform.rotation);
        // NetworkServer.Spawn(spawnedCloud);
        if (localPlayer == teleportedPlayer)
        {
            questionBackground.WindowAnimation();
        }
        teleportedPlayer.transform.position = teleportationDestination.transform.position;
    }

}
