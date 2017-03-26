using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour, IDamageable
{

    [Header("Attributes")]
    [SerializeField]
    private int _maxHalth = 1;
    [SyncVar(hook = "OnHealthChanged")]
    private int _health;
    [SyncVar(hook = "OnScoreChanged")]
    private int _score = 0;

    private SpriteRenderer _sprite;
    private BoxCollider2D _collider;
    private ParticleSystem _afterburner;
    private AudioSource _audioEffect;


    #region UnityCallbacks

    private void Awake()
    {
        _audioEffect = GetComponent<AudioSource>();
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        _afterburner = transform.FindChild("Afterburner").gameObject.GetComponent<ParticleSystem>();
    }

    [ServerCallback]
    private void OnEnable()
    {
        _health = _maxHalth;
    }

    private void Start()
    {
        _health = _maxHalth;
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CmdToggleGameSpeed();
        }

        if (Input.GetKeyDown(KeyCode.Q))
            EndGame();
    }

    #endregion

    #region Hooks

    private void OnHealthChanged(int value)
    {
        _health = value;
        if (isLocalPlayer)
            HUD.canvas.UpdateHP(_health);
    }

    private void OnScoreChanged(int value)
    {
        _score = value;
        if (isLocalPlayer)
            HUD.canvas.UpdateScore(_score);
    }

    #endregion


    //Command is called from the local Player and run on the server
    //Server methods only run on the server
    #region Server/Commands

    [Server]
    public void TakeDamage(int damage, GameObject sender)
    {
        _health -= damage;

        if (_health <= 0)
        {
            _health = 0;

            RpcDied();
        }
    }

    [Command]
    private void CmdAddScore(int score)
    {
        _score += score;
    }

    [Command]
    private void CmdToggleGameSpeed()
    {
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        RpcToggleTime(Time.timeScale);
    }

    #endregion

    #region RPC

    [ClientRpc]
    void RpcToggleTime(float t)
    {
        GameManager.instance.ToggleMusicSnapshot();
        Time.timeScale = t;
        HUD.canvas.ToggleSprite(t);
    }

    [ClientRpc]
    private void RpcDied()
    {
        if (isLocalPlayer)
        {
            HUD.canvas.SetStatusText("GAME OVER");
            GetComponent<PlayerMove>().enabled = false;
            GetComponent<PlayerShoot>().enabled = false;
        }

        _sprite.enabled = false;
        _collider.enabled = false;
        _afterburner.Stop();
        _audioEffect.Play();

        //Invoke("Respawn", 2f);

        Invoke("EndGame", 4f);

        //camera shake
        //audio
        //explosion effect
    }

    #endregion

    #region Public

    public void AddScore(int score)
    {
        CmdAddScore(score);
    }

    #endregion

    #region Private

    private void EndGame()
    {
        if (isLocalPlayer)
            GameManager.instance.StopHost();
    }

    /*
    void Respawn()
    {
        if(isLocalPlayer)
        {
            HUD.canvas.HideStatusText();
            Transform spawn = NetworkManager.singleton.GetStartPosition();
            transform.position = spawn.position;

            GetComponent<PlayerMove>().enabled = true;
            GetComponent<PlayerShoot>().enabled = true;
        }

        _sprite.enabled = true;
        _collider.enabled = true;
        _afterburner.Play();

        _health = _maxHalth;//server
    }
    */

    #endregion


}
