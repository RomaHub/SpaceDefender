using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    public static HUD canvas;

    [Header("Component References")]
    [SerializeField]
    private Image pauseImage;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text hpText;
    [SerializeField]
    private Text statusText;

    [Header("Sprites")]
    [SerializeField]
    private Sprite pauseSprite;
    [SerializeField]
    private Sprite playSprite;


    private void Awake()
    {
        if (canvas == null)
            canvas = this;
        else if (canvas != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        HideStatusText();
    }

    public void ToggleSprite(float isPlaying)
    {
        pauseImage.sprite = isPlaying == 1f ? pauseSprite : playSprite;
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateHP(int hp)
    {
        hpText.text = hp.ToString();
    }

    public void SetStatusText(string text)
    {
        statusText.text = text;
        statusText.enabled = true;
    }

    public void HideStatusText()
    {
        statusText.enabled = false;
    }

}
