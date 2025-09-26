using MessageID;
using UnityEngine;

public class UI_NGRankingButton : MonoBehaviour
{
	[SerializeField]
	private int _btnIndex;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_RankingBoardBtnDown, null, _btnIndex);
	}
}
