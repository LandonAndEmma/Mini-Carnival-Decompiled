using UnityEngine;

public class COMA_Run_Road_MoveX : COMA_Run_Road_Move
{
	private void Tick(float fDela)
	{
		if (!_bCanMove)
		{
			Init();
		}
		if (_bCanMove && COMA_PlayerSelf.Instance != null)
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
			Vector3 position3 = base.transform.position;
			if ((position3 - _curDirInitPos).sqrMagnitude >= _fSqrMaxMoveDis)
			{
				ChangeDir();
			}
		}
	}

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
		Tick(Time.deltaTime);
	}

	private bool Disappear()
	{
		Object.DestroyObject(base.gameObject);
		return false;
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Debug.Log("--------XXX----------ControllerColliderHit");
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
			_fMoveSpeed = 0f;
			Explode();
		}
	}
}
