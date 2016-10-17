using UnityEngine;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using System;

public class MultiplayerController : RealTimeMultiplayerListener
{
    private static MultiplayerController _instance = null;

    public static MultiplayerController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MultiplayerController();
            }
            return _instance;
        }
    }

    private uint minimumOpponents = 1;
    private uint maximumOpponents = 1;
    private uint gameVariation = 0;

    private byte _protocolVersion = 1;
    private int _myMessageNum;
    private int _updateMessageLength = 23;
    private int _finishMessageLength = 6;
    private List<byte> _updateMessage;

    public MPLobbyListener lobbyListener;
    public MPUpdateListener updateListener;

    private MultiplayerController()
    {
        _updateMessage = new List<byte>(_updateMessageLength);

        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    public void SignInAndStartMPGame()
    {
        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    StartMatchmaking();
                }
                else
                {
                    Debug.Log("Oh... we're not signed in.");
                }
            });
        }
        else
        {
            StartMatchmaking();
        }
    }

    public void TrySilentSignIn()
    {
        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log("Silently signed in! Welcome " + PlayGamesPlatform.Instance.localUser.userName);
                }
                else
                {
                    Debug.Log("Oh... we're not signed in.");
                }
            }, true);
        }
        else
        {
            Debug.Log("We're already signed in.");
        }
    }

    public void SignOut()
    {
        PlayGamesPlatform.Instance.SignOut();
    }

    public List<Participant> GetAllPlayers()
    {
        return PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
    }

    public string GetMyParticipantId()
    {
        return PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId;
    }

    public void SendMyUpdate(float posX, float posY, Vector2 velocity, float rotZ)
    {
        _updateMessage.Clear();
        _updateMessage.Add(_protocolVersion);
        _updateMessage.Add((byte)'U');
        _updateMessage.AddRange(System.BitConverter.GetBytes(++_myMessageNum));
        _updateMessage.AddRange(System.BitConverter.GetBytes(posX));
        _updateMessage.AddRange(System.BitConverter.GetBytes(posY));
        _updateMessage.AddRange(System.BitConverter.GetBytes(velocity.x));
        _updateMessage.AddRange(System.BitConverter.GetBytes(velocity.y));
        byte rot = (byte)(rotZ * 256 / 360);
        _updateMessage.Add(rot);
        byte[] messageToSend = _updateMessage.ToArray();
        PlayGamesPlatform.Instance.RealTime.SendMessageToAll(false, messageToSend);
    }

    public void SendFinishMessage(float totalTime)
    {
        List<byte> bytes = new List<byte>(_finishMessageLength);
        bytes.Add(_protocolVersion);
        bytes.Add((byte)'F');
        bytes.AddRange(System.BitConverter.GetBytes(totalTime));
        byte[] messageToSend = bytes.ToArray();
        PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, messageToSend);
    }

    private void StartMatchmaking()
    {
        PlayGamesPlatform.Instance.RealTime.CreateQuickGame(
            minimumOpponents, 
            maximumOpponents, 
            gameVariation, 
            this
        );
    }

    public void LeaveGame()
    {
        PlayGamesPlatform.Instance.RealTime.LeaveRoom();
    }

    private void ShowMPStatus(string message)
    {
        if (lobbyListener != null)
        {
            lobbyListener.SetLobbyStatusMessage(message);
        }
    }

    public bool IsAuthenticated()
    {
        return PlayGamesPlatform.Instance.localUser.authenticated;
    }

    public void OnRoomSetupProgress(float percent)
    {
        //ShowMPStatus("We are " + percent + "% done with setup");
        ShowMPStatus("Waiting for opponents...");
    }

    public void OnRoomConnected(bool success)
    {
        if (success)
        {
            lobbyListener.HideLobby();
            lobbyListener = null;
            _myMessageNum = 0;
            Application.LoadLevel("Game");
        }
        else
        {
            ShowMPStatus("Uh-oh. Encountered some error connecting to the room.");
        }
    }

    public void OnLeftRoom()
    {
        ShowMPStatus("We have left the room. We should probably perform some clean-up tasks.");
        if (updateListener != null)
        {
            updateListener.LeftRoomConfirmed();
        }
    }

    public void OnParticipantLeft(Participant participant)
    {
    }

    public void OnPeersConnected(string[] participantIds)
    {
        foreach (string participantID in participantIds)
        {
            ShowMPStatus("Player " + participantID + " has joined.");
        }
    }

    public void OnPeersDisconnected(string[] participantIds)
    {
        foreach (string participantID in participantIds)
        {
            ShowMPStatus("Player " + participantID + " has left.");
            if (updateListener != null)
            {
                updateListener.PlayerLeftRoom(participantID);
            }
        }
    }

    public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
    {
        byte messageVersion = (byte)data[0];
        char messageType = (char)data[1];
        if (messageType == 'U' && data.Length == _updateMessageLength)
        {
            int messageNum = System.BitConverter.ToInt32(data, 2);
            float posX = System.BitConverter.ToSingle(data, 6);
            float posY = System.BitConverter.ToSingle(data, 10);
            float velX = System.BitConverter.ToSingle(data, 14);
            float velY = System.BitConverter.ToSingle(data, 18);
            byte rot = data[22];
            float rotZ = rot * 360.0f / 256.0f;
            if (updateListener != null)
            {
                updateListener.UpdateReceived(senderId, messageNum, posX, posY, velX, velY, rotZ);
            }
        }
        else if (messageType == 'F' && data.Length == _finishMessageLength)
        {
            float finalTime = System.BitConverter.ToSingle(data, 2);
            updateListener.PlayerFinished(senderId, finalTime);
        }
    }
}
