using UnityEngine;

public class UIRPG_AvatarEnhance_ChooseGemBackBtn : MonoBehaviour
{
	[SerializeField]
	private GameObject _hideObj;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		if (_hideObj != null)
		{
			_hideObj.SetActive(false);
		}
	}
}
