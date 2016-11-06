using UnityEngine;
using System.Collections.Generic;
using GooglePlayGames.BasicApi.Multiplayer;
using System;

public class GameController : MonoBehaviour, MPUpdateListener
{
    private static GameController _instance = null;
    public static GameController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameController>();
            }
            return _instance;
        }
    }

    public static int FINISH_LINE_LAYER = -1;

    private const float BROADCAST_INTERVAL = 0.16f;
    public GameObject opponentPrefab;

    private bool _multiplayerReady;
    private string _myParticipantId = "player";
    private Dictionary<string, NetworkedCarController> _opponentScripts;
    private Dictionary<string, float> _finishTimes;
    private float _nextBroadcastTime = 0;

	public GameObject myCar;
	public GuiController guiObject;
	public GUISkin guiSkin;
	public float[] startTimesPerLevel;
	public int[] lapsPerLevel;
    public AudioSource SFXPlaybackSource;
    public List<GameRules> rules;

    public bool _paused;

    private GameRules _currentRule;
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

    private List<VehicleBase> _vehicles;
    public List<VehicleBase> Vehicles { get { return _vehicles; } }

    // Use this for initialization
    void Start ()
    {
        FINISH_LINE_LAYER = LayerMask.NameToLayer("FinishLine");

        _trackController = FindObjectOfType<RaceTrackController>();
        _trackController.onLapFinished = OnLapFinished;
        _trackController.onRaceFinished = OnRaceFinished;

		RetainedUserPicksScript userPicksScript = RetainedUserPicksScript.Instance;
		_multiplayerGame = userPicksScript.multiplayerGame;
		if (! _multiplayerGame) {
            SetupSinglePlayerGame();
		} else {
            SetupMultiplayerGame();
		}
	}

    private void OnRuleFinished()
    {
        AdvanceToNextRule();
    }

    private void AdvanceToNextRule()
    {
        if (rules.Count > 0)
        {
            _currentRule = rules[rules.Count - 1];
            rules.Remove(_currentRule);

            _currentRule.OnFinish = OnRuleFinished;
            _currentRule.Begin();
        }
        else
        {
            _currentRule = null;
            // FINISHED ALL THE RULES!!!
        }
    }

    private void OnLapFinished(string id)
    {
        if (id == _myParticipantId)
        {
            _lapsRemaining -= 1;
            Debug.Log("Next lap finished!");
            guiObject.SetLaps(_lapsRemaining);
            myCar.GetComponent<CarController>().PlaySoundForLapFinished();
        }
    }

    private void OnRaceFinished(string id)
    {
        if (_multiplayerGame)
        {
            myCar.GetComponent<CarController>().Stop();
            NetworkProvider.Instance.SendMyUpdate(myCar.transform.position.x,
                myCar.transform.position.y,
                new Vector2(0, 0),
                myCar.transform.rotation.eulerAngles.z);
            NetworkProvider.Instance.SendFinishMessage(_timePlayed);
            PlayerFinished(_myParticipantId, _timePlayed);
        }
        else {
            ShowGameOver(true);
        }
    }

    void SetupSinglePlayerGame()
    {
        RetainedUserPicksScript userPicksScript = RetainedUserPicksScript.Instance;

        // Can we get the car number from the previous menu?
        CarController car = myCar.GetComponent<CarController>();
        car.SetCarChoice(userPicksScript.carSelected, false);
        car.id = _myParticipantId;
        
        // Set our time left and laps remaining
        _timeLeft = startTimesPerLevel[userPicksScript.diffSelected];
        _lapsRemaining = lapsPerLevel[userPicksScript.diffSelected];

        guiObject.SetTime(_timeLeft);
        guiObject.SetLaps(_lapsRemaining);
        _trackController.targetLaps = _lapsRemaining;
        
        car.SpawnAtStart(0);

        VehicleBase[] cars = FindObjectsOfType<VehicleBase>();
        _vehicles = new List<VehicleBase>(cars.Length);
        for (int i = 0; i < cars.Length; ++i)
        {
            _vehicles.Add(cars[i]);
        }

        // Rules are created with the expectation that top will execute first, but data works better to remove from the end
        rules.Reverse();
        AdvanceToNextRule();
    }

    void SetupMultiplayerGame() {
        NetworkProvider.Instance.updateListener = this;
        _myParticipantId = NetworkProvider.Instance.GetMyPlayerId();
        List<NetworkPlayer> allPlayers = NetworkProvider.Instance.GetAllPlayers();
        _opponentScripts = new Dictionary<string, NetworkedCarController>(allPlayers.Count - 1);
        _finishTimes = new Dictionary<string, float>(allPlayers.Count);
        for (int i = 0; i < allPlayers.Count; i++)
        {
            string nextParticipantId = allPlayers[i].PlayerId;
            _finishTimes[nextParticipantId] = -1;
            Debug.Log("Setting up car for " + nextParticipantId);
            Vector3 carStartPoint = _trackController.SpawnPoint(i);
            if (nextParticipantId == _myParticipantId)
            {
                CarController car = myCar.GetComponent<CarController>();
                car.SetCarChoice(i + 1, true);
                car.SpawnAtStart(i);
            }
            else
            {
                GameObject opponentCar = (Instantiate(opponentPrefab, carStartPoint, Quaternion.identity) as GameObject);
                NetworkedCarController opponentScript = opponentCar.GetComponent<NetworkedCarController>();
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
	}

    public void UpdateReceived(string senderId, int messageNum, float posX, float posY, float velX, float velY, float rotZ)
    {
        if (_multiplayerReady)
        {
            NetworkedCarController opponent = _opponentScripts[senderId];
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
