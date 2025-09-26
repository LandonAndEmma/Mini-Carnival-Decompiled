using System;
using UnityEngine;

public class COMA_Dodge_Rocket : MonoBehaviour
{
	public ParticleSystem hitPtl;

	[NonSerialized]
	public float distance = 20f;

	[NonSerialized]
	public float moveSpeed = 4f;

	private COMA_Creation creationCom;

	private void Start()
	{
	}

	private void Update()
	{
		float num = moveSpeed * Time.deltaTime;
		base.transform.position += base.transform.forward * num;
		distance -= num;
		Ray ray = new Ray(base.transform.position - base.transform.forward * 1.2f, base.transform.forward);
		LayerMask layerMask = 1 << LayerMask.NameToLayer("Player");
		RaycastHit hitInfo;
		if (Physics.SphereCast(ray, 0.5f, out hitInfo, 1.2f, layerMask))
		{
			HitParticle(base.transform.position);
			OnHitTarget(hitInfo);
			UnityEngine.Object.DestroyObject(base.gameObject);
		}
		else if (distance < 0f)
		{
			UnityEngine.Object.DestroyObject(base.gameObject);
		}
	}

	private void OnHitTarget(RaycastHit hitInfo)
	{
		creationCom = hitInfo.collider.GetComponent<COMA_Creation>();
		if (creationCom != null)
		{
			Vector3 vector = creationCom.transform.position + Vector3.up * creationCom.bodyHeight * 0.7f;
			Vector3 push = (vector - base.transform.position).normalized * 20f;
			if (creationCom.creationKind == CreationKind.Player)
			{
				creationCom.ReceiveHurt(0, 0, 0f, push);
			}
		}
	}

	private void HitParticle(Vector3 hitPoint)
	{
		if (hitPtl != null)
		{
			Quaternion localRotation = hitPtl.transform.localRotation;
			hitPtl.transform.parent = null;
			hitPtl.transform.position = hitPoint;
			hitPtl.transform.rotation = localRotation;
			UnityEngine.Object.DestroyObject(hitPtl.gameObject, 3f);
			hitPtl.Play(true);
			Animation[] componentsInChildren = hitPtl.GetComponentsInChildren<Animation>();
			Animation[] array = componentsInChildren;
			foreach (Animation animation in array)
			{
				animation.Play();
			}
		}
	}
}
