using MessageID;
using UnityEngine;

public class UIRPG_Button_WorldRanking_Click : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRankings_CaptionTypeSwitched, null, UIRankings_ButtonCaptionType.ECaptionType.World_Unsel);
	}
}
