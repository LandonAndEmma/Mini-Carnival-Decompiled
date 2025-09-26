using UnityEngine;

public class COMA_Fishing_FishBoat : MonoBehaviour
{
	[SerializeField]
	private Animation _aniCmp;

	[SerializeField]
	public Transform _transPlayerNode;

	[SerializeField]
	public Transform _transPlayerFishingNode;

	[SerializeField]
	private GameObject _objSailEffect;

	[SerializeField]
	private GameObject _objIdleEffect;

	public void SetSail(bool b)
	{
		if (b)
		{
			if (!_objSailEffect.activeSelf)
			{
				_objSailEffect.SetActive(true);
			}
			_objIdleEffect.SetActive(false);
		}
		else
		{
			COMA_Fishing_SailEffect component = _objSailEffect.GetComponent<COMA_Fishing_SailEffect>();
			if (component.HanldeDisable() && !_objIdleEffect.activeSelf)
			{
				_objIdleEffect.SetActive(true);
			}
		}
	}

	public void PlayBoatAni(string ani)
	{
		if (!_aniCmp.IsPlaying(ani))
		{
			_aniCmp.CrossFade(ani, 0.2f);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
