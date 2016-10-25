#if (UNITY_ANDROID || (UNITY_IPHONE && !NO_GPGS))
using UnityEngine;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using System;
public class GPGProvider : NetworkProvider, RealTimeMultiplayerListener
{
    public GPGProvider()
    {
        _updateMessage = new List<byte>(_updateMessageLength);

        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }
#region NetworkProvider
    public override List<NetworkPlayer> GetAllPlayers()
    {
        List <Participant> participants = PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
        List<NetworkPlayer> players = new List<NetworkPlayer>(participants.Count);
        for( int i = 0; i < participants.Count; ++i)
        {
            NetworkPlayer player = new NetworkPlayer();
            players.Add(player);
        }
        return players;
    }

    public override string GetMyPlayerId()
    {
        return PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId;
    }

    public override bool IsAuthenticated()
    {
        return PlayGamesPlatform.Instance.localUser.authenticated;
    }

    public override void LeaveGame()
    {
        PlayGamesPlatform.Instance.RealTime.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        ShowMPStatus("We have left the room. We should probably perform some clean-up tasks.");
        if (updateListener != null)
        {
            updateListener.LeftRoomConfirmed();
        }
    }

    public void OnParticipantLeft(Participant participant)
    {
        NetworkPlayer player = new NetworkPlayer();
        player.PlayerId = participant.ParticipantId;
        OnPlayerLeft(player);
    }

    public void OnPeersConnected(string[] participantIds)
    {
        OnPlayersConnected(participantIds);
    }

    public void OnPeersDisconnected(string[] participantIds)
    {
        OnPlayersDisconnected(participantIds);
    }

    public override void OnPlayerLeft(NetworkPlayer player)
    {
    }

    public override void OnPlayersConnected(string[] playerIds)
    {
        foreach (string participantID in playerIds)
        {
            ShowMPStatus("Player " + participantID + " has joined.");
        }
    }

    public override void OnPlayersDisconnected(string[] playerIds)
    {
        foreach (string participantID in playerIds)
        {
            ShowMPStatus("Player " + participantID + " has left.");
            if (updateListener != null)
            {
                updateListener.PlayerLeftRoom(participantID);
            }
        }
    }

    public override void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
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

    public override void OnRoomConnected(bool success)
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

    public override void OnRoomSetupProgress(float percent)
    {
        //ShowMPStatus("We are " + percent + "% done with setup");
        ShowMPStatus("Waiting for opponents...");
    }

    public override void SendFinishMessage(float totalTime)
    {
        List<byte> bytes = new List<byte>(_finishMessageLength);
        bytes.Add(_protocolVersion);
        bytes.Add((byte)'F');
        bytes.AddRange(System.BitConverter.GetBytes(totalTime));
        byte[] messageToSend = bytes.ToArray();
        PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, messageToSend);
    }

    public override void SendMyUpdate(float posX, float posY, Vector2 velocity, float rotZ)
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

    public override void SignInAndStartMPGame()
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

    public override void SignOut()
    {
        PlayGamesPlatform.Instance.SignOut();
    }

    public override void TrySilentSignIn()
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

    protected override void StartMatchmaking()
    {
        PlayGamesPlatform.Instance.RealTime.CreateQuickGame(
            minimumOpponents,
            maximumOpponents,
            gameVariation,
            this
        );
    }
#endregion

}
#endif