using UnityEngine;

public class COMA_Run_Camera : COMA_Camera
{
	public Transform PosTrs
	{
		get
		{
			return posTrs;
		}
	}

	public Transform RotTrs
	{
		get
		{
			return rotTrs;
		}
	}

	private void Start()
	{
		posTrs = base.transform;
		rotTrs = base.transform.FindChild("CameraRot");
	}

	public override void CameraInit(Transform targetTrs)
	{
		COMA_PlayerSelf_Run component = targetTrs.GetComponent<COMA_PlayerSelf_Run>();
		component.cmrCom = this;
	}
}
