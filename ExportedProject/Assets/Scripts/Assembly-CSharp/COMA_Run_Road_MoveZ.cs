using UnityEngine;

public class COMA_Run_Road_MoveZ : COMA_Run_Road_Move
{
	private enum EMoveState
	{
		PreIdle = 0,
		Idle = 1,
		keep = 2,
		disappear = 3,
		disappear1 = 4
	}

	[SerializeField]
	private float _fRespawnWaitingTime = 3f;

	[SerializeField]
	private float _fSpawnWaitingTime;

	private EMoveState _moveState;

	private float _fEndMoment;

	private float _fKeepTime;

	private float _fKeepStartMoment;

	private float _fPreMoveSpeed;

	private void Awake()
	{
	}

	private void Start()
	{
		base.animation["Move"].layer = 1;
		if (base.animation["Attack"] != null)
		{
			base.animation["Attack"].layer = 2;
		}
		if (base.animation["Hit"] != null)
		{
			base.animation["Hit"].layer = 2;
		}
	}

	private void Tick(float fDela)
	{
		if (!_bCanMove)
		{
			Init();
		}
		if (!_bCanMove || !(COMA_PlayerSelf.Instance != null))
		{
			return;
		}
		switch (_moveState)
		{
		case EMoveState.PreIdle:
			if (Time.time - _fStartMoment >= _fSpawnWaitingTime)
			{
				_moveState = EMoveState.Idle;
			}
			break;
		case EMoveState.Idle:
		{
			Vector3 vector = _fMoveSpeed * _initMoveDir;
			if (cCtl != null)
			{
				cCtl.Move(vector * fDela);
			}
			else
			{
				Vector3 position = base.transform.position;
				Vector3 position2 = position + vector * fDela;
				base.transform.position = position2;
			}
			if ((base.transform.position - _curDirInitPos).sqrMagnitude >= _fSqrMaxMoveDis)
			{
				_moveState = EMoveState.keep;
				_fKeepStartMoment = Time.time;
			}
			break;
		}
		case EMoveState.keep:
			if (Time.time - _fKeepStartMoment >= _fKeepTime)
			{
				Disappear();
			}
			break;
		case EMoveState.disappear:
			if (Time.time - _fEndMoment >= _fRespawnWaitingTime - 1f)
			{
				SpawnDisappearEffect(true);
				_moveState = EMoveState.disappear1;
			}
			break;
		case EMoveState.disappear1:
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
			if (_fPreMoveSpeed != 0f)
			{
				_fMoveSpeed = _fPreMoveSpeed;
			}
			base.transform.position = _positiveDirInitPos;
		}
		_moveState = EMoveState.Idle;
	}

	private void Spawn()
	{
		Spawn(false);
	}

	private void SpawnDisappearEffect(bool bInitPos)
	{
		if (COMA_PlayerSelf.Instance != null)
		{
			Vector3 vector = base.transform.position;
			if (bInitPos)
			{
				vector = _positiveDirInitPos;
			}
			if (vector.z - COMA_PlayerSelf.Instance.transform.position.z < 50f || COMA_PlayerSelf.Instance.transform.position.z - vector.z < 5f)
			{
				GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Appear/Appear")) as GameObject;
				gameObject.transform.position = vector + new Vector3(0f, 0f, 0.5f);
				Object.DestroyObject(gameObject, 1f);
			}
		}
	}

	private void SpawnDisappearEffect()
	{
		SpawnDisappearEffect(false);
	}

	private bool Disappear()
	{
		SpawnDisappearEffect();
		base.transform.position = new Vector3(-1000f, -1000f, -1000f);
		_moveState = EMoveState.disappear;
		_fEndMoment = Time.time;
		return false;
	}

	private void Update()
	{
		Tick(Time.deltaTime);
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Debug.Log("------------------ControllerColliderHit");
		COMA_PlayerSelf_Run component = hit.collider.gameObject.GetComponent<COMA_PlayerSelf_Run>();
		if (component != null)
		{
			NotifyCollideHitPlayer();
		}
	}

	public override void NotifyCollideHitPlayer()
	{
		if (!bProcessHitPlayer)
		{
			bProcessHitPlayer = true;
			_fPreMoveSpeed = _fMoveSpeed;
			_fMoveSpeed = 0f;
			Explode();
		}
	}
}
