using UnityEngine;

public class COMA_DropFight_Camera : COMA_Camera
{
	public Vector3 posGodMode = Vector3.zero;

	public Quaternion rotGodMode = Quaternion.identity;

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
		posGodMode = posTrs.position;
		rotGodMode = rotTrs.rotation;
	}

	public override void CameraInit(Transform targetTrs)
	{
		COMA_PlayerSelf_DropFight component = targetTrs.GetComponent<COMA_PlayerSelf_DropFight>();
		component.cmrCom = this;
	}
}
