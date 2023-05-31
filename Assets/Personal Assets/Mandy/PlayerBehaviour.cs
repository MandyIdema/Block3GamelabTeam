using GM;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public Animator anim;

    public static PlayerBehaviour Local;

    private Sprite defaultSprite;
    public enum PlayerStatus
    {
        Joined,
        Ready,
        Playing
    }

    [Space]

    #region Domains
    [Header("Domains")]
    [HideInInspector] public bool onDomain = false;
    [HideInInspector] public int finalDomain = 0;
    [HideInInspector] public GameObject currentDomain;
    [HideInInspector] public int currentDomainNumber;
    [HideInInspector] public bool showDomainMenu = true;
    [HideInInspector] public List<GameObject> domainObjects = new();
    #endregion

    [Space]

    [Header("Player Stats")]
    [SyncVar] public PlayerStatus currentStatus = PlayerStatus.Joined;
    [SyncVar] public int starsCollected = 0;
    [SyncVar] public List<GameObject> obtainedClothes;
    [SyncVar] public bool gameFinished = false;

    [Space]

    [Header("Questions")]
    public bool inQuestionRange = false;
    public GameObject currentQuestion;

    [Space]

    [Header("Power-ups")]
    [SyncVar] public bool possessesAPowerUp = false;
    [SyncVar] public bool movementBlocked = true;
    private GameObject gm;
    public InactivateRule ir;
    public enum PowerUpTypes
    {
        None, // [K] Temporary measure, maybe will be able to remove it once I figure out the Inspector editor
        SelfAcceleration,
        GeneralLaziness,
        SwappingControls,
        SwappingPositions // Has to be swapped for an actual spell
    }

    [SyncVar] public PowerUpTypes currentPowerUpType = PowerUpTypes.None;

    [Space]

    [Header("UI")]
    [HideInInspector] public GameObject exitMenuPanel;
    [HideInInspector] public GameObject settingsMenuPanel;
    public GameObject AlertSprite;

    private Camera _camera;
    private Material playerMaterialClone;
    [SyncVar] public float normalizedMovement;
    [SyncVar] public float speed;
    private Rigidbody2D rb;
    private AudioSource stepsAudio;
    public AudioClip stepSound;

    ////===== TELEPORTATION ============

    //public GameObject[] TeleportationDoor;
    //public GameObject[] TeleportationDestination;

    // ===== Alert =========
    [Header("Obsolete properties")]
    //public TextMesh playerNameText;
    public TMP_Text playerNameText;
    public GameObject floatingInfo;
    void Start()
    {
        normalizedMovement = 1;
        defaultSprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
        if (isLocalPlayer)
        {
            Local = this;
            _camera = Camera.main;
            exitMenuPanel = GameObject.FindGameObjectWithTag("GamePanel");
            exitMenuPanel.transform.GetChild(0).gameObject.SetActive(false);
            //settingsMenuPanel = GameObject.FindGameObjectWithTag("shop");
        }
        gm = GameObject.Find("Game Manager"); //theres no other way to access game manager than this for powerupps
        //what is this for
        if (Local.ir == null)
        {
            Local.ir = FindObjectOfType<InactivateRule>();
        }
/*         var _childOfMenu = settingsMenuPanel.transform.GetChild(4);
        if(_childOfMenu.GetComponent<SettingsMenu>().appliedClothes.Count != 0){
            foreach(var i in _childOfMenu.GetComponent<SettingsMenu>().appliedClothes){
                var _tempGameObject = Instantiate(i,gameObject.transform);
                _tempGameObject.transform.SetParent(gameObject.transform);
            }
        }
        settingsMenuPanel.SetActive(false); */

        stepsAudio = GetComponent<AudioSource>();
        stepsAudio.Stop();
        //stepsAudio.clip = stepSound;
    }

    private void Update()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (ir != null && ir.gameObject.activeSelf == false && movementBlocked)
        {
            movementBlocked = false;
            ir = null;
        }

        if (currentDomain != null)
        {
            if (currentDomain.GetComponent<DomainInformation>().currentStatus == DomainInformation.DomainStatus.Chosen)
            {
                return;
            }
        }

        if (isLocalPlayer)
        {
            //If these inputs are made, trigger the animation bool
            #region Movement
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                anim.SetBool("WalkTrigger", true);
                if(!stepsAudio.isPlaying){
                    stepsAudio.Play();
                }
            }
            else
            {
                anim.SetBool("WalkTrigger", false);
                stepsAudio.Stop();
            }

            //If the player moves from left to right, mirror the character (look into direction
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }

            if (Input.GetKey(KeyCode.Space))
            {
                anim.SetBool("InteractionTrigger", true);
                //activated
            }
            else
            {
                anim.SetBool("InteractionTrigger", false);
            }
            #endregion

            #region Experimenting with Save System
            // [K] These can be used to test the save system

            if (Input.GetKeyDown(KeyCode.J))
            {
                XMLManager.instance.SaveStarScoreGame();
                Debug.Log("Score is saved");
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                Debug.Log("Current score is " + XMLManager.instance.LoadStarScore());
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                XMLManager.instance.NullifyStarScore();
                XMLManager.instance.NullifyOutfits();
                Debug.Log("Score is reset");
            }

            #endregion

            if (onDomain && finalDomain == 0)
            {
                Debug.Log("this works");
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ChoosingDomain();
                }
            }


            if (inQuestionRange)
            {
                if (isLocalPlayer)
                {
                    if (currentQuestion != null)
                    {
                        if (currentQuestion.GetComponent<TeleportationScript>().currentPuzzleStatus == TeleportationScript.puzzleStatus.Unsolved)
                        {
                            Local.AlertSprite.SetActive(true);
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    QuestionPrompted();
                    anim.SetBool("InteractionTrigger", true);
                }
                else
                {
                    anim.SetBool("InteractionTrigger", false);
                }
            }
            else
            {
                Local.AlertSprite.SetActive(false);
            }

            if (possessesAPowerUp)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (currentPowerUpType == PowerUpTypes.GeneralLaziness || currentPowerUpType == PowerUpTypes.SwappingControls || 
                        currentPowerUpType == PowerUpTypes.SwappingPositions)
                    {
                        if (!isClient)
                        {
                            return;
                        }
                    }

                    StartCoroutine(UsingAPowerUp());
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (exitMenuPanel != null)
                {
                    exitMenuPanel.transform.GetChild(0).gameObject.SetActive(!exitMenuPanel.transform.GetChild(0).gameObject.activeSelf);
                }
            }
        }

            if (currentDomain != null)
        {
            if (currentDomain.GetComponent<DomainInformation>().currentStatus == DomainInformation.DomainStatus.Chosen)
            {
                Local.showDomainMenu = false;
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
        //if(!QuestionScript.isEnabled){
        if(!movementBlocked){
            movement();
        }
        //}
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
            Vector3 movement = new Vector2(moveHorizontal * speed * normalizedMovement, moveVertical * speed * normalizedMovement);
            this.transform.position = transform.position + movement;
        }

    }

    #region Default functions

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
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Door"))
        {
            currentQuestion = collision.gameObject;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Door"))
        {
            inQuestionRange = true;
            if (isLocalPlayer)
            {
                if (currentQuestion != null)
                {
                    if (currentQuestion.GetComponent<TeleportationScript>().currentPuzzleStatus == TeleportationScript.puzzleStatus.Unsolved)
                    {
                        currentQuestion.GetComponent<QuestionRandomizer>().questionPromptUI.SetActive(inQuestionRange);
                    }
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Door"))
        {
            inQuestionRange = false;
            if (isLocalPlayer)
            {
                if (currentQuestion != null)
                {
                    currentQuestion.GetComponent<QuestionRandomizer>().questionPromptUI.SetActive(inQuestionRange);
                }
            }
            currentQuestion = null;
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
            if (isLocalPlayer)
            {
                FindObjectOfType<starAnimate>().canRotate = true;
            }
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
            currentDomain.GetComponent<DomainInformation>().currentStatus = DomainInformation.DomainStatus.Chosen;
            onDomain = false;
        }
    }

    public void QuestionPrompted()
    {
        if (isLocalPlayer)
        {
            currentQuestion.GetComponent<QuestionRandomizer>().ActivateQuestion();
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

    // [K] Set-up for Pepe
    IEnumerator UsingAPowerUp()
    {
        List<GameObject> _Players = gm.GetComponent<GameManager>().players;
        int _countPlayers = _Players.Count;

        possessesAPowerUp = false;

        switch (currentPowerUpType)
        {
            case PowerUpTypes.SelfAcceleration:
                speed *= 2;
                yield return new WaitForSeconds(3);
                speed /= 2;
                break;

            case PowerUpTypes.GeneralLaziness:

                CmdSlowOthersDown(false);
                yield return new WaitForSeconds(6);
                CmdSlowOthersDown(true);
                break;

            case PowerUpTypes.SwappingPositions:
                yield return new WaitForEndOfFrame();
                break;

            case PowerUpTypes.SwappingControls:
                CmdSwapControls(false);
                yield return new WaitForSeconds(30);
                CmdSwapControls(true);
                break;

            case PowerUpTypes.None:
                yield return null;
                break;
        }
        possessesAPowerUp = false;
        currentPowerUpType = PowerUpTypes.None;
    }

    public struct PlayerData
    {
        public string playerName { get; private set; }

        public PlayerData(string Player_name)
        {
            playerName = Player_name;
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdSlowOthersDown(bool slowingDown)
    {
        List<GameObject> _Players = gm.GetComponent<GameManager>().players;
        int _countPlayers = _Players.Count;

        for (int i = 0; i < _countPlayers; i++)
        {
            if (_Players[i].name != gameObject.name)
            {
                if (slowingDown == false)
                {
                    _Players[i].GetComponent<PlayerBehaviour>().speed /= 5;
                }
                else
                {
                    _Players[i].GetComponent<PlayerBehaviour>().speed *= 5;
                }
            }
        }
    }

    //[TargetRpc]
    //public void RpcSwapPlayersReceiver()
    //{

    //}

    [Command(requiresAuthority = false)]
    public void CmdSwapPositionsUser()
    {
        List<GameObject> _Players = gm.GetComponent<GameManager>().players;
        int _countPlayers = _Players.Count;

        if (_countPlayers > 1)
        {
            Vector3 _playerPos = gameObject.transform.position;
        choosingPlayer:
            int _randNum = Random.Range(0, _countPlayers - 1);
            if (_Players[_randNum] == gameObject)
            {
                Debug.Log("The player chose themselves");
                goto choosingPlayer;
            }
            Vector3 _otherPlayerPos = _Players[_randNum].transform.position;
            _Players[_randNum].transform.position = _playerPos;
            gameObject.transform.position = gameObject.transform.position - _otherPlayerPos;
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdSwapControls(bool normalControls)
    {
        List<GameObject> _Players = gm.GetComponent<GameManager>().players;
        int _countPlayers = _Players.Count;

        for (int i = 0; i < _countPlayers; i++)
        {
            if (_Players[i] != gameObject)
            {
                if (normalControls == false)
                {
                    _Players[i].GetComponent<PlayerBehaviour>().normalizedMovement = -1;
                }
                else
                {
                    _Players[i].GetComponent<PlayerBehaviour>().normalizedMovement = 1;
                }
            }
        }
    }
   
}

