using UnityEngine;

public class UIRPG_AvatarEnhance_ChooseGemCaptionBtn : MonoBehaviour
{
	[SerializeField]
	private GameObject[] _btnState;

	[SerializeField]
	private UIRPG_AvatarEnhance_ChooseGemMgr _chooseGemMgr;

	[SerializeField]
	private UIRPG_AvatarEnhance_ChooseGemMgr.ECaptionType _captionType;

	public UIRPG_AvatarEnhance_ChooseGemMgr.ECaptionType CaptionType
	{
		get
		{
			return _captionType;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetActiveBtn(bool bAction)
	{
		_btnState[0].SetActive(bAction);
		_btnState[1].SetActive(!bAction);
	}

	public void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		_chooseGemMgr.HandleClickBtn(_captionType);
	}
}
