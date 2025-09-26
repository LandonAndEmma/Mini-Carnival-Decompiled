using UnityEngine;

public class COMA_Fishing_SceneParticleController : MonoBehaviour
{
	private float tmr1;

	private float TMR1 = 30f;

	private float tmr2 = 15f;

	private float TMR2 = 30f;

	private void Start()
	{
	}

	private void Update()
	{
		tmr1 += Time.deltaTime;
		if (tmr1 > TMR1)
		{
			tmr1 = 0f;
			GameObject obj = Object.Instantiate(Resources.Load("Particle/effect/Whale/Whale_00")) as GameObject;
			Object.DestroyObject(obj, 10f);
			SceneTimerInstance.Instance.Add(1.7f, AudioWhaleAppear);
			SceneTimerInstance.Instance.Add(6.3f, AudioWhaleDisappear);
		}
		tmr2 += Time.deltaTime;
		if (tmr2 > TMR2)
		{
			tmr2 = 0f;
			GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Whale/Whale_00")) as GameObject;
			gameObject.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
			Object.DestroyObject(gameObject, 10f);
			SceneTimerInstance.Instance.Add(1.5f, AudioWhaleAppear);
			SceneTimerInstance.Instance.Add(6.3f, AudioWhaleDisappear);
		}
	}

	public bool AudioWhaleAppear()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.Amb_Whale_Appear);
		return false;
	}

	public bool AudioWhaleDisappear()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.Amb_Whale_Disappear);
		return false;
	}
}
