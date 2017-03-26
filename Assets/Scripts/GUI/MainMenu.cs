using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [Header("Component References")]
    [SerializeField]
    private Button buttonNewGame;
    [SerializeField]
    private Button buttonLanGame;
    [SerializeField]
    private Button buttonQuit;
    [SerializeField]
    private Button buttonConnect;
    [SerializeField]
    private Button buttonBack;
    [SerializeField]
    private InputField inputIp;

    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject connectMenu;

    private void OnEnable()
    {
        buttonNewGame.onClick.AddListener(NewGame);
        buttonLanGame.onClick.AddListener(EnableConnectMenu);
        buttonQuit.onClick.AddListener(QuitGame);
        buttonConnect.onClick.AddListener(Connect);
        buttonBack.onClick.AddListener(EnableMainMenu);

        inputIp.onEndEdit.RemoveAllListeners();
        inputIp.onEndEdit.AddListener(onEndEditIP);
    }

    private void OnDisable()
    {
        buttonNewGame.onClick.RemoveListener(NewGame);
        buttonLanGame.onClick.RemoveListener(EnableConnectMenu);
        buttonQuit.onClick.RemoveListener(QuitGame);
        buttonConnect.onClick.RemoveListener(Connect);
        buttonBack.onClick.RemoveListener(EnableMainMenu);
    }

    private void Start()
    {
        mainMenu.SetActive(true);
        connectMenu.SetActive(false);
    }

    private void EnableMainMenu()
    {
        GameManager.instance.ClickSound();
        mainMenu.SetActive(true);
        connectMenu.SetActive(false);
    }

    private void EnableConnectMenu()
    {
        GameManager.instance.ClickSound();
        mainMenu.SetActive(false);
        connectMenu.SetActive(true);
    }

    private void NewGame()
    {
        GameManager.instance.ClickSound();
        GameManager.instance.StartHost();
    }

    private void Connect()
    {
        GameManager.instance.ClickSound();
        GameManager.instance.networkAddress = inputIp.text;
        GameManager.instance.StartClient();
    }

    private void QuitGame()
    {
        GameManager.instance.ClickSound();
        GameManager.instance.QuitGame();
    }

    private void onEndEditIP(string text)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Connect();
        }
    }



}
