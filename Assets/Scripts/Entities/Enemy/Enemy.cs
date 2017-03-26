using UnityEngine;
using UnityEngine.Networking;

public class Enemy : NetworkBehaviour, IDamageable
{

    public GameObject astreroidPrefab;
    public int astreroidAmount = 4;

    [Header("Attributes")]
    public float speed;
    public int damage;

    [Header("References")]
    [SerializeField]
    private GameObject explosionPrefab;

    private AudioSource _audio;
    private GameObject _killer;

    private int _health;

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            if (_health <= 0)
            {
                DeathScore();
            }
        }
    }


    #region UnityCallbacks

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(damage, gameObject);
            Death();
        }
    }

    #endregion

    #region Public

    public void TakeDamage(int damage, GameObject sender)
    {
        _audio.Play();

        _killer = sender;
        Health -= damage;
    }

    #endregion

    #region Private

    private void Death()
    {
        Destroy(gameObject);
        GameObject go = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
        Destroy(go, 2f);
        NetworkServer.Spawn(go);

        if (astreroidPrefab != null)
        {
            float randomAsteroidAmount = Random.Range(1, astreroidAmount);

            for (int i = 0; i < randomAsteroidAmount; i++)
            {
                SpawnChild();
            }
        }
    }

    private void SpawnChild()
    {
        GameObject child = Instantiate(astreroidPrefab, gameObject.transform.position, Quaternion.identity);

        float size = Random.Range(-0.1f, 0.1f);
        child.transform.localScale += new Vector3(size, size, 0);

        Enemy enemy = child.GetComponent<Enemy>();
        float randomSpeed = Random.Range(speed * 2f, speed * 3f);

        Vector3 vector = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);

        Rigidbody2D rb2d = child.GetComponent<Rigidbody2D>();
        rb2d.AddForce(vector * randomSpeed);

        NetworkServer.Spawn(child);
    }

    private void DeathScore()
    {
        Death();
        _killer.GetComponent<Player>().AddScore(1);
    }

    #endregion
}
