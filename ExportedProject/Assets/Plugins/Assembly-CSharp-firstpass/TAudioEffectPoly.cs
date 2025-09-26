using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("AudioEffect/AudioEffect Poly")]
public class TAudioEffectPoly : ITAudioEvent
{
	public GameObject audioEffectRef;

	public float intervalRangeMin;

	public float intervalRangeMax;

	public int polyphony = 1;

	public int playCount;

	private ITAudioEvent m_audioEvt;

	private ITAudioLimit[] m_audioLimits = new ITAudioLimit[0];

	private bool m_isPlay;

	private bool m_awake;

	private int m_playCount;

	private float m_delaylast;

	private float m_playTime;

	private List<float> m_delays = new List<float>();

	public override bool isPlaying
	{
		get
		{
			return m_isPlay;
		}
	}

	public override bool isLoop
	{
		get
		{
			return playCount == 0;
		}
	}

	private void Awake()
	{
		m_audioLimits = GetComponents<ITAudioLimit>();
		GameObject gameObject = Object.Instantiate(audioEffectRef) as GameObject;
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.zero;
		m_audioEvt = gameObject.GetComponent<ITAudioEvent>();
		m_awake = true;
	}

	private void Update()
	{
		if (!m_isPlay)
		{
			return;
		}
		float num = Time.realtimeSinceStartup - m_playTime;
		int num2 = 0;
		foreach (float delay in m_delays)
		{
			float num3 = delay;
			if (num < num3)
			{
				break;
			}
			m_audioEvt.Trigger();
			Debug.Log("Play" + Time.realtimeSinceStartup);
			num2++;
		}
		if (num2 <= 0)
		{
			return;
		}
		m_delays.RemoveRange(0, num2);
		if (!m_audioEvt.isLoop)
		{
			for (int i = 0; i < num2; i++)
			{
				PlayOnce();
			}
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

	private void PlayOnce()
	{
		if (playCount <= 0 || m_playCount < playCount)
		{
			float num = Random.Range(intervalRangeMin, intervalRangeMax);
			num *= 0.001f;
			num += m_delaylast;
			m_delays.Add(num);
			m_playCount++;
			m_delaylast = num;
		}
	}

	public override void Trigger()
	{
		if (!m_awake)
		{
			Debug.LogWarning("TAudioEffectPoly is not Awake");
		}
		if (!m_audioEvt)
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
		m_playCount = 0;
		m_isPlay = true;
		m_delaylast = 0f;
		m_playTime = Time.realtimeSinceStartup;
		for (int j = 0; j < polyphony; j++)
		{
			PlayOnce();
		}
		SendTriggerEvent(null);
	}

	public override void Stop()
	{
		m_isPlay = false;
		m_audioEvt.Stop();
		m_delays.Clear();
	}
}
