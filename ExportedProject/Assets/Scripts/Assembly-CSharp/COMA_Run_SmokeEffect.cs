using UnityEngine;

public class COMA_Run_SmokeEffect : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem[] _ps;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetSmokeActive(bool bActive)
	{
		if (bActive && !base.gameObject.activeSelf && _ps != null)
		{
			ParticleSystem[] ps = _ps;
			foreach (ParticleSystem particleSystem in ps)
			{
				particleSystem.Play();
			}
		}
		base.gameObject.SetActive(bActive);
	}
}
