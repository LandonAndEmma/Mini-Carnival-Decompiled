using MessageID;
using UnityEngine;

public class UIRankings_ButtonCaptionType : MonoBehaviour
{
	[SerializeField]
	public enum ECaptionType
	{
		World_Sel = 0,
		World_Unsel = 1,
		Friend_Sel = 2,
		Friend_Unsel = 3,
		SearchInput = 4,
		SearchFriend = 5,
		Unknow = 6
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
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRankings_CaptionTypeButtonSelChanged, null, _captionType);
	}
}
