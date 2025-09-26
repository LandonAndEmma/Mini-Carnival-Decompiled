using UnityEngine;

public class UIRPG_AvatarEnhanceChooseGemBtn : MonoBehaviour
{
	[SerializeField]
	private GameObject _popUpObj;

	[SerializeField]
	private UISprite _addGemIcon;

	[SerializeField]
	private UISprite _disGemIcon;

	[SerializeField]
	private UISprite _changeGemIcon;

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

	public void DisplayGemIcon(int type, int level)
	{
		_addGemIcon.gameObject.SetActive(false);
		_changeGemIcon.gameObject.SetActive(true);
		_disGemIcon.gameObject.SetActive(true);
		_disGemIcon.spriteName = UIRPG_DataBufferCenter.GetSmallGemSpriteNameByTypeAndLevel(type, level);
		_disGemIcon.MakePixelPerfect();
		_disGemIcon.transform.localScale = _disGemIcon.transform.localScale * 2f;
	}

	public void RecoverGemIcon()
	{
		_disGemIcon.gameObject.SetActive(false);
		_addGemIcon.gameObject.SetActive(true);
		_changeGemIcon.gameObject.SetActive(false);
	}
}
