using UnityEngine;

public class COMA_Fishing_SceneAudioController : MonoBehaviour
{
	private float tmr1;

	private float TMR1 = 6f;

	private float tmr2;

	private float TMR2 = 10f;

	private void Start()
	{
	}

	private void Update()
	{
		tmr1 += Time.deltaTime;
		if (tmr1 > TMR1)
		{
			tmr1 = 0f;
			TMR1 = 6f;
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.Amb_Tree_In_Wind, base.transform, 5f);
		}
		tmr2 += Time.deltaTime;
		if (tmr2 > TMR2)
		{
			tmr2 = 0f;
			TMR2 = Random.Range(10, 20);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.Amb_Seagull, base.transform, 5f);
		}
	}
}
