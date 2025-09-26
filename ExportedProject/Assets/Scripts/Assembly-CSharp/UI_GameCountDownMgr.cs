using UnityEngine;

public class UI_GameCountDownMgr : MonoBehaviour
{
	private static UI_GameCountDownMgr _instance;

	[SerializeField]
	private TUILabel _timeLabel;

	[SerializeField]
	private float _fCurTime;

	private bool _bStarted;

	private int _nElapsedSecond;

	private int _nDurTimeSecond;

	private bool timeLock;

	[SerializeField]
	private bool _bTest;

	[SerializeField]
	private int _nTime;

	[SerializeField]
	private bool _bPause;

	[SerializeField]
	private bool _bResume;

	public static UI_GameCountDownMgr Instance
	{
		get
		{
			return _instance;
		}
	}

	private void OnEnable()
	{
		_instance = this;
	}

	private void OnDisable()
	{
		_instance = null;
	}

	public void StartGameCountDown(int nDurTime)
	{
		Debug.Log("------------AudioCategory.UI_TimeCountDown");
		_bStarted = true;
		_fCurTime = 0f;
		_nElapsedSecond = 0;
		_nDurTimeSecond = nDurTime;
		_timeLabel.Text = GetFormatTime();
	}

	public void PauseCountDown()
	{
		_bStarted = false;
	}

	public void ResumeCountDown()
	{
		_bStarted = true;
	}

	private string GetFormatTime()
	{
		string empty = string.Empty;
		int num = _nDurTimeSecond - _nElapsedSecond;
		int num2 = num / 60;
		int num3 = num % 60;
		empty = string.Format("{0:00}", num2);
		empty += ":";
		string text = string.Format("{0:00}", num3);
		return empty + text;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (_bTest)
		{
			StartGameCountDown(_nTime);
			_bTest = false;
		}
		if (_bPause)
		{
			PauseCountDown();
			_bPause = false;
		}
		if (_bResume)
		{
			ResumeCountDown();
			_bResume = false;
		}
		if (!_bStarted || timeLock)
		{
			return;
		}
		_fCurTime += Time.deltaTime;
		if (_fCurTime >= (float)(_nElapsedSecond + 1))
		{
			_nElapsedSecond++;
			_timeLabel.Text = GetFormatTime();
			if (_nElapsedSecond >= _nDurTimeSecond)
			{
				_bStarted = false;
				timeLock = true;
				Debug.Log("Timeout");
				UI_GameEndMgr.Instance.EnterGameEnd();
			}
		}
	}
}
