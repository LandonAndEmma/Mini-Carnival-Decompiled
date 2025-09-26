using UnityEngine;

public class COMA_Fishing_FishFloat : TBaseEntity
{
	public enum EState
	{
		Disable = 0,
		Idle = 1,
		Shake = 2,
		Rest = 3,
		SYN = 4
	}

	[SerializeField]
	private float _fMaxResetIntervalTime = 10f;

	[SerializeField]
	private float _fMinShakeDur = 0.5f;

	[SerializeField]
	private float _fMaxShakeDur = 3f;

	[SerializeField]
	private Animation _aniCmp;

	[SerializeField]
	private EState _curState;

	[SerializeField]
	private float _fStart10Time;

	[SerializeField]
	private float _fShakeTime;

	[SerializeField]
	private float _fShakeDurTime;

	private void Reset10Time()
	{
		_fStart10Time = Time.time;
		_fShakeDurTime = Random.Range(_fMinShakeDur, _fMaxShakeDur);
		_fShakeTime = _fStart10Time + Random.Range(0f, _fMaxResetIntervalTime - _fShakeDurTime);
		Debug.Log("Shake Buff: _fShakeTime=" + _fShakeTime + "  _fShakeDurTime=" + _fShakeDurTime);
		_aniCmp.Play("Idle");
		ChangeStateTo(EState.Idle);
	}

	public void EnableFishFloat(Vector3 v)
	{
		base.transform.position = v;
		Reset10Time();
	}

	public override bool HandleMessage(TTelegram msg)
	{
		switch (msg._nMsgId)
		{
		case 7:
			SceneTimerInstance.Instance.Remove(RecoverIdle);
			_aniCmp.Play("Idle");
			ChangeStateTo(EState.Rest);
			break;
		case 8:
			SceneTimerInstance.Instance.Remove(RecoverIdle);
			_aniCmp.Stop();
			ChangeStateTo(EState.Disable);
			break;
		case 12:
			SceneTimerInstance.Instance.Remove(RecoverIdle);
			_aniCmp.Play("Shake02");
			break;
		}
		return true;
	}

	protected bool RecoverIdle()
	{
		_aniCmp.Play("Idle");
		return false;
	}

	private void Awake()
	{
		ChangeStateTo(EState.Disable);
	}

	private void Start()
	{
	}

	private new void Update()
	{
		if (_curState == EState.SYN)
		{
			return;
		}
		if (_curState == EState.Disable)
		{
			base.transform.position = new Vector3(-10000f, -10000f, -10000f);
		}
		else
		{
			if (_curState == EState.Rest)
			{
				return;
			}
			base.Update();
			if (Time.time - _fStart10Time >= _fMaxResetIntervalTime)
			{
				Reset10Time();
			}
			switch (_curState)
			{
			case EState.Idle:
				if (Time.time >= _fShakeTime)
				{
					Debug.Log("--------------------------->To Shake!!!!!");
					_aniCmp.Play("Shake01");
					ChangeStateTo(EState.Shake);
					TBuff tBuff = new TBuff();
					tBuff.SetBuffType(0);
					tBuff.SetStartTime(Time.time);
					tBuff.SetDurTime(_fShakeDurTime);
					int iDByName = TFishingAddressBook.Instance.GetIDByName(0);
					TMessageDispatcher.Instance.DispatchMsg(-2, iDByName, 10001, TTelegram.SEND_MSG_IMMEDIATELY, tBuff);
					TMessageDispatcher.Instance.DispatchMsg(-2, iDByName, 10002, _fShakeDurTime, tBuff);
					SceneTimerInstance.Instance.Add(_fShakeDurTime, RecoverIdle);
				}
				break;
			case EState.Shake:
				if (!(Time.time >= _fShakeTime + _fShakeDurTime))
				{
				}
				break;
			}
		}
	}

	private void FixUpdate()
	{
	}

	public void ChangeStateTo(EState state)
	{
		_curState = state;
	}
}
