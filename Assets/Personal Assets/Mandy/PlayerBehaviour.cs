using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum PlayerDomain
{
    Brainstorming,
    Conceptualizing,
    Communication,
    Execution,
    Logistics,
    Leadership
}

public class PlayerBehaviour : NetworkBehaviour
{



    public static PlayerBehaviour Local;

    [Header("Main Properties")]

    private Sprite defaultSprite;
    public enum PlayerStatus
    {
        Joined,
        Ready,
        Playing
    }

    [Space]

    [Header("Domains")]
    [SyncVar] [HideInInspector] public bool onDomain = false;
    [SyncVar] public int finalDomain = 0;
    public GameObject currentDomain;
    [HideInInspector] public int currentDomainNumber;
    [HideInInspector] public bool showMenu = true;
    [HideInInspector] public List<GameObject> domainObjects = new();

    [Space]

    [Header("Player Stats")]
    [SyncVar] public PlayerStatus currentStatus = PlayerStatus.Joined;
    [SyncVar] public int starsCollected = 0;
    [SyncVar] public bool gameFinished = false;

    [Space]
    public TextMesh playerNameText;
    public GameObject floatingInfo;

    [Space]
    private Camera _camera;

    private Material playerMaterialClone;

    float speed = 0.1f;

    //===== TELEPORTATION ============

    public GameObject[] TeleportationDoor;
    public GameObject[] TeleportationDestination;

    public float DoorTouched;

    void Start()
    {
        defaultSprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
        if (isLocalPlayer)
        {
            Local = this;
            _camera = Camera.main;
        }

    }

    private void Update()
    {

        

        if (currentDomain != null)
        {
            if (currentDomain.GetComponent<DomainInformation>().currentStatus == DomainInformation.DomainStatus.Chosen)
            {
                return;
            }
        }

        if (isLocalPlayer)
        {
            if (onDomain && finalDomain == 0)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ChoosingDomain();
                }
            }
        }

            if (currentDomain != null)
        {
            if (currentDomain.GetComponent<DomainInformation>().currentStatus == DomainInformation.DomainStatus.Chosen)
            {
                Local.showMenu = false;
            }
        }

            if(finalDomain != 0)
        {
            currentStatus = PlayerStatus.Ready;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movement();
    }

    private void LateUpdate()
    {
        if (!isLocalPlayer || !_camera) return;
        _camera.transform.position = transform.position + 10 * Vector3.back;
    }

    void movement()
    {
        //Don't run the code if it is not the local player
        if(isLocalPlayer)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector2(moveHorizontal * speed, moveVertical * speed);
            this.transform.position = transform.position + movement;
        }

    }

    [SyncVar(hook = nameof(OnNameChanged))]
    public string playerName;

    [SyncVar(hook = nameof(OnColorChanged))]
    public Color playerColor = Color.white;

    void OnNameChanged(string _Old, string _New)
    {
        //Change the name of the player from the text above its head
        playerNameText.text = playerName;
    }
    void OnColorChanged(Color _Old, Color _New)
    {
        //Change the material of the player 
        playerNameText.color = _New;
        playerMaterialClone = new Material(GetComponent<Renderer>().material);
        playerMaterialClone.color = _New;
        GetComponent<Renderer>().material = playerMaterialClone;
    }

    public void CmdSetupPlayer(string _name, Color _col)
    {
        // player info sent to server, then server updates sync vars which handles it on all clients
        playerName = _name;
        playerColor = _col;
    }

    public override void OnStartLocalPlayer()
    {
        //On start of the game, choose a random color for the player to assign

        string name = "Player" + Random.Range(100, 999);
        Color color = new Color(1, 1, 1, 1/*Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)*/);
        CmdSetupPlayer(name, color);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (QuestionScript.QuestionAwnsered)
        {
   
        }
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("AvatarChoice"))
        {
            EnteringAvatarChoice(collision);
        }
        if (collision.gameObject.CompareTag("Star"))
        {
            CollectingStar(collision, collision.gameObject.GetComponent<NetworkIdentity>());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("AvatarChoice"))
        {
            ExitingAvatarChoice(collision);
        }
    }

    [Command]
    public void ChoosingDomain()
    {
        if (currentDomain != null)
        {
            finalDomain = currentDomainNumber;
            playerColor = currentDomain.GetComponent<SpriteRenderer>().color;
            currentDomain.GetComponent<DomainInformation>().currentStatus = DomainInformation.DomainStatus.Chosen;
            onDomain = false;
        }
    }

    public void EnteringAvatarChoice(Collider2D colAvatar)
    {
        for (int i = 0; i < domainObjects.Count; i++)
        {
            if (colAvatar.gameObject == domainObjects[i])
            {
                if (domainObjects.Contains(colAvatar.gameObject))
                {
                    currentDomainNumber = i;
                    currentDomain = colAvatar.gameObject;
                    if (finalDomain == 0)
                    {
                        this.gameObject.GetComponent<SpriteRenderer>().sprite =
                            colAvatar.gameObject.GetComponent<DomainInformation>().characterModel;
                    }
                }
            }
            onDomain = true;
        }
    }

    public void ExitingAvatarChoice(Collider2D colAvatar)
    {
        currentDomainNumber = 0;
        currentDomain = null;
        if (finalDomain == 0)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = defaultSprite;
        }
        onDomain = false;
    }

 
    public void CollectingStar(Collider2D starPrefab, NetworkIdentity item)
    {
        // item.AssignClientAuthority(connectionToClient);
        // starsCollected++;
        // This might have to be replaced into the Star Property script
    }

    public void CheckingStuff(Collider2D item)
    {
        item.gameObject.GetComponent<NetworkIdentity>().RemoveClientAuthority();
    }
}

