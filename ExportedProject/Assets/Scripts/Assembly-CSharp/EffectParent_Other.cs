using UnityEngine;

public class EffectParent_Other : MonoBehaviour
{
	private bool result;

	public void Awake()
	{
	}

	public void OnDestroy()
	{
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Period) && !result)
		{
			result = true;
			ParticleBehaviour componentInChildren = base.gameObject.GetComponentInChildren<ParticleBehaviour>();
			if (componentInChildren != null)
			{
				componentInChildren.PlayEffect();
			}
		}
		if (Input.GetKeyUp(KeyCode.Period))
		{
			result = false;
		}
	}
}
