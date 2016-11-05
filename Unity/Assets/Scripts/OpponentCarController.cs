using UnityEngine;
using System.Collections;

public class OpponentCarController : MonoBehaviour
{
    private Vector3 _startPos;
    private Vector3 _destinationPos;
    private Quaternion _startRot;
    private Quaternion _destinationRot;
    private Vector3 _lastKnownVel;
    private float _lastUpdateTime;
    private float _timePerUpdate = 0.16f;
    private int _lastMessageNum;

    public float lastUpdateTime { get { return _lastUpdateTime; } }

    public void Start()
    {
        _startPos = transform.position;
        _startRot = transform.rotation;
        _destinationPos = _startPos;
        _destinationRot = _startRot;
        _lastUpdateTime = Time.time;
        _lastMessageNum = 0;
    }

    public void SetCarNumber(int carNum)
    {
        //GetComponent<SpriteRenderer>().sprite = carSprites[carNum - 1];
    }

    public void SetCarInformation(int messageNum, float posX, float posY, float velX, float velY, float rotZ)
    {
        if (messageNum <= _lastMessageNum)
        {
            return;
        }
        _lastMessageNum = messageNum;

        _startPos = transform.position;
        _startRot = transform.rotation;

        _destinationPos = new Vector3(posX, posY, 0);
        _destinationRot = Quaternion.Euler(0, 0, rotZ);

        _lastUpdateTime = Time.time;
        _lastKnownVel = new Vector3(velX, velY, 0f);
    }

    public void Update()
    {
        float pctDone = (Time.time - _lastUpdateTime) / _timePerUpdate;

        if (pctDone <= 1.0f)
        {
            transform.position = Vector3.Lerp(_startPos, _destinationPos, pctDone);
            transform.rotation = Quaternion.Slerp(_startRot, _destinationRot, pctDone);
        }
        else
        {
            transform.position = transform.position + (_lastKnownVel * Time.deltaTime);
        }
    }

    public void HideCar()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
    }
}
