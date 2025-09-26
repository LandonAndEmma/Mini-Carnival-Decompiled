using MessageID;
using UnityEngine;

public class UIMarket_ButtonGotoFitting : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UIMarket_GotoFittingRoom, null, null);
	}
}
