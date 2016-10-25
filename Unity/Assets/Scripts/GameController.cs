using UnityEngine;
using System.Collections.Generic;
using GooglePlayGames.BasicApi.Multiplayer;
using System;

public class GameController : MonoBehaviour, MPUpdateListener {

    private const float BROADCAST_INTERVAL = 0.16f;
    public GameObject opponentPrefab;

    private bool _multiplayerReady;
    private string _myParticipantId;
    private Dictionary<string, OpponentCarController> _opponentScripts;
    private Dictionary<string, float> _finishTimes;
    private float _nextBroadcastTime = 0;

	public GameObject myCar;
	public GuiController guiObject;
	public GUISkin guiSkin;
	public float[] startTimesPerLevel;
	public int[] lapsPerLevel;

	public bool _paused;
	private float _timeLeft;
	private float _timePlayed;
	private int _lapsRemaining;
	private bool _showingGameOver;
	private bool _multiplayerGame;
	private string gameOvertext;
	private float _nextCarAngleTarget = Mathf.PI;
	private const float FINISH_TARGET = Mathf.PI;

    public float timeOutThreshold = 5.0f;
    private float _timeOutCheckInterval = 1.0f;
    private float _nextTimeoutCheck = 0.0f;
    private RaceTrackController _trackController;

    // Use this for initialization
    void Start () {
        _trackController = FindObjectOfType<RaceTrackController>();

		RetainedUserPicksScript userPicksScript = RetainedUserPicksScript.Instance;
		_multiplayerGame = userPicksScript.multiplayerGame;
		if (! _multiplayerGame) {
            SetupSinglePlayerGame();
		} else {
            SetupMultiplayerGame();
		}

	}

    void SetupSinglePlayerGame()
    {
        RetainedUserPicksScript userPicksScript = RetainedUserPicksScript.Instance;

        // Can we get the car number from the previous menu?
        myCar.GetComponent<CarController>().SetCarChoice(userPicksScript.carSelected, false);
        // Set our time left and laps remaining
        _timeLeft = startTimesPerLevel[userPicksScript.diffSelected];
        _lapsRemaining = lapsPerLevel[userPicksScript.diffSelected];

        guiObject.SetTime(_timeLeft);
        guiObject.SetLaps(_lapsRemaining);

        myCar.transform.position = _trackController.SpawnPoint(0);
    }

    void SetupMultiplayerGame() {
        NetworkProvider.Instance.updateListener = this;
        _myParticipantId = NetworkProvider.Instance.GetMyPlayerId();
        List<NetworkPlayer> allPlayers = NetworkProvider.Instance.GetAllPlayers();
        _opponentScripts = new Dictionary<string, OpponentCarController>(allPlayers.Count - 1);
        _finishTimes = new Dictionary<string, float>(allPlayers.Count);
        for (int i = 0; i < allPlayers.Count; i++)
        {
            string nextParticipantId = allPlayers[i].PlayerId;
            _finishTimes[nextParticipantId] = -1;
            Debug.Log("Setting up car for " + nextParticipantId);
            Vector3 carStartPoint = _trackController.SpawnPoint(i);
            if (nextParticipantId == _myParticipantId)
            {
                myCar.GetComponent<CarController>().SetCarChoice(i + 1, true);
                myCar.transform.position = carStartPoint;
            }
            else
            {
                GameObject opponentCar = (Instantiate(opponentPrefab, carStartPoint, Quaternion.identity) as GameObject);
                OpponentCarController opponentScript = opponentCar.GetComponent<OpponentCarController>();
                opponentScript.SetCarNumber(i + 1);
                _opponentScripts[nextParticipantId] = opponentScript;
            }
        }
        _lapsRemaining = 3;
        _timePlayed = 0;
        guiObject.SetLaps(_lapsRemaining);
        guiObject.SetTime(_timePlayed);
        _multiplayerReady = true;
    }


	void PauseGame() {
		_paused = true;
		myCar.GetComponent<CarController>().SetPaused(true);
	}
	
	void ShowGameOver(bool didWin) {
		gameOvertext = (didWin) ? "Woo hoo! You win!" : "Awww... better luck next time";
		PauseGame ();
		_showingGameOver = true;
		Invoke ("StartNewGame", 3.0f);
	}

	void StartNewGame() {
		Application.LoadLevel ("MainMenu");
	}

