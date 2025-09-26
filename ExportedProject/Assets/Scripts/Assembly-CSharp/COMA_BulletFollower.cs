using UnityEngine;

public class COMA_BulletFollower : COMA_Bullet
{
	private Vector3 tarDir1 = Vector3.zero;

	private float state = 1f;

	private void Start()
	{
	}

	public void SetTarget(Transform trs)
	{
		tarDir1 = new Vector3(0f, 4f, 2f);
		if (trs != null)
		{
			creationCom = trs.GetComponent<COMA_Creation>();
		}
	}

	private void Update()
	{
		if (state > 1.5f && creationCom != null)
		{
			Vector3 vector = creationCom.transform.position + Vector3.up * 0.5f;
			Vector3 vector2 = vector - base.transform.position;
			if (vector2.sqrMagnitude < 0.25f || Vector3.Dot(vector2, base.transform.forward) < 0f)
			{
				PlayParticle(hitPath, base.transform.position);
				if (fromPlayerCom != null)
				{
					fromPlayerCom.OnHitOther(fromPlayerCom.id, creationCom.id, base.gameObject.name, 0f, Vector3.zero);
				}
				Object.DestroyObject(base.gameObject);
			}
			else
			{
				base.transform.forward = Vector3.Lerp(base.transform.forward, vector2, 0.01f);
				base.transform.position += base.transform.forward * 24f * Time.deltaTime;
			}
		}
		else
		{
			state += Time.deltaTime;
			base.transform.forward = Vector3.Lerp(base.transform.forward, tarDir1, 0.02f);
			base.transform.position += base.transform.forward * 6f * Time.deltaTime;
			if (state > 5f)
			{
				Object.DestroyObject(base.gameObject);
			}
		}
	}
}
