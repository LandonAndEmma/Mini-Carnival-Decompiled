using System;
using UnityEngine;

public class COMA_Bullet : MonoBehaviour
{
	public COMA_PlayerSelf fromPlayerCom;

	public string hitPath = string.Empty;

	public string blastPath = string.Empty;

	[NonSerialized]
	public float distance = 20f;

	[NonSerialized]
	public float moveSpeed = 20f;

	[NonSerialized]
	public float gravity;

	protected float downSpeed;

	[NonSerialized]
	public float ap = 1f;

	[NonSerialized]
	public float apr;

	[NonSerialized]
	public float radius;

	[NonSerialized]
	public float push;

	private Vector3 bulletHeadPos = Vector3.zero;

	protected COMA_Creation creationCom;

	public int _nReflectLeft = 1;

	public bool _bIngoreObstacle;

	protected void PlayParticle(string path, Vector3 pos)
	{
		if (!(path == string.Empty))
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Particle/effect/" + path)) as GameObject;
			gameObject.transform.position = pos;
			UnityEngine.Object.DestroyObject(gameObject, 3f);
		}
	}

	protected void PlayParticle(string path, Vector3 pos, Vector3 up)
	{
		if (!(path == string.Empty))
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Particle/effect/" + path)) as GameObject;
			Debug.Log(gameObject.name);
			gameObject.transform.position = pos;
			gameObject.transform.up = up;
			UnityEngine.Object.DestroyObject(gameObject, 3f);
		}
	}

	private void Start()
	{
		bulletHeadPos = base.transform.position;
		if (base.gameObject.name == "B002")
		{
			Ray ray = new Ray(base.transform.position, base.transform.forward);
			LayerMask layerMask = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Obstacle"));
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, distance, layerMask))
			{
				distance = hitInfo.distance;
			}
			Ray ray2 = new Ray(base.transform.position, base.transform.forward);
			LayerMask layerMask2 = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Enemy"));
			RaycastHit[] array = Physics.RaycastAll(ray2, distance, layerMask2);
			RaycastHit[] array2 = array;
			foreach (RaycastHit hitInfo2 in array2)
			{
				OnHitTarget(hitInfo2);
			}
			UnityEngine.Object.DestroyObject(base.gameObject);
		}
		if (base.gameObject.name == "B003")
		{
			Ray ray3 = new Ray(base.transform.position, base.transform.forward);
			LayerMask layerMask3 = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Obstacle"));
			RaycastHit hitInfo3;
			if (Physics.Raycast(ray3, out hitInfo3, distance, layerMask3))
			{
				PlayParticle(blastPath, hitInfo3.point + hitInfo3.normal * 0.05f, hitInfo3.normal);
				distance = hitInfo3.distance;
			}
			Transform transform = base.transform.FindChild("Energy_Laser_pfb");
			transform.localScale = new Vector3(1f, 1f, distance);
			Ray ray4 = new Ray(base.transform.position, base.transform.forward);
			LayerMask layerMask4 = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Obstacle"));
			RaycastHit[] array3 = Physics.RaycastAll(ray4, distance, layerMask4);
			RaycastHit[] array4 = array3;
			for (int j = 0; j < array4.Length; j++)
			{
				RaycastHit hitInfo4 = array4[j];
				PlayParticle(hitPath, hitInfo4.point);
				OnHitTarget(hitInfo4);
				ap *= 0.7f;
			}
			UnityEngine.Object.DestroyObject(base.gameObject, 2f);
		}
	}

	private void CommonUpdate()
	{
		if (base.gameObject.name == "B002" || base.gameObject.name == "B003")
		{
			return;
		}
		downSpeed += gravity * Time.deltaTime;
		Vector3 vector = (base.transform.forward * moveSpeed + Vector3.down * downSpeed) * Time.deltaTime;
		float magnitude = vector.magnitude;
		Ray ray = new Ray(bulletHeadPos, vector);
		LayerMask layerMask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Transfer")) | (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Obstacle"));
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, Mathf.Min(magnitude, distance), layerMask))
		{
			bulletHeadPos = hitInfo.point;
			string text = LayerMask.LayerToName(hitInfo.collider.gameObject.layer);
			if (text == "Player" || text == "Enemy")
			{
				PlayParticle(hitPath, bulletHeadPos);
			}
			else
			{
				PlayParticle(blastPath, bulletHeadPos);
			}
			OnHitTarget(hitInfo);
			UnityEngine.Object.DestroyObject(base.gameObject);
		}
		else
		{
			if (!(distance > magnitude))
			{
				UnityEngine.Object.DestroyObject(base.gameObject);
				return;
			}
			base.transform.forward = vector;
			bulletHeadPos += vector;
			distance -= magnitude;
		}
		base.transform.position = bulletHeadPos;
	}

	private void TankUpdate()
	{
		downSpeed += gravity * Time.deltaTime;
		Vector3 vector = (base.transform.forward * moveSpeed + Vector3.down * downSpeed) * Time.deltaTime;
		float magnitude = vector.magnitude;
		Ray ray = new Ray(bulletHeadPos, vector);
		LayerMask layerMask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Transfer")) | (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Obstacle"));
		RaycastHit hitInfo;
		bool flag = Physics.Raycast(ray, out hitInfo, Mathf.Min(magnitude, distance), layerMask);
		string text = ((!(hitInfo.collider == null)) ? LayerMask.LayerToName(hitInfo.collider.gameObject.layer) : string.Empty);
		if (!_bIngoreObstacle && flag)
		{
			bulletHeadPos = hitInfo.point;
			string text2 = LayerMask.LayerToName(hitInfo.collider.gameObject.layer);
			if (_nReflectLeft > 0 && COMA_CommonOperation.Instance.IsMode_Tank(COMA_NetworkConnect.sceneName) && text2 == "Ground")
			{
				_nReflectLeft--;
				ReflectOnWall(hitInfo.normal);
			}
			else
			{
				if (text2 == "Player" || text2 == "Enemy")
				{
					PlayParticle(hitPath, bulletHeadPos);
				}
				else
				{
					PlayParticle(blastPath, bulletHeadPos);
				}
				OnHitTarget(hitInfo);
				playTankSoundBlast(hitInfo.transform);
				UnityEngine.Object.DestroyObject(base.gameObject, base.gameObject.name.Contains("B013") ? 1 : 0);
				moveSpeed = 0f;
			}
		}
		else if ((_bIngoreObstacle && flag && text == "Player") || text == "Enemy" || text == "Obstacle")
		{
			if (text == "Player" || text == "Enemy")
			{
				PlayParticle(hitPath, bulletHeadPos);
			}
			else
			{
				PlayParticle(blastPath, bulletHeadPos);
			}
			OnHitTarget(hitInfo);
			UnityEngine.Object.DestroyObject(base.gameObject, base.gameObject.name.Contains("B013") ? 1 : 0);
			moveSpeed = 0f;
			playTankSoundBlast(hitInfo.transform);
		}
		else
		{
			if (!(distance > magnitude))
			{
				UnityEngine.Object.DestroyObject(base.gameObject);
				return;
			}
			if (vector != Vector3.zero)
			{
				base.transform.forward = vector;
			}
			bulletHeadPos += vector;
			distance -= magnitude;
		}
		base.transform.position = bulletHeadPos;
	}

	private void Update()
	{
		if (COMA_CommonOperation.Instance.IsMode_Tank(COMA_NetworkConnect.sceneName))
		{
			TankUpdate();
		}
		else
		{
			CommonUpdate();
		}
	}

	private void playTankSoundBlast(Transform transformHit)
	{
		Debug.Log("tank bullet blash:" + base.name);
		if (base.name.Contains("B011"))
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Tank01_Blast, transformHit);
		}
		else if (base.name.Contains("B014"))
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Tank02_Blast, transformHit);
		}
		else if (base.name.Contains("B016"))
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Tank03_Blast, transformHit);
		}
		else if (base.name.Contains("B013"))
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_energy_gun_Smitten, transformHit);
		}
	}

	private void ReflectOnWall(Vector3 vHitNormal)
	{
		float num = Vector3.Angle(base.transform.forward, vHitNormal);
		base.transform.forward = Vector3.Reflect(base.transform.forward, vHitNormal);
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Tank_Miss, base.transform);
	}

	protected void OnHitTarget(RaycastHit hitInfo)
	{
		if (fromPlayerCom == null)
		{
			return;
		}
		if (hitInfo.collider.name.StartsWith("Block") && base.name == "B001")
		{
			COMA_DropFight_SceneController.Instance.SendBlockBroken(hitInfo.collider.gameObject);
		}
		creationCom = hitInfo.collider.GetComponent<COMA_Creation>();
		if (creationCom != null)
		{
			Vector3 vector = creationCom.transform.position + Vector3.up * creationCom.bodyHeight * creationCom.transform.localScale.x * 0.7f;
			Vector3 vector2 = (vector - bulletHeadPos).normalized * push;
			if (creationCom.creationKind == CreationKind.Enemy)
			{
				creationCom.OnHurt(fromPlayerCom, base.gameObject.name, ap, vector2);
			}
			else if (creationCom.creationKind == CreationKind.Player)
			{
				fromPlayerCom.OnHitOther(fromPlayerCom.id, creationCom.id, base.gameObject.name, ap, vector2);
				if (COMA_CommonOperation.Instance.IsMode_Blood(Application.loadedLevelName))
				{
					COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Impact_Player, hitInfo.collider.transform);
				}
			}
			if (COMA_CommonOperation.Instance.IsMode_Blood(Application.loadedLevelName))
			{
				COMA_PlayerSync_Blood cOMA_PlayerSync_Blood = creationCom as COMA_PlayerSync_Blood;
				if (cOMA_PlayerSync_Blood != null)
				{
					cOMA_PlayerSync_Blood.RedShine();
				}
			}
		}
		else if (COMA_CommonOperation.Instance.IsMode_Blood(Application.loadedLevelName))
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Gun_Rico, hitInfo.collider.transform);
		}
		BlastBullet(bulletHeadPos);
	}

	protected void BlastBullet(Vector3 blastPoint)
	{
		if (!(radius > 0f))
		{
			return;
		}
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
			if (Vector3.SqrMagnitude(vector - blastPoint) < radius * radius)
			{
				Vector3 vector3 = (vector2 - blastPoint).normalized * push;
				if (component.creationKind == CreationKind.Enemy)
				{
					component.OnHurt(fromPlayerCom, base.gameObject.name, apr, vector3);
				}
				else if (component.creationKind == CreationKind.Player)
				{
					fromPlayerCom.OnHitOther(fromPlayerCom.id, component.id, base.gameObject.name, apr, vector3);
				}
			}
		}
	}
}
