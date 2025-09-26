using System.Collections.Generic;
using UnityEngine;

public class COMA_Enemy : COMA_Creation
{
	protected bool bSync;

	public COMA_PlayerSelf_Castle playerCastleCom;

	public EnemyCategory category;

	public float view;

	public Transform targetToAttack;

	public Transform curMoveTrs;

	protected bool bFlying = true;

	protected Dictionary<byte, string> animIndexToName = new Dictionary<byte, string>();

	protected Dictionary<string, byte> animNameToIndex = new Dictionary<string, byte>();

	private void OnDrawGizmos()
	{
		if (curMoveTrs != null)
		{
			Vector3 vector = base.transform.position + Vector3.up * cCtl.height;
			Vector3 position = curMoveTrs.position;
			position.y = vector.y;
			Gizmos.DrawLine(vector, position);
		}
	}

	protected new void Start()
	{
		base.Start();
		creationKind = CreationKind.Enemy;
		base.animation["Move"].layer = 1;
		if (base.animation["Attack"] != null)
		{
			base.animation["Attack"].layer = 2;
		}
		if (base.animation["Hit"] != null)
		{
			base.animation["Hit"].layer = 2;
		}
		base.animation["Death"].layer = 3;
		byte b = 0;
		foreach (AnimationState item in base.animation)
		{
			animIndexToName.Add(b, item.name);
			animNameToIndex.Add(item.name, b);
			b++;
		}
	}

	public void SetSync(bool isSync)
	{
	}

	protected void PlayMyAnimation(string animName)
	{
		PlayMyAnimation(animName, 0.3f, 1f);
	}

	protected void PlayMyAnimation(string animName, float fadeTime)
	{
		PlayMyAnimation(animName, fadeTime, 1f);
	}

	protected void PlayMyAnimation(string animName, float fadeTime, float playSpeed)
	{
		if (playSpeed == 0f)
		{
			base.animation.Stop(animName);
			return;
		}
		base.animation[animName].speed = playSpeed;
		if (fadeTime == 0f)
		{
			base.animation.Play(animName);
		}
		else
		{
			base.animation.CrossFade(animName, fadeTime);
		}
	}

	public void FreezeAnimation()
	{
		base.animation.Sample();
		base.animation.Stop();
	}

	public override void OnHurt(COMA_PlayerSelf from, string bulletName, float bulletAP, Vector3 push)
	{
		// Use int.Parse so the conditional expression returns a consistent numeric type (int)
		int bulletID = !string.IsNullOrEmpty(bulletName) ? int.Parse(bulletName.Substring(1)) : 0;
		OnHurt(bulletID, bulletAP, push);
	}

	public void OnHurt(int bulletID, float bulletAP, Vector3 push)
	{
		if (base.IsDead || base.IsFrozen)
		{
			return;
		}
		switch (bulletID)
		{
		case 4:
			if (!base.IsVenom)
			{
				base.IsVenom = true;
				SceneTimerInstance.Instance.Add(COMA_Buff.Instance.lastTime_venom, base.VenomRecover);
			}
			break;
		case 6:
		{
			FreezeAnimation();
			base.IsFrozen = true;
			icefromID = 0;
			iceAP = bulletAP;
			icePush = push.magnitude;
			GameObject gameObject = Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_Ice")) as GameObject;
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = Vector3.zero;
			Object.DestroyObject(gameObject, COMA_Buff.Instance.lastTime_ice);
			SceneTimerInstance.Instance.Add(COMA_Buff.Instance.lastTime_ice, FrozenBroken);
			return;
		}
		case 7:
			base.hp = 0f;
			DropItem();
			Object.DestroyObject(base.gameObject);
			return;
		}
		base.hp -= bulletAP;
		if (base.IsDead)
		{
			if (category != EnemyCategory.Enemy04)
			{
				PlayMyAnimation("Death", 0.1f);
				Object.DestroyObject(base.gameObject, 2f);
			}
			else
			{
				PlayMyAnimation("Death", 0.1f, 0.2f);
			}
			DropItem();
		}
		else if (category != EnemyCategory.Enemy03)
		{
			PlayMyAnimation("Hit", 0f);
		}
		else
		{
			PlayMyAnimation("Move", 0f);
		}
		movePsv = push;
		bFlying = true;
	}

	private void DropItem()
	{
		playerCastleCom.score += ((!playerCastleCom.buff_doubleScore) ? base.score : (base.score * 2));
		float[] array = new float[8] { 0.005f, 0.002f, 0.004f, 0.001f, 0.004f, 0.001f, 0.004f, 0.001f };
		Random.seed = (int)Time.time % 100 * int.Parse(base.gameObject.name) * 123456;
		float num = Random.Range(0f, 1f);
		int i;
		for (i = 0; i < array.Length && num >= array[i]; i++)
		{
			num -= array[i];
		}
		string empty = string.Empty;
		switch (i)
		{
		default:
			return;
		case 0:
			empty = "FBX/Item/PFB/PFB_Item_FullFill";
			break;
		case 1:
			empty = "FBX/Item/PFB/PFB_Item_W001";
			break;
		case 2:
			empty = "FBX/Item/PFB/PFB_Item_W002";
			break;
		case 3:
			empty = "FBX/Item/PFB/PFB_Item_W003";
			break;
		case 4:
			empty = "FBX/Item/PFB/PFB_Item_W004";
			break;
		case 5:
			empty = "FBX/Item/PFB/PFB_Item_W006";
			break;
		case 6:
			empty = "FBX/Item/PFB/PFB_Item_W007";
			break;
		case 7:
			empty = "FBX/Item/PFB/PFB_Item_W008";
			break;
		}
		GameObject gameObject = Object.Instantiate(Resources.Load(empty)) as GameObject;
		gameObject.name = gameObject.name.Replace("(Clone)", string.Empty);
		gameObject.transform.position = base.transform.position + Vector3.up * Random.Range(0.1f, 0.2f);
	}
}
