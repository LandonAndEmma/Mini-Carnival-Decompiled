using UnityEngine;

public class UIRPG_AvatarEnhanceChooseAvatarBtn : MonoBehaviour
{
	[SerializeField]
	private GameObject _popUpObj;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		if (_popUpObj != null)
		{
			_popUpObj.SetActive(true);
		}
	}
}
