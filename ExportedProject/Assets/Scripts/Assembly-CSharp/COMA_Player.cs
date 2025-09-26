using UnityEngine;

public class COMA_Player : COMA_Creation
{
	public Team team;

	public COMA_PlayerCharacter characterCom;

	public Transform shadowTrs;

	protected Vector3 localEuler = Vector3.zero;

	protected Vector3 LOCALEULER = Vector3.zero;

	protected COMA_Gun gunCom;

	public COMA_DialogBox dialogBoxCom;

	public bool _bRideRocket;

	protected new void Start()
	{
		if (dialogBoxCom != null)
		{
			dialogBoxCom.playerNickName = nickname;
		}
		base.Start();
		creationKind = CreationKind.Player;
		LOCALEULER = characterCom.boneTrs_Waist.localEulerAngles;
		localEuler = LOCALEULER;
	}

	protected void RotateWaist()
	{
		Quaternion to = Quaternion.Euler(localEuler);
		characterCom.boneTrs_Waist.localRotation = Quaternion.Lerp(characterCom.boneTrs_Waist.localRotation, to, 0.3f);
	}

	protected void UpdateShadow()
	{
		Ray ray = new Ray(base.transform.position + Vector3.up, Vector3.down);
		int layerMask = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Collision")) | (1 << LayerMask.NameToLayer("Death"));
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 100f, layerMask))
		{
			shadowTrs.position = hitInfo.point + Vector3.up * 0.01f;
			float num = 1f / hitInfo.distance;
			if (num > 1f)
			{
				num = 1f;
			}
			shadowTrs.localScale = new Vector3(num, num, num);
		}
	}

	public void HideStart(float alpha)
	{
		IsHidden = true;
		HideAdd(alpha);
		SceneTimerInstance.Instance.Remove(HideEnd);
		SceneTimerInstance.Instance.Add(COMA_Buff.Instance.lastTime_hide, HideEnd);
	}

	public void ColorStart(float r, float g, float b, float last)
	{
		HideAdd(r, g, b, 1f);
		SceneTimerInstance.Instance.Remove(HideEnd);
		SceneTimerInstance.Instance.Add(last, HideEnd);
	}

	public void HideAdd(float r, float g, float b, float alpha)
	{
		Color color = new Color(r, g, b, alpha);
		characterCom.bodyObjs[0].renderer.material.color = color;
		characterCom.bodyObjs[1].renderer.material.color = color;
		characterCom.bodyObjs[2].renderer.material.color = color;
		MeshRenderer[] componentsInChildren = characterCom.GetComponentsInChildren<MeshRenderer>();
		MeshRenderer[] array = componentsInChildren;
		foreach (MeshRenderer meshRenderer in array)
		{
			meshRenderer.material.color = color;
		}
		SkinnedMeshRenderer[] componentsInChildren2 = characterCom.GetComponentsInChildren<SkinnedMeshRenderer>();
		SkinnedMeshRenderer[] array2 = componentsInChildren2;
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in array2)
		{
			skinnedMeshRenderer.material.color = color;
		}
		if (shadowTrs != null)
		{
			shadowTrs.renderer.material.color = color;
		}
	}

	public void HideAdd(float alpha)
	{
		Color color = new Color(1f, 1f, 1f, alpha);
		characterCom.bodyObjs[0].renderer.material.color = color;
		characterCom.bodyObjs[1].renderer.material.color = color;
		characterCom.bodyObjs[2].renderer.material.color = color;
		MeshRenderer[] componentsInChildren = characterCom.GetComponentsInChildren<MeshRenderer>();
		MeshRenderer[] array = componentsInChildren;
		foreach (MeshRenderer meshRenderer in array)
		{
			meshRenderer.material.color = color;
		}
		SkinnedMeshRenderer[] componentsInChildren2 = characterCom.GetComponentsInChildren<SkinnedMeshRenderer>();
		SkinnedMeshRenderer[] array2 = componentsInChildren2;
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in array2)
		{
			skinnedMeshRenderer.material.color = color;
		}
		if (shadowTrs != null)
		{
			shadowTrs.renderer.material.color = color;
		}
	}

	public bool HideEnd()
	{
		IsHidden = false;
		Color color = new Color(1f, 1f, 1f, 1f);
		characterCom.bodyObjs[0].renderer.material.color = color;
		characterCom.bodyObjs[1].renderer.material.color = color;
		characterCom.bodyObjs[2].renderer.material.color = color;
		MeshRenderer[] componentsInChildren = characterCom.GetComponentsInChildren<MeshRenderer>();
		MeshRenderer[] array = componentsInChildren;
		foreach (MeshRenderer meshRenderer in array)
		{
			meshRenderer.material.color = color;
		}
		SkinnedMeshRenderer[] componentsInChildren2 = characterCom.GetComponentsInChildren<SkinnedMeshRenderer>();
		SkinnedMeshRenderer[] array2 = componentsInChildren2;
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in array2)
		{
			skinnedMeshRenderer.material.color = color;
		}
		if (shadowTrs != null)
		{
			shadowTrs.renderer.material.color = color;
		}
		return false;
	}

	public void HideTerminate()
	{
		SceneTimerInstance.Instance.Remove(HideEnd);
		HideEnd();
	}

	public virtual void CharacterCall_Fire()
	{
	}

	public virtual void CharacterCall_Fire2()
	{
	}

	public virtual void Chat(string c)
	{
		if (dialogBoxCom != null)
		{
			dialogBoxCom.Chatting(c);
		}
	}

	public virtual void NotifyPlayerExtraProcess(COMA_CommandDatas commandDatas, string aniName)
	{
	}
}
