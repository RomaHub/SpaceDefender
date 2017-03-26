using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{

    [Header("Attributes")]
    [SerializeField]
    private int _damage = 1;
    [SerializeField]
    private float _lifeTime = 1f;



    public GameObject Owner { get; set; }

    private void Start()
    {
        Invoke("DestroyBullet", _lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isServer)
            return;

        var damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(_damage, Owner);
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        NetworkServer.Destroy(gameObject);
    }
}
