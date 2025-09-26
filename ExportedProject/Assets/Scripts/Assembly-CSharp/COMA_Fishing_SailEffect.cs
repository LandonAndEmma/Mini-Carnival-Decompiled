using UnityEngine;

public class COMA_Fishing_SailEffect : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem[] _effects;

	private bool bTrueClose;

	private bool bPlay;

	private void OnEnable()
	{
		bTrueClose = false;
		bPlay = true;
		for (int i = 0; i < _effects.Length; i++)
		{
			_effects[i].loop = true;
		}
	}

	private void OnDisable()
	{
		bTrueClose = true;
	}

	public bool HanldeDisable()
	{
		bPlay = false;
		for (int i = 0; i < _effects.Length; i++)
		{
			_effects[i].loop = false;
		}
		return bTrueClose;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (!bPlay && !_effects[0].isPlaying)
		{
			base.gameObject.SetActive(false);
		}
	}
}
