using UnityEngine;

[ExecuteInEditMode]
public class ParticleBehaviour : MonoBehaviour
{
	private bool play;

	public float m_Time;

	public float m_maxTime;

	private bool m_Pause;

	private ParticleSystem[] particle_systems;

	private Animation[] particle_animations;

	public void Awake()
	{
	}

	public void OnDestroy()
	{
	}

	public void Update()
	{
		if (play)
		{
			m_Time += Time.deltaTime;
			if (m_maxTime > 0f && m_Time >= m_maxTime)
			{
				StopEffect();
				m_Time = 0f;
				play = false;
			}
		}
	}

	public void SetPauseData(bool pause)
	{
		m_Pause = pause;
	}

	public void SetPause()
	{
		if (!m_Pause)
		{
			return;
		}
		Animation[] array = particle_animations;
		foreach (Animation animation in array)
		{
			foreach (AnimationState item in animation)
			{
				item.speed = 0f;
			}
		}
	}

	public void SetUnPause()
	{
		Animation[] array = particle_animations;
		foreach (Animation animation in array)
		{
			foreach (AnimationState item in animation)
			{
				item.speed = 1f;
			}
		}
	}

	public void PlayEffect()
	{
		play = true;
		m_Time = 0f;
		particle_systems = GetComponentsInChildren<ParticleSystem>();
		particle_animations = GetComponentsInChildren<Animation>();
		if (m_maxTime == 0f)
		{
			if (particle_animations.Length > 0)
			{
				float num = 0f;
				Animation[] array = particle_animations;
				foreach (Animation animation in array)
				{
					Debug.Log(animation.name);
					num = ((!(animation.clip.length > num)) ? num : animation.clip.length);
				}
				m_maxTime = num;
			}
			else if (particle_systems.Length > 0)
			{
				float num2 = 0f;
				ParticleSystem[] array2 = particle_systems;
				foreach (ParticleSystem particleSystem in array2)
				{
					num2 = ((!(particleSystem.duration > num2)) ? num2 : particleSystem.duration);
				}
				m_maxTime = num2;
			}
		}
		Animation[] array3 = particle_animations;
		foreach (Animation animation2 in array3)
		{
			animation2.Play();
		}
		ParticleSystem[] array4 = particle_systems;
		foreach (ParticleSystem particleSystem2 in array4)
		{
			particleSystem2.Play();
		}
		EffectAudioBehaviour component = base.gameObject.GetComponent<EffectAudioBehaviour>();
		if (component != null)
		{
			component.PlayEffect();
		}
	}

	public void StopEffect()
	{
		Animation[] array = particle_animations;
		foreach (Animation animation in array)
		{
			animation.Stop();
		}
		ParticleSystem[] array2 = particle_systems;
		foreach (ParticleSystem particleSystem in array2)
		{
			particleSystem.Stop();
		}
		EffectAudioBehaviour component = base.gameObject.GetComponent<EffectAudioBehaviour>();
		if (component != null)
		{
			component.StopEffect();
		}
	}
}
