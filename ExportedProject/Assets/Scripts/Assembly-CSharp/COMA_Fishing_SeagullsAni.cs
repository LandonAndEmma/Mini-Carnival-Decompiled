using UnityEngine;

public class COMA_Fishing_SeagullsAni : MonoBehaviour
{
	private float fstarttime;

	[SerializeField]
	private float _fDelayTime;

	private bool bPlay;

	private void Awake()
	{
		fstarttime = Time.time;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (!bPlay && Time.time - fstarttime >= _fDelayTime)
		{
			base.animation.Play();
			bPlay = true;
		}
	}
}
