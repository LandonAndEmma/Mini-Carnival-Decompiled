using UnityEngine;

public class AudioControlSelfBehaviour : TAudioController
{
	public void Awake()
	{
	}

	public void OnDestroy()
	{
	}

	public void Update()
	{
		ITAudioEvent component = base.gameObject.GetComponent<ITAudioEvent>();
		if (component != null && !component.isPlaying)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
