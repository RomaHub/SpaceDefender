using UnityEngine;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour
{

    [Header("Attributes")]
    public float roundTime = 15f;
    public int round = 1;
    public float spawnInterval = 5f;
    public float enemySpeed = 20f;
    public float speedMultiplier = 1.1f;
    public int enemyHealth = 2;
    public float enemySizeDelta = 0.2f;
    public int enemyAmount = 1;

    private float _ellapsedTime;
    private float _waveCooldown;

    [Header("References")]
    [SerializeField]
    private GameObject _enemyPrefab;

    private void Start()
    {
        _waveCooldown = spawnInterval;
    }


    private void Update()
    {

        if (Time.time > _waveCooldown)
        {
            _waveCooldown = Time.time + roundTime;

            round++;

            enemyAmount = (int)Mathf.Log(round, 2f);

            spawnInterval = spawnInterval - (spawnInterval / 10f);

            if (round % 2 == 0)
                enemyHealth++;

            enemySpeed *= speedMultiplier;

        }

        _ellapsedTime += Time.deltaTime;

        if (_ellapsedTime > spawnInterval)
        {
            _ellapsedTime = 0;

            SpawnEnemy(enemyAmount);
        }

    }

    public void SpawnEnemy(int amount)
    {

        for (int i = 0; i < amount; i++)
        {
            GameObject go = Instantiate(_enemyPrefab, new Vector3(Random.Range(-4, 4), transform.position.y, transform.position.z), Quaternion.identity);

            float size = Random.Range(-enemySizeDelta, enemySizeDelta);
            go.transform.localScale += new Vector3(size, size, 0);

            Enemy enemy = go.GetComponent<Enemy>();

            float randomSpeed = Random.Range(enemySpeed, enemySpeed * 1.5f);

            enemy.speed = randomSpeed;
            enemy.Health = enemyHealth;

            Vector3 vector = new Vector3(Random.Range(-0.5f, 0.5f), -1f, 0f);

            Rigidbody2D rb2d = go.GetComponent<Rigidbody2D>();
            rb2d.AddForce(vector * randomSpeed);
            rb2d.AddTorque(Random.Range(-8f, 8f));

            NetworkServer.Spawn(go);
        }

    }


}
