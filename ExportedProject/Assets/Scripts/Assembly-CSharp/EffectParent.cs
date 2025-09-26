using UnityEngine;

public class EffectParent : MonoBehaviour
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
		if (Input.GetKeyDown(KeyCode.Comma) && !result)
		{
			result = true;
			ParticleBehaviour componentInChildren = base.gameObject.GetComponentInChildren<ParticleBehaviour>();
			if (componentInChildren != null)
			{
				componentInChildren.PlayEffect();
			}
		}
		if (Input.GetKeyUp(KeyCode.Comma))
		{
			result = false;
		}
	}
}
