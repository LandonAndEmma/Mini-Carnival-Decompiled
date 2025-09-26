using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class EffectAudioBehaviour : TAudioController
{
	public string m_AudioName = string.Empty;

	public float m_DeltaTime;

	public bool m_DeleteWhenEnd;

	public void Awake()
	{
	}

	public void Start()
	{
		if (!(m_AudioName == string.Empty))
		{
			PlayEffect();
		}
	}

	public void OnDestroy()
	{
		if (!(m_AudioName == string.Empty))
		{
			StopEffect();
		}
	}

	public void Update()
	{
	}

	private IEnumerator PlaySfxDelay()
	{
		yield return new WaitForSeconds(m_DeltaTime);
		PlayAudio(m_AudioName);
	}

	public void PlayEffect()
	{
		StartCoroutine(PlaySfxDelay());
	}

	public void StopEffect()
	{
		if (m_DeleteWhenEnd)
		{
			StopAudio(m_AudioName);
		}
	}
}
