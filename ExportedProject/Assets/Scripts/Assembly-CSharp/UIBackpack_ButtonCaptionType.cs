using MessageID;
using UnityEngine;

public class UIBackpack_ButtonCaptionType : MonoBehaviour
{
	[SerializeField]
	public enum ECaptionType
	{
		Backpack_Sel = 0,
		Backpack_Unsel = 1,
		Sell_Sel = 2,
		Sell_Unsel = 3,
		Unknow = 4
	}

	[SerializeField]
	private ECaptionType _captionType = ECaptionType.Unknow;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIBackpack_CaptionTypeButtonSelChanged, null, _captionType);
	}
}
