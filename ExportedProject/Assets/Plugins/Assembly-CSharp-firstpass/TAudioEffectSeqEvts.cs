using UnityEngine;

[AddComponentMenu("AudioEffect/AudioEffect SeqEvts")]
public class TAudioEffectSeqEvts : ITAudioEvent
{
	public ITAudioEvent[] audioEvts;

	public float deltaTime = 0.2f;

	private ITAudioLimit[] m_audioLimits = new ITAudioLimit[0];

	private int m_lastPlayIndex = -1;

	private float m_triggerTime;

	private bool m_isTimeout = true;

	private bool m_awake;

	public override bool isPlaying
	{
		get
		{
			return false;
		}
	}

	public override bool isLoop
	{
		get
		{
			return false;
		}
	}

	private void Awake()
	{
		m_audioLimits = GetComponents<ITAudioLimit>();
		m_awake = true;
	}

	private void Update()
	{
		if (!m_isTimeout && Time.realtimeSinceStartup - m_triggerTime > deltaTime)
		{
			m_isTimeout = true;
		}
	}

	private void OnDestroy()
	{
		if (TAudioManager.checkInstance)
		{
			Stop();
		}
	}

	private void SendTriggerEvent(AudioClip clip)
	{
		ITAudioLimit[] audioLimits = m_audioLimits;
		foreach (ITAudioLimit iTAudioLimit in audioLimits)
		{
			iTAudioLimit.OnAudioTrigger(clip);
		}
	}

	public override void Trigger()
	{
		if (!m_awake)
		{
			Debug.LogWarning("TAudioEffectSequence is not Awake");
		}
		if (audioEvts.Length == 0)
		{
			return;
		}
		ITAudioLimit[] audioLimits = m_audioLimits;
		foreach (ITAudioLimit iTAudioLimit in audioLimits)
		{
			if (!iTAudioLimit.isCanPlay)
			{
				return;
			}
		}
		if (deltaTime < 0f)
		{
			m_lastPlayIndex = Mathf.Min(m_lastPlayIndex + 1, audioEvts.Length - 1);
		}
		else
		{
			m_lastPlayIndex = ((!m_isTimeout) ? Mathf.Min(m_lastPlayIndex + 1, audioEvts.Length - 1) : 0);
			m_isTimeout = false;
			m_triggerTime = Time.realtimeSinceStartup;
		}
		ITAudioEvent iTAudioEvent = audioEvts[m_lastPlayIndex];
		if (null != iTAudioEvent)
		{
			iTAudioEvent.Trigger();
		}
	}

	public override void Stop()
	{
	}
}