	void OnGUI() {
		if (_showingGameOver) {
			GUI.skin = guiSkin;
			GUI.Box(
                new Rect(
                    Screen.width * 0.25f, Screen.height * 0.25f, 
                    Screen.width * 0.5f, Screen.height * 0.5f), 
                gameOvertext
            );
		}
        if (NetworkProvider.Instance != null)
        {
            if (GUI.Button(new Rect(0.0f, 0.0f, Screen.width * 0.1f, Screen.height * 0.1f), "Quit"))
            {
                NetworkProvider.Instance.LeaveGame();
            }
        }
	}
    
    
    void DoMultiplayerUpdate() {
        if (NetworkProvider.Instance == null)
            return;

        // In a multiplayer game, time counts up!
        _timePlayed += Time.deltaTime;
        guiObject.SetTime(_timePlayed);

        if (Time.time > _nextTimeoutCheck)
        {
            CheckForTimeOuts();
            _nextTimeoutCheck = Time.time + _timeOutCheckInterval;
        }

        if (Time.time > _nextBroadcastTime)
        {
            Transform transform = myCar.GetComponent<Transform>();
            Rigidbody2D rigidbody2D = myCar.GetComponent<Rigidbody2D>();
            NetworkProvider.Instance.SendMyUpdate(
                transform.position.x,
                transform.position.y,
                rigidbody2D.velocity,
                transform.rotation.eulerAngles.z);
            _nextBroadcastTime = Time.time + BROADCAST_INTERVAL;
        }
    }
	
    void DoSinglePlayerUpdate()
    {
        _timeLeft -= Time.deltaTime;
        guiObject.SetTime(_timeLeft);
        if (_timeLeft <= 0)
        {
            ShowGameOver(false);
        }
    }

	void Update () {
		if (_paused) {
			return;
		}

		if (_multiplayerGame) {
            DoMultiplayerUpdate();
		} else {
            DoSinglePlayerUpdate();
		}

		float carAngle = Mathf.Atan2 (myCar.transform.position.y, myCar.transform.position.x) + Mathf.PI;
		if (carAngle >= _nextCarAngleTarget && (carAngle - _nextCarAngleTarget) < Mathf.PI / 4) {
			_nextCarAngleTarget += Mathf.PI / 2;
			if (Mathf.Approximately(_nextCarAngleTarget, 2*Mathf.PI)) _nextCarAngleTarget = 0;
			if (Mathf.Approximately(_nextCarAngleTarget, FINISH_TARGET)) {
				_lapsRemaining -= 1;
				Debug.Log("Next lap finished!");
				guiObject.SetLaps (_lapsRemaining);
				myCar.GetComponent<CarController>().PlaySoundForLapFinished();
				if (_lapsRemaining <= 0) {
					if (_multiplayerGame) {
                        myCar.GetComponent<CarController>().Stop();
                        NetworkProvider.Instance.SendMyUpdate(myCar.transform.position.x,
                            myCar.transform.position.y,
                            new Vector2(0, 0),
                            myCar.transform.rotation.eulerAngles.z);
                        NetworkProvider.Instance.SendFinishMessage(_timePlayed);
                        PlayerFinished(_myParticipantId, _timePlayed);
					} else {
						ShowGameOver(true);
					}
				}
			}
		}

	}

    public void UpdateReceived(string senderId, int messageNum, float posX, float posY, float velX, float velY, float rotZ)
    {
        if (_multiplayerReady)
        {
            OpponentCarController opponent = _opponentScripts[senderId];
            if (opponent != null)
            {
                opponent.SetCarInformation(messageNum, posX, posY, velX, velY, rotZ);
            }
        }
    }

    public void PlayerFinished(string senderId, float finalTime)
    {
        if (_finishTimes[senderId] < 0)
        {
            _finishTimes[senderId] = finalTime;
        }
        CheckForMPGameOver();
    }

    public void LeftRoomConfirmed()
    {
        NetworkProvider.Instance.updateListener = null;
        Application.LoadLevel("MainMenu");
    }

    private void CheckForMPGameOver()
    {
        float myTime = _finishTimes[_myParticipantId];
        int fasterThanMe = 0;
        foreach(float nextTime in _finishTimes.Values)
        {
            if (nextTime < 0)
                return;

            if (nextTime < myTime)
                fasterThanMe++;
        }
        string[] places = new string[] { "1st", "2nd", "3rd", "4th" };
        gameOvertext = "Game over! You are in " + places[fasterThanMe] + " place!";
        PauseGame();
        _showingGameOver = true;

        Invoke("LeaveMPGame", 3.0f);
    }

    public void LeaveMPGame()
    {
        NetworkProvider.Instance.LeaveGame();
    }

    public void PlayerLeftRoom(string participantId)
    {
        if (_finishTimes[participantId] < 0)
        {
            _finishTimes[participantId] = 999999.0f;
            CheckForMPGameOver();
        }
    }

    private void CheckForTimeOuts()
    {
        foreach (string participantId in _opponentScripts.Keys)
        {
            if (_finishTimes[participantId] < 0)
            {
                if (_opponentScripts[participantId].lastUpdateTime < Time.time - timeOutThreshold)
                {
                    PlayerLeftRoom(participantId);
                }
            }
        }
    }
}
