using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class NetworkProvider
{
    private static NetworkProvider _instance = null;
    public static NetworkProvider Instance
    {
        get
        {
            return _instance;
        }
    }

    public static void Initialize()
    {

#if (UNITY_ANDROID || (UNITY_IPHONE && !NO_GPGS))
        _instance = new GPGProvider();
#else
        _instance = null;
#endif
    }

    protected uint minimumOpponents = 1;
    protected uint maximumOpponents = 1;
    protected uint gameVariation = 0;

    protected byte _protocolVersion = 1;
    protected int _myMessageNum;
    protected int _updateMessageLength = 23;
    protected int _finishMessageLength = 6;
    protected List<byte> _updateMessage;

    public MPLobbyListener lobbyListener;
    public MPUpdateListener updateListener;

    public abstract void SignInAndStartMPGame();
    public abstract void TrySilentSignIn();
    public abstract void SignOut();
    public abstract void LeaveGame();

    public abstract List<NetworkPlayer> GetAllPlayers();
    public abstract string GetMyPlayerId();
    public abstract bool IsAuthenticated();

    public abstract void SendMyUpdate(float posX, float posY, Vector2 velocity, float rotZ);
    public abstract void SendFinishMessage(float totalTime);

    public abstract void OnRoomSetupProgress(float percent);
    public abstract void OnRoomConnected(bool success);
    public abstract void OnLeftRoom();
    public abstract void OnPlayerLeft(NetworkPlayer player);
    public abstract void OnPlayersConnected(string[] playerIds);
    public abstract void OnPlayersDisconnected(string[] participantIds);
    public abstract void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data);

    protected abstract void StartMatchmaking();

    protected virtual void ShowMPStatus(string message)
    {
        if (lobbyListener != null)
        {
            lobbyListener.SetLobbyStatusMessage(message);
        }
    }
}
