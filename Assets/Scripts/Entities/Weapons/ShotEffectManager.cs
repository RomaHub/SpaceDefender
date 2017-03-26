using UnityEngine;

public class ShotEffectManager : MonoBehaviour
{

    [Header("References")]
    [SerializeField]
    private AudioSource gunAudio;
    [SerializeField]
    private ParticleSystem muzzleFlash;

    public void PlayShotEffect()
    {
        gunAudio.Stop();
        gunAudio.Play();
        muzzleFlash.Stop(true);
        muzzleFlash.Play(true);
    }


}
