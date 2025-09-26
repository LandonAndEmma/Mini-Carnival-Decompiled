using UnityEngine;

public class COMA_Gold : MonoBehaviour
{
	public float animDelay;

	public Transform targetTrs;

	private float speed = 8f;

	private float accel = 16f;

	private void Start()
	{
		base.transform.GetChild(0).animation["Anim01"].time = animDelay;
	}

	private void Update()
	{
		if (targetTrs != null)
		{
			if (base.collider.enabled)
			{
				base.collider.enabled = false;
			}
			Vector3 vector = targetTrs.position - base.transform.position;
			if (vector.sqrMagnitude < speed * Time.deltaTime * speed * Time.deltaTime)
			{
				base.transform.position = targetTrs.position;
				COMA_PlayerSelf_Run component = targetTrs.GetComponent<COMA_PlayerSelf_Run>();
				component.OnTriggerEnter(base.collider);
				Object.DestroyObject(base.gameObject);
			}
			else
			{
				base.transform.position += vector.normalized * speed * Time.deltaTime;
				speed += accel * Time.deltaTime;
			}
		}
	}
}
