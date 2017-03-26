using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{

    [Header("Attributes")]
    [SerializeField]
    private float _bulletSpeed = 400f;
    [SerializeField]
    private float _rateOfFire = 0.2f;
    private float _fireTimer;

    [Header("References")]
    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private Transform _gunBarrel;
    [SerializeField]
    private ShotEffectManager shotEffect;

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetButton("Fire1"))
        {
            if (Time.time > _fireTimer)
            {
                _fireTimer = Time.time + _rateOfFire;
                CmdShoot();
            }
        }
    }

    [Command]
    private void CmdShoot()
    {
        GameObject bulletInstance = Instantiate(_bulletPrefab, _gunBarrel.position, _gunBarrel.rotation) as GameObject;
        bulletInstance.GetComponent<Rigidbody2D>().AddForce(_gunBarrel.up * _bulletSpeed);

        bulletInstance.GetComponent<Bullet>().Owner = gameObject;

        NetworkServer.Spawn(bulletInstance);

        RpcShotEffect();
    }

    [ClientRpc]
    private void RpcShotEffect()
    {
        shotEffect.PlayShotEffect();
    }

}
