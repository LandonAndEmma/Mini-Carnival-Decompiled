using UnityEngine;

public class COMA_Run_Road_Move : COMA_Run_RoadCollider
{
	[SerializeField]
	protected CharacterController cCtl;

	[SerializeField]
	protected Vector3 _initMoveDir = new Vector3(1f, 0f, 0f);

	protected Vector3 _positiveDirInitPos;

	protected Vector3 _negativeDirInitPos;

	protected Vector3 _curDirInitPos;

	[SerializeField]
	protected float _fMaxMoveDis = 15f;

	protected float _fSqrMaxMoveDis;

	[SerializeField]
	protected float _fMoveSpeed = 4f;

	protected bool _bCanMove;

	protected float _fStartMoment;

	[SerializeField]
	protected bool _bNeedRot = true;

	protected void ChangeDir()
	{
		_initMoveDir *= -1f;
		if (_curDirInitPos == _positiveDirInitPos)
		{
			_curDirInitPos = _negativeDirInitPos;
		}
		else
		{
			_curDirInitPos = _positiveDirInitPos;
		}
		if (_bNeedRot)
		{
			Quaternion rotation = base.transform.rotation;
			base.transform.rotation = rotation * new Quaternion(0f, 1f, 0f, 0f);
		}
	}

	protected void Init()
	{
		if (COMA_Run_SceneController.Instance != null && COMA_Run_SceneController.Instance.IsCreatedRoad)
		{
			_initMoveDir.Normalize();
			_positiveDirInitPos = base.transform.position;
			_negativeDirInitPos = _positiveDirInitPos + _fMaxMoveDis * _initMoveDir;
			_curDirInitPos = _positiveDirInitPos;
			_fSqrMaxMoveDis = _fMaxMoveDis * _fMaxMoveDis;
			_bCanMove = true;
			_fStartMoment = Time.time;
			Debug.Log(string.Concat("_positiveDirInitPos=", _positiveDirInitPos, "   _negativeDirInitPos=", _negativeDirInitPos));
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public override void Explode()
	{
		if (!COMA_Scene.Instance.runingGameOver)
		{
			Object.DestroyObject(base.gameObject);
			GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Bazooka_Brust/Bazooka_Brust")) as GameObject;
			gameObject.transform.position = base.transform.position;
			Object.DestroyObject(gameObject, 2f);
		}
	}
}
