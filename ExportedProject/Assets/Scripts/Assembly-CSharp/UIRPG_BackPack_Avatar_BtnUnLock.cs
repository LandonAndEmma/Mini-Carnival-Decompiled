using UnityEngine;

public class UIRPG_BackPack_Avatar_BtnUnLock : MonoBehaviour
{
	[SerializeField]
	private UILabel _unLockCrystal;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnEnable()
	{
		_unLockCrystal.text = RPGGlobalData.Instance.RpgMiscUnit._unitRPGAvatarBagPrice.ToString();
	}
}
