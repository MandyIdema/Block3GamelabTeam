using GM;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    [Header("Player Stats")]
    [SyncVar] public int starsCollected = 0;
    [SyncVar] public List<GameObject> obtainedClothes;
    [SyncVar] public float normalizedMovement;
    [SyncVar] public float speed;

    [Header("Player Info")]
    [SyncVar] public PlayerStatus currentStatus = PlayerStatus.Joined;
    [SyncVar] public bool movementBlocked = true;
    public GameObject AlertSprite;
    public AudioClip stepSound;
    public TMP_Text playerNameText;
    public GameObject playerUsername;
    private string localUsernameString;
    [SyncVar] public string playerUsernameString;

    [Space]

    [Header("Questions")]
    public bool inQuestionRange = false;
    public GameObject currentQuestion;

    [Space]

    [Header("Power-ups")]
    [SyncVar] public bool possessesAPowerUp = false;

    public enum PowerUpTypes
    {
        None, // [K] Temporary measure, maybe will be able to remove it once I figure out the Inspector editor
        SelfAcceleration,
        GeneralLaziness,
        SwappingControls
    }

    [SyncVar] public PowerUpTypes currentPowerUpType = PowerUpTypes.None;

    [Space]

    public GameObject exitMenuPanel;

    private Camera _camera;
    private Rigidbody2D rb;
    private AudioSource stepsAudio;
    

    private GameManager _gm;
    public Referencer _Referencer;
    public GameObject usernameHolder;
    public GameObject usernameInput;
    private InactivateRule ir;

    void Start()
    {

        if (isLocalPlayer)
        {
            Local = this;
            _camera = Camera.main;


            usernameHolder = GameObject.FindGameObjectWithTag("UsernameHolder");
            if (usernameHolder != null)
            {
                usernameInput = usernameHolder.gameObject.transform.GetChild(0).gameObject;
            }

            if (usernameHolder != null)
            {
                usernameInput.GetComponent<TMP_InputField>().onEndEdit.AddListener(delegate { DeclareUsername(); });
            }
        }

        normalizedMovement = 1;

        defaultSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        _Referencer = FindObjectOfType<Referencer>();
        exitMenuPanel = _Referencer.ExitMenuPanel;
        exitMenuPanel.SetActive(false);

        _gm = FindObjectOfType<GameManager>();

        if (ir == null)
        {
            ir = FindObjectOfType<InactivateRule>();
        }

/*         if(obtainedClothes.Count == 0){
            var _shopMenu = _Referencer.MainMenuPanelButtons.transform.GetChild(4);
            obtainedClothes =  _shopMenu.GetComponent<SettingsMenu>().appliedClothes;
            if(obtainedClothes.Count > 0){
                foreach (var i in obtainedClothes){
                    var _tempCloth = Instantiate(i,gameObject.transform);
                    _tempCloth.transform.SetParent(gameObject.transform);
                }
            }
        } */

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
            if (isClient)
            {
                movementBlocked = false;
            }
            ir = null;
        }

        if (isServer)
        {
            RpcUpdateUsername();
        }

        if (isLocalPlayer)
        {
            
            //If these inputs are made, trigger the animation bool
            #region Movement
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                anim.SetBool("WalkTrigger", true);
                if(!stepsAudio.isPlaying){
                    //stepsAudio.Play();
                }
            }
            else
            {
                anim.SetBool("WalkTrigger", false);
                //stepsAudio.Stop();
            }

            //If the player moves from left to right, mirror the character (look into direction
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                //playerUsername.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                // playerUsername.transform.localRotation = Quaternion.Euler(0, 0, 0);
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
                Debug.Log("Score and outfits are reset");
            }

            #endregion

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

            if (possessesAPowerUp && currentPowerUpType != PowerUpTypes.None)
            {
                if (!isClient)
                {
                    return;
                }
                StartCoroutine(UsingAPowerUp());
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                findingMenu:
                if (exitMenuPanel != null)
                {
                    exitMenuPanel.SetActive(!exitMenuPanel.activeSelf);
                }
                else
                {
                    exitMenuPanel = _Referencer.ExitMenuPanel;
                    goto findingMenu;
                }
            }
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

        if (!isLocalPlayer)
        {
            playerUsername.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
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
        if (collision.gameObject.CompareTag("Star"))
        {
            if (isLocalPlayer)
            {
                FindObjectOfType<starAnimate>().canRotate = true;
            }
        }
    }
    public void QuestionPrompted()
    {
        if (isLocalPlayer)
        {
            currentQuestion.GetComponent<QuestionRandomizer>().ActivateQuestion();
        }
    }
    // [K] Set-up for Pepe
    IEnumerator UsingAPowerUp()
    {
        possessesAPowerUp = false;
        switch (currentPowerUpType)
        {
            case PowerUpTypes.SelfAcceleration:
                speed *= 1.3f;
                yield return new WaitForSeconds(5);
                speed /= 1.3f;
                break;

            case PowerUpTypes.GeneralLaziness:

                CmdSlowOthersDown(false);
                yield return new WaitForSeconds(4);
                CmdSlowOthersDown(true);
                break;

            case PowerUpTypes.SwappingControls:
                CmdSwapControls(false);
                yield return new WaitForSeconds(5);
                CmdSwapControls(true);
                break;

            case PowerUpTypes.None:
                yield return null;
                break;
        }
        currentPowerUpType = PowerUpTypes.None;
    }

    //public struct PlayerData
    //{
    //    public string playerName { get; private set; }

    //    public PlayerData(string Player_name)
    //    {
    //        playerName = Player_name;
    //    }
    //}

    [Command(requiresAuthority = false)]
    public void CmdSlowOthersDown(bool slowingDown)
    {
        List<GameObject> _Players = _gm.players;
        int _countPlayers = _Players.Count;

        for (int i = 0; i < _countPlayers; i++)
        {
            if (_Players[i].name != gameObject.name)
            {
                if (slowingDown == false)
                {
                    _Players[i].GetComponent<PlayerBehaviour>().speed /= 1.8f;
                }
                else
                {
                    _Players[i].GetComponent<PlayerBehaviour>().speed *= 1.8f;
                }
            }
        }
    }
    [Command(requiresAuthority = false)]
    public void CmdSwapControls(bool normalControls)
    {
        List<GameObject> _Players = _gm.players;
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

    [ClientRpc]
    public void RpcUpdateUsername()
    {
        playerUsername.transform.rotation = Quaternion.Euler(0, -gameObject.transform.rotation.y, 0);
    }


    void DeclareUsername()
    {
        if (!isLocalPlayer)
        {
            Debug.Log("Somebody else changed their username!");
            return;
        }
        if (!string.IsNullOrEmpty(Local.usernameInput.GetComponent<TMP_InputField>().text))
        {
            Debug.Log("You chose a username!");
            localUsernameString = usernameInput.GetComponent<TMP_InputField>().text;
        }
        else if(string.IsNullOrEmpty(Local.usernameInput.GetComponent<TMP_InputField>().text))
        {
            Debug.Log("You didn't choose a username!");
            localUsernameString = $"Player {(int)Random.Range(0, 999)}";
        }
        Debug.Log($"Your local string is {localUsernameString}");
        movementBlocked = false;
        if (isClient && isServer)
        {
            CmdSetupUsername();
        }
        if (isClientOnly)
        {
            Debug.Log(netIdentity.connectionToClient);
            UpdatePlayerUsernameClient();
        }
    }
    public void UpdatePlayerUsernameClient()
    {
        playerUsernameString = Local.localUsernameString;
        playerUsername.GetComponent<TextMeshProUGUI>().text = playerUsernameString;
    }

    [Command]
    public void CmdSetupUsername()
    {
        Debug.Log($"Your command string is {Local.localUsernameString}");
        UpdatePlayerUsername(gameObject.GetComponent<NetworkIdentity>(), Local.localUsernameString);
        //usernameHolder.SetActive(false);
    }

    [ClientRpc]
    public void UpdatePlayerUsername(NetworkIdentity chosenPlayer, string chosenUsername)
    {
        chosenPlayer.gameObject.GetComponent<PlayerBehaviour>().playerUsernameString = chosenUsername;
        Debug.Log($"The server says that string is {chosenPlayer.gameObject.GetComponent<PlayerBehaviour>().playerUsernameString}");
        chosenPlayer.gameObject.GetComponent<PlayerBehaviour>().playerUsername.GetComponent<TextMeshProUGUI>().text = playerUsernameString;
    }
}

