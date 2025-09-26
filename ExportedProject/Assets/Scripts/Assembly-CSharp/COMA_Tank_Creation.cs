using System.Collections;
using UnityEngine;

public class COMA_Tank_Creation : COMA_Tank_Breakable
{
	public int nScore = 10;

	public float _fDamageRadius;

	public float _fDamage;

	public bool oilbox;

	public bool wall;

	public bool justbox;

	public bool crystal;

	public bool ironWall;

	private void OnDrawGizmos()
	{
		if (_fDamageRadius > 0f)
		{
			Gizmos.DrawWireSphere(base.transform.position, _fDamageRadius);
		}
	}

	protected override bool canbeDamaged(COMA_PlayerSelf from)
	{
		return !ironWall;
	}

	protected new void OnEnable()
	{
		base.OnEnable();
		_nDestoryScore = nScore;
	}

	private void RecieveCreate(COMA_CommandDatas data)
	{
	}

	private void RecieveDelete(COMA_CommandDatas data)
	{
		COMA_CD_DeleteItem cOMA_CD_DeleteItem = data as COMA_CD_DeleteItem;
		if (cOMA_CD_DeleteItem.blockIndex == (byte)base.ObstacleID)
		{
			Object.Destroy(base.gameObject);
		}
	}

	protected override void onDestoryed(bool bFromNet)
	{
		StartCoroutine(doDestory(bFromNet));
	}

	private IEnumerator doDestory(bool bFromNet)
	{
		yield return new WaitForSeconds((!bFromNet) ? 0f : 0.1f);
		if (GetComponent<MeshRenderer>() != null)
		{
			GetComponent<MeshRenderer>().enabled = false;
		}
		if (GetComponent<BoxCollider>() != null)
		{
			GetComponent<BoxCollider>().enabled = false;
		}
		MeshRenderer[] componentsInChildren = GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRender in componentsInChildren)
		{
			if (meshRender.renderer.name == "sub" || meshRender.renderer.name == "Wall_shadow")
			{
				meshRender.enabled = false;
			}
		}
		if (_fDamageRadius > 0f && _fDamage > 0f)
		{
			explode();
			Object.DestroyObject(base.gameObject, 3f);
		}
		else if (!bFromNet && crystal)
		{
		}
		Debug.Log("oil?:" + oilbox + "wall?" + wall);
		if (oilbox)
		{
			GameObject explodeObj = Object.Instantiate(Resources.Load("Particle/effect/Tank_Effect/Tank_Tex_Broken/Tank_Tex_Broken_02_pfb")) as GameObject;
			explodeObj.transform.parent = base.transform;
			explodeObj.transform.localPosition = new Vector3(0f, 0f, 0f);
			Object.Destroy(explodeObj, 3f);
			GameObject hugeExplode = Object.Instantiate(Resources.Load("Particle/effect/Tank_Effect/Tank_Attack/Tank_Brust/Tank_Brust_02_pfb")) as GameObject;
			hugeExplode.transform.parent = base.transform;
			hugeExplode.transform.localPosition = Vector3.zero;
			Object.Destroy(hugeExplode, 3f);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Can_Explode, base.transform);
		}
		else if (wall)
		{
			GameObject explodeObj2 = Object.Instantiate(Resources.Load("Particle/effect/Tank_Effect/Tank_Tex_Broken/Tank_Tex_Broken_03_pfb")) as GameObject;
			explodeObj2.transform.parent = base.transform;
			explodeObj2.transform.localPosition = new Vector3(0f, 0f, 0f);
			Object.Destroy(explodeObj2, 3f);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Wall_Destroy, base.transform);
		}
		else if (justbox)
		{
			GameObject explodeObj3 = Object.Instantiate(Resources.Load("Particle/effect/Tank_Effect/Tank_Tex_Broken/Tank_Tex_Broken_01_pfb")) as GameObject;
			explodeObj3.transform.parent = base.transform;
			explodeObj3.transform.localPosition = new Vector3(0f, 0f, 0f);
			Object.Destroy(explodeObj3, 3f);
		}
		else if (crystal)
		{
			GameObject ptlObj = Object.Instantiate(Resources.Load("Particle/effect/Crystal_Broken/Crystal_Broken")) as GameObject;
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Crystal_Broken, ptlObj.transform);
			ptlObj.transform.position = base.transform.position + Vector3.up * 1.1f;
			Object.DestroyObject(ptlObj, 3f);
			Object.DestroyObject(base.gameObject);
		}
	}

	public void doDropItem(bool bIsGold)
	{
		GameObject gameObject = null;
		gameObject = ((!bIsGold) ? (Object.Instantiate(Resources.Load("FBX/Common/Tank_Gold")) as GameObject) : (Object.Instantiate(Resources.Load("FBX/Scene/Tank/Prefab/Item")) as GameObject));
		if (gameObject != null)
		{
			gameObject.transform.parent = base.transform;
			gameObject.transform.position = base.transform.position + Vector3.up * 1f;
		}
	}

	public void tryDestory()
	{
		Object.Destroy(base.gameObject);
	}

	private void DropItem()
	{
		COMA_CD_CreateItem cOMA_CD_CreateItem = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.ITEMCREATE) as COMA_CD_CreateItem;
		cOMA_CD_CreateItem.blockIndex = (byte)base.ObstacleID;
		float num = 0.33f;
		float num2 = 0.33f;
		float num3 = Random.Range(0f, 1f);
		if (num3 < num)
		{
			cOMA_CD_CreateItem.itemIndex = 0;
			doDropItem(cOMA_CD_CreateItem.itemIndex == 0);
			COMA_CommandHandler.Instance.Send(cOMA_CD_CreateItem);
		}
		else if (num3 < num + num2)
		{
			cOMA_CD_CreateItem.itemIndex = 1;
			doDropItem(cOMA_CD_CreateItem.itemIndex == 0);
			COMA_CommandHandler.Instance.Send(cOMA_CD_CreateItem);
		}
	}

	private void explode()
	{
		Transform playerNodeTrs = COMA_Scene.Instance.playerNodeTrs;
		for (int i = 0; i < playerNodeTrs.childCount; i++)
		{
			Transform child = playerNodeTrs.GetChild(i);
			COMA_Creation component = child.GetComponent<COMA_Creation>();
			if (!(component != null))
			{
				continue;
			}
			Vector3 vector = child.position + Vector3.up * component.bodyHeight * 0.5f;
			Vector3 vector2 = child.position + Vector3.up * component.bodyHeight * 0.7f;
			if (Vector3.SqrMagnitude(vector - base.transform.position) < _fDamageRadius * _fDamageRadius)
			{
				if (component.creationKind == CreationKind.Enemy)
				{
					component.OnHurt(null, base.gameObject.name, _fDamage, Vector3.zero);
				}
				else if (component.creationKind == CreationKind.Player)
				{
					component.ReceiveHurt(-1, 0, _fDamage, Vector3.zero);
				}
			}
		}
	}
}
