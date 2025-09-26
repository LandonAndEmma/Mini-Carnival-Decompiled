using UnityEngine;

public class COMA_PlayerSelf_Square : COMA_PlayerSelf
{
	private GameObject cmrParentObj;

	protected new void Start()
	{
		base.Start();
		cmrParentObj = GameObject.Find("CameraNode").gameObject;
		OnRelive();
	}

	protected new void OnEnable()
	{
		if (COMA_Network.Instance.IsConnected())
		{
			SceneTimerInstance.Instance.Add(0.1f, base.SendTransform);
		}
		base.OnEnable();
	}

	protected new void OnDisable()
	{
		if (COMA_Network.Instance.IsConnected())
		{
			SceneTimerInstance.Instance.Remove(base.SendTransform);
		}
		base.OnDisable();
	}

	private new void Update()
	{
		UpdateShadow();
		if (COMA_Sys.Instance.bCoverUpdate)
		{
			return;
		}
		Vector3 spdV = cmrParentObj.transform.forward * moveInput.y + cmrParentObj.transform.right * moveInput.x;
		if (Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.y) > 0.2f)
		{
			float num = speedRun + gunCom.config.movespeedadd;
			moveCur = spdV.normalized * num;
		}
		else
		{
			moveInput = Vector2.zero;
			moveCur = Vector3.zero;
		}
		if (cCtl.isGrounded && movePsv.y <= 0f)
		{
			if (moveCur == Vector3.zero)
			{
				characterCom.PlayMyAnimation("Idle", gunCom.name);
			}
			else
			{
				characterCom.PlayMyAnimation("Run", gunCom.name, "Front");
				TurnAround(spdV);
			}
		}
		else
		{
			movePsv += Physics.gravity * Time.deltaTime;
		}
		if (cCtl.Move((moveCur + movePsv) * Time.deltaTime) != CollisionFlags.None)
		{
			movePsv = Vector3.zero;
		}
		RotateWaist();
		cmrParentObj.transform.position = Vector3.Lerp(cmrParentObj.transform.position, base.transform.position, 0.1f);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.name == "PFB_Item_Gold")
		{
			COMA_Pref.Instance.AddGold(1);
			GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Get_money/Get_money_01")) as GameObject;
			gameObject.transform.position = base.transform.position + Vector3.up;
			Object.DestroyObject(gameObject, 2f);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_Gold, base.transform, 2f, false);
			Object.DestroyObject(other.gameObject);
		}
	}

	protected override void RotatePlayer(float _x, float _y)
	{
		float y = _x * 314f * 2f / (float)Screen.width;
		Quaternion quaternion = Quaternion.Euler(0f, y, 0f);
		cmrParentObj.transform.rotation *= quaternion;
	}

	public override bool OnRelive()
	{
		base.transform.position = bornPointTrs.position;
		base.transform.rotation = bornPointTrs.rotation;
		return base.OnRelive();
	}
}
