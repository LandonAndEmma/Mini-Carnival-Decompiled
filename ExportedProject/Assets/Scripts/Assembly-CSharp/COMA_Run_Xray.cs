using UnityEngine;

public class COMA_Run_Xray : MonoBehaviour
{
	[SerializeField]
	private Transform _fireJet;

	[SerializeField]
	private float _fireIntervalTime = 2f;

	private bool bStarted;

	private void Start()
	{
		if (!bStarted)
		{
			Fire();
			SceneTimerInstance.Instance.Add(_fireIntervalTime + 2f, Fire);
			bStarted = true;
		}
	}

	private void Update()
	{
	}

	public bool Fire()
	{
		if (_fireJet.gameObject.active)
		{
			GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Fire/Fire_burst")) as GameObject;
			gameObject.transform.parent = _fireJet;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Fire, base.transform);
			Object.DestroyObject(gameObject, 1.8f);
		}
		return true;
	}
}
