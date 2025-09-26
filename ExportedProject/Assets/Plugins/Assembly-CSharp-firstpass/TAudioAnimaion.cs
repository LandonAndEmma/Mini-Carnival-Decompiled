using System.Collections.Generic;
using UnityEngine;

public class TAudioAnimaion : MonoBehaviour
{
	public TAudioAnimEvt audioAnimEvt;

	private static TAudioAnimaion s_instance;

	private Dictionary<string, AnimationClip> m_audio_anim = new Dictionary<string, AnimationClip>();

	public static TAudioAnimaion instance
	{
		get
		{
			return s_instance;
		}
	}

	private void Awake()
	{
		s_instance = this;
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	private void Start()
	{
		if (!audioAnimEvt)
		{
			return;
		}
		m_audio_anim.Clear();
		TAudioAnimEvt.Anim[] anims = audioAnimEvt.anims;
		foreach (TAudioAnimEvt.Anim anim in anims)
		{
			AnimationClip animationClip = new AnimationClip();
			animationClip.name = anim.name;
			animationClip.wrapMode = anim.wrapMode;
			TAudioAnimEvt.AnimEvt[] evts = anim.evts;
			foreach (TAudioAnimEvt.AnimEvt animEvt in evts)
			{
				AnimationEvent animationEvent = new AnimationEvent();
				animationEvent.functionName = "PlayAudio";
				animationEvent.stringParameter = animEvt.prefab;
				animationEvent.time = animEvt.time;
				animationClip.AddEvent(animationEvent);
			}
			m_audio_anim.Add(anim.name, animationClip);
		}
	}

	public Animation GetAudioAnim(Transform anim_trans, string audio_anim_name)
	{
		AnimationClip value;
		if (!m_audio_anim.TryGetValue(audio_anim_name, out value))
		{
			Debug.LogError(audio_anim_name + " is null!");
			return null;
		}
		Transform transform = anim_trans.FindChild("AudioAnim");
		if (!transform)
		{
			GameObject gameObject = new GameObject("AudioAnim");
			gameObject.AddComponent<Animation>();
			gameObject.transform.parent = anim_trans;
			gameObject.transform.localPosition = Vector3.zero;
			transform = gameObject.transform;
			gameObject.AddComponent<TAudioController>();
		}
		Animation component = transform.GetComponent<Animation>();
		component.playAutomatically = false;
		component.AddClip(value, audio_anim_name);
		component.clip = value;
		return component;
	}
}
