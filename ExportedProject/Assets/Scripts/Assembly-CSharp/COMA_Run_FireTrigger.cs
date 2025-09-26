using UnityEngine;

public class COMA_Run_FireTrigger : MonoBehaviour
{
	[SerializeField]
	private int nType;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
		COMA_PlayerSelf_Run component = other.gameObject.GetComponent<COMA_PlayerSelf_Run>();
		if (component != null && component == COMA_PlayerSelf.Instance && !component.IsInvincible)
		{
			component.ColliderFire(nType);
		}
	}
}
