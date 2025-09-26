using UnityEngine;

public class UIRPG_AvatarEnhance_ChooseAvatarCloseBtn : MonoBehaviour
{
	[SerializeField]
	private GameObject _hideObj;

	[SerializeField]
	private GameObject _selObj;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		_selObj.SetActive(false);
		_hideObj.SetActive(false);
	}
}
