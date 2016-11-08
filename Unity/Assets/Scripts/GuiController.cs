using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GuiController : MonoBehaviour
{
    private static GuiController _instance = null;
    public static GuiController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GuiController>();
            }
            return _instance;
        }
    }

    public GUIText lapText;
	public GUIText timeText;
    [SerializeField]
    private Text _centerNotification;
    private Vector3 _centerNotificationPosition;

    private int _laps;
	private float _timeToShow;

	// Use this for initialization
	void Start ()
    {
        _centerNotification.gameObject.SetActive(false);
        _centerNotificationPosition = _centerNotification.transform.localPosition;
    }

	public void SetLaps(int laps) {
		_laps = laps;
	}

	public void SetTime(float timeToShow) {
		_timeToShow = timeToShow;
	}

    public void ShowNotification(string text, float duration)
    {
        StopAllCoroutines();
        _centerNotification.text = text;
        StartCoroutine(FadeNotificationRoutine(duration));
        StartCoroutine(MoveUpRoutine(duration));
    }

    public IEnumerator FadeNotificationRoutine(float duration)
    {
        float time = duration * 0.33f;
        WaitForSeconds wait = new WaitForSeconds(time);
        _centerNotification.CrossFadeAlpha(0, 0.01f, true);
        yield return new WaitForSeconds(0.01f);
        _centerNotification.gameObject.SetActive(true);

        _centerNotification.CrossFadeAlpha(1f, time, true);
        yield return wait;
        // Now hold
        yield return wait;
        // Now fade out
        _centerNotification.CrossFadeAlpha(0f, time, true);
        // Turn off
        yield return wait;
        _centerNotification.gameObject.SetActive(false);
    }

    public IEnumerator MoveUpRoutine(float duration)
    {
        _centerNotification.transform.localPosition = _centerNotificationPosition;
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        float time = Time.time;
        while(Time.time < time + duration)
        {
            _centerNotification.transform.Translate(Vector3.up * Time.deltaTime * 10f); 
            yield return wait;
        }
    }

	// Update is called once per frame
	void Update () {
		lapText.text = "Laps: " + _laps;
		timeText.text = "Time: " + _timeToShow.ToString("F1");

	}
}
