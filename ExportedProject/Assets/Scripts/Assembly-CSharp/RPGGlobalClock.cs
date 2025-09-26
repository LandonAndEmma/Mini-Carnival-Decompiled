using System;
using UnityEngine;

public class RPGGlobalClock : MonoBehaviour
{
	private static RPGGlobalClock _instance;

	private bool _bTimeStart;

	private float _fTimeStart;

	private float _fSrvTime;

	private uint _iServerTime;

	private float _fTimeServerStart;

	private float _fCurAccumulateTime;

	public static RPGGlobalClock Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	protected void OnEnable()
	{
		_instance = this;
	}

	protected void OnDisable()
	{
		_instance = null;
	}

	public void InitClock(float srvTime)
	{
		_bTimeStart = true;
		_fTimeStart = Time.realtimeSinceStartup;
		_fSrvTime = srvTime;
		Debug.Log("***************" + _fSrvTime + " xxxxxxxxxxxx " + (uint)_fSrvTime);
		_fCurAccumulateTime = 0f;
	}

	public void InitClock(uint srvTime)
	{
		_iServerTime = srvTime;
		_fTimeServerStart = Time.realtimeSinceStartup;
	}

	[Obsolete("Please use GetCorrectSrvTimeUInt32 instead.")]
	public float GetCorrectSrvTime()
	{
		return _fSrvTime;
	}

	public uint GetCorrectSrvTimeUInt32()
	{
		float num = Time.realtimeSinceStartup - _fTimeServerStart;
		return _iServerTime + (uint)num;
	}

	public int GetMobilityValue()
	{
		uint num = GetCorrectSrvTimeUInt32() - UIDataBufferCenter.Instance.RPGData.m_mobility_time;
		int num2 = RPGGlobalData.Instance.RpgMiscUnit._energyRenewTimePerUnit * 60;
		if (num >= num2)
		{
			int value = (int)(num / num2);
			return Mathf.Clamp(value, 0, RPGGlobalData.Instance.RpgMiscUnit._energyValue_Max);
		}
		return 0;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (!_bTimeStart)
		{
			return;
		}
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		float num = realtimeSinceStartup - _fTimeStart;
		_fCurAccumulateTime += num;
		if (_fCurAccumulateTime > 300f)
		{
			_fCurAccumulateTime = 0f;
			UIDataBufferCenter.Instance.FetchServerTime(delegate(uint time)
			{
				Debug.Log("COMA_Server_Account.Instance.svrTime = " + time);
				COMA_Server_Account.Instance.svrTime = time;
				Instance.InitClock(time);
			});
		}
		_fTimeStart = Time.realtimeSinceStartup;
	}
}
