using UnityEngine;

public class COMA_Run_Road_MoveDown : MonoBehaviour
{
	private enum EMoveState
	{
		Disable = 0,
		Idle = 1,
		Warning = 2,
		Predown = 3,
		downing = 4,
		keep = 5,
		disappear = 6
	}

	[SerializeField]
	private float _fRespawnWaitingTime = 3f;

	[SerializeField]
	private float _fStartIdleDis = 20f;

	[SerializeField]
	private float _fDelaDownTime = 1.5f;

	private float _fAheadWarnTime = 1f;

	private float _fStartMoment;

	private float _fEndMoment;

	private float _fKeepTime = 3f;

	private float _fKeepStartMoment;

	private EMoveState _moveState;

	private string[] strAniName = new string[3] { "Box_falldown01", "Box_falldown02", "Box_falldown03" };

	private Vector3 _InitPos = Vector3.zero;

	private float fDownDis = 10f;

	private void Awake()
	{
	}

	private void Hide()
	{
		Vector3 position = base.transform.position;
		position.y = 10000f;
		base.transform.position = position;
	}

	private void Show()
	{
		base.transform.position = _InitPos + new Vector3(0f, fDownDis, 0f);
	}

	private void StartDownInit()
	{
		base.transform.position = _InitPos;
	}

	private void Start()
	{
		_InitPos = base.transform.position;
		Hide();
	}

	private void Update()
	{
		switch (_moveState)
		{
		case EMoveState.Disable:
			if (COMA_PlayerSelf.Instance != null && base.transform.position.z - COMA_PlayerSelf.Instance.transform.position.z <= _fStartIdleDis)
			{
				Spawn();
			}
			break;
		case EMoveState.Idle:
			if (Time.time >= _fStartMoment + _fDelaDownTime - _fAheadWarnTime)
			{
				_moveState = EMoveState.Warning;
			}
			break;
		case EMoveState.Warning:
		{
			GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Stone_Remind/Stone_Remind")) as GameObject;
			gameObject.transform.position = _InitPos;
			Object.DestroyObject(gameObject, 1f);
			_moveState = EMoveState.Predown;
			break;
		}
		case EMoveState.Predown:
			if (Time.time - _fStartMoment >= _fDelaDownTime)
			{
				if (base.animation != null)
				{
					StartDownInit();
					int num = Random.Range(0, 3);
					base.animation.Play(strAniName[num]);
				}
				_moveState = EMoveState.downing;
			}
			break;
		case EMoveState.downing:
			break;
		case EMoveState.keep:
			if (Time.time - _fKeepStartMoment >= _fKeepTime)
			{
				Disappear();
			}
			break;
		case EMoveState.disappear:
			if (Time.time - _fEndMoment >= _fRespawnWaitingTime)
			{
				Spawn(true);
			}
			break;
		}
	}

	private void Spawn(bool bRespawn)
	{
		if (bRespawn)
		{
			Show();
		}
		_fStartMoment = Time.time;
		_moveState = EMoveState.Idle;
	}

	private void Spawn()
	{
		Spawn(false);
	}

	private void Disappear()
	{
		_moveState = EMoveState.disappear;
		_fEndMoment = Time.time;
	}

	public void DowningEnd()
	{
		_fKeepStartMoment = Time.time;
		_moveState = EMoveState.keep;
	}
}
