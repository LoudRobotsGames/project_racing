using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuScript : MonoBehaviour, MPLobbyListener
{
    public GameObject lobbyMessageBox;
    public Text lobbyMessageField;
    public Button singlePlayer;
    public Button multiPlayer;
    public Button signOut;

    public void SetLobbyStatusMessage(string message)
    {
        lobbyMessageField.text = message;
        lobbyMessageBox.SetActive(true);
    }

    public void HideLobby()
    {
        lobbyMessageField.text = "";
        lobbyMessageBox.SetActive(false);
    }

    public void SignOut()
    {
        if (NetworkProvider.Instance != null)
        {
            NetworkProvider.Instance.SignOut();
            signOut.gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        if (NetworkProvider.Instance != null)
        {
            if (NetworkProvider.Instance.IsAuthenticated())
            {
                multiPlayer.interactable = true;
                signOut.gameObject.SetActive(true);
            }
        }
    }

    public void StartSinglePlayer()
    {
        // Single player mode!
        RetainedUserPicksScript.Instance.multiplayerGame = false;
        RetainedUserPicksScript.Instance.carSelected = 2;
        RetainedUserPicksScript.Instance.diffSelected = 3;
        Application.LoadLevel("Game");
    }

    public void StartMultiPlayer()
    {
        RetainedUserPicksScript.Instance.multiplayerGame = true;
        SetLobbyStatusMessage("Starting a multi-player game...");
        NetworkProvider.Instance.lobbyListener = this;
        NetworkProvider.Instance.SignInAndStartMPGame();
    }

    void Start()
    {
        HideLobby();

        NetworkProvider.Initialize();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        signOut.gameObject.SetActive(false);
        if (NetworkProvider.Instance != null)
        {
            NetworkProvider.Instance.TrySilentSignIn();
        }
        else
        {
            multiPlayer.interactable = false;
        }
    }
}
