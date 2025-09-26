using System;
using UnityEngine;

public class COMA_Gun : MonoBehaviour
{
	[NonSerialized]
	public UnityEngine.Object bulletPfb;

	public UnityEngine.Object bulletPfbRed;

	public UnityEngine.Object bulletPfbBlue;

	public string bulletName = string.Empty;

	public Transform bulletInitLoc;

	public string flashPath = string.Empty;

	[NonSerialized]
	public COMA_DataConfig_Gun config = new COMA_DataConfig_Gun();

	private GameObject energyLaserObj;

	private void Start()
	{
		if (base.name == "W000" || base.name.StartsWith("W025") || base.name.StartsWith("W026") || base.name.StartsWith("W027"))
		{
			return;
		}
		if (bulletName == "B011" || bulletName == "B013" || bulletName == "B014" || bulletName == "B016")
		{
			bulletPfbBlue = Resources.Load("FBX/Magazine/Bullet/PFB/" + bulletName + "_B");
			bulletPfbRed = Resources.Load("FBX/Magazine/Bullet/PFB/" + bulletName + "_R");
			return;
		}
		bulletPfb = Resources.Load("FBX/Magazine/Bullet/PFB/" + bulletName);
		if (bulletPfb == null)
		{
			Debug.LogError("bulletPfb is null!! " + base.name + " " + bulletName);
		}
	}

	public void InitData()
	{
		if (COMA_DataConfig.Instance != null)
		{
			COMA_DataConfig.Instance.GetDataConfig(base.gameObject.name, ref config);
		}
		else
		{
			Debug.LogWarning("COMA_DataConfig.Instance is Not Available!!");
		}
	}

	public void EnergyGunAccumulate(bool isAccumulating)
	{
		if (energyLaserObj == null)
		{
			energyLaserObj = base.transform.FindChild("Energy_gun_Cohesion_01").gameObject;
		}
		if (isAccumulating)
		{
			energyLaserObj.particleSystem.Play(true);
		}
		else
		{
			energyLaserObj.particleSystem.Stop(true);
		}
	}

	public void PlayFlash()
	{
		if (!(flashPath == string.Empty))
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Particle/effect/" + flashPath)) as GameObject;
			gameObject.transform.position = bulletInitLoc.position;
			gameObject.transform.rotation = bulletInitLoc.rotation;
			UnityEngine.Object.DestroyObject(gameObject, 2f);
		}
	}
}
