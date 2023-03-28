using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Domains")]
    [SyncVar] public bool onDomain = false;
    [SyncVar] public int finalDomain = 0;
    public GameObject currentDomain;
    [HideInInspector] public int currentDomainNumber;
    [HideInInspector] public bool showMenu = true;
    [HideInInspector] public List<GameObject> domainObjects = new();

    [Space]
    public TextMesh playerNameText;
    public GameObject floatingInfo;

    [Space]
    private Camera _camera;

    private Material playerMaterialClone;

    float speed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
           if (isLocalPlayer)
        {
            Local = this;
            _camera = Camera.main;
        }
    }

    private void Update()
    {

        // if (!isLocalPlayer) return;


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
        playerNameText.text = playerName;
    }
    void OnColorChanged(Color _Old, Color _New)
    {
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
        string name = "Player" + Random.Range(100, 999);
        Color color = new Color(1, 1, 1, 1/*Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)*/);
        CmdSetupPlayer(name, color);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Domain"))
        {
            //if (isLocalPlayer)
            //{
                for (int i = 0; i < domainObjects.Count; i++)
                {
                    if (collision.gameObject == domainObjects[i])
                    {
                        if (domainObjects.Contains(collision.gameObject))
                        {
                            currentDomainNumber = i;
                            currentDomain = collision.gameObject;
                        }
                    }
                }
            //}
            onDomain = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Domain"))
        {
            //if (isLocalPlayer)
            //{
                currentDomainNumber = 0;
                currentDomain = null;
            //}
            onDomain = false;
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

    [ClientRpc]
    public void LocalStuff()
    {
        OnColorChanged(Color.white, currentDomain.GetComponent<SpriteRenderer>().color);
    }
}

