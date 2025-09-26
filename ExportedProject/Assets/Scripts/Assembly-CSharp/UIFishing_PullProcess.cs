using UnityEngine;

public class UIFishing_PullProcess : MonoBehaviour
{
	[SerializeField]
	private Transform _slider;

	private float _fOffsetX;

	private float _fScale1Len;

	private float _fStartTime;

	private float _curProcess;

	[SerializeField]
	private float _fCurDur;

	[SerializeField]
	private GameObject _objContainer;

	[SerializeField]
	private float _fMaxValue = 100f;

	[SerializeField]
	private float _fPerSecAttenuation = 5f;

	[SerializeField]
	private float _fPerClickAdd = 10f;

	[SerializeField]
	private float _fMaxTimeDur = 5f;

	private bool _bHasResult;

	private void Awake()
	{
		float num = 27f;
		float num2 = 121f;
		float num3 = num2 - num;
		_fScale1Len = num3 / 9.5f;
		_fOffsetX = num - _fScale1Len / 2f;
	}

	private void OnEnable()
	{
		_curProcess = 0f;
		SetProcess(_curProcess);
		_fStartTime = Time.time;
		_bHasResult = false;
		_fCurDur = 0f;
		Debug.Log("RESET PULL PROCESS!");
	}

	private void OnDisable()
	{
	}

	private void Start()
	{
	}

	public void AddProcess()
	{
		_curProcess += _fPerClickAdd / _fMaxValue;
		SetProcess(_curProcess);
	}

	private void SetProcess(float f)
	{
		f = Mathf.Clamp01(f);
		float num = f * 20f;
		float x = num * _fScale1Len / 2f + _fOffsetX;
		Vector3 localPosition = _slider.localPosition;
		localPosition.x = x;
		_slider.localPosition = localPosition;
		Vector3 localScale = _slider.localScale;
		localScale.x = num;
		_slider.localScale = localScale;
		if (f >= 1f)
		{
			_bHasResult = true;
			int iDByName = TFishingAddressBook.Instance.GetIDByName(0);
			TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 9, 0.3f, null);
		}
	}

	private void Update()
	{
		if (!_bHasResult)
		{
			if (_fCurDur >= _fMaxTimeDur)
			{
				_bHasResult = true;
				int iDByName = TFishingAddressBook.Instance.GetIDByName(0);
				TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 10, 0.3f, null);
			}
			if (Time.time - _fStartTime >= 1f)
			{
				_fStartTime = Time.time;
				_fPerSecAttenuation = Random.Range(5, 15);
				Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>_fPerSecAttenuation=" + _fPerSecAttenuation);
				_curProcess -= _fPerSecAttenuation / _fMaxValue;
				SetProcess(_curProcess);
				_fCurDur += 1f;
			}
		}
	}
}
