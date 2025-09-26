using UnityEngine;

public class COMA_PlayerSync_Blood : COMA_PlayerSync
{
	private BloodBar bloodBarCom;

	private GameObject cameraObj;

	protected new void OnEnable()
	{
		base.OnEnable();
	}

	protected new void OnDisable()
	{
		base.OnDisable();
	}

	protected new void Start()
	{
		base.Start();
		Vector3 position = base.transform.position + new Vector3(0f, 2.4f, 0f);
		GameObject gameObject = Object.Instantiate(Resources.Load("FBX/Common/BloodBar/PFB_BloodBar_Frame"), position, Quaternion.identity) as GameObject;
		gameObject.transform.parent = base.transform;
		bloodBarCom = gameObject.GetComponent<BloodBar>();
		if (team != (Team)(1 + COMA_Network.Instance.TNetInstance.Myself.SitIndex % COMA_Scene.Instance.teamsNum))
		{
			bloodBarCom.InitBloodBar(BloodBar.BarColor.Red);
		}
		else
		{
			bloodBarCom.InitBloodBar(BloodBar.BarColor.Blue);
		}
		cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
	}

	protected new void Update()
	{
		UpdateShadow();
		if (!(COMA_PlayerSelf.Instance == null))
		{
			base.Update();
			if (cameraObj != null)
			{
				bloodBarCom.transform.forward = cameraObj.transform.position - bloodBarCom.transform.position;
			}
		}
	}

	protected override void onHpChange(COMA_CommandDatas commandDatas)
	{
		COMA_CD_PlayerHPSet cOMA_CD_PlayerHPSet = commandDatas as COMA_CD_PlayerHPSet;
		base.hp = cOMA_CD_PlayerHPSet.hp;
		bloodBarCom.SetBloodBar(base.hp / HP);
	}

	public void RedShine()
	{
		HideAdd(1f, 0f, 0f, 1f);
		SceneTimerInstance.Instance.Remove(RecoverRedShine);
		SceneTimerInstance.Instance.Add(COMA_Buff.Instance.lastTime_blood_shotRed, RecoverRedShine);
	}

	public bool RecoverRedShine()
	{
		HideAdd(1f, 1f, 1f, 1f);
		return false;
	}
}
