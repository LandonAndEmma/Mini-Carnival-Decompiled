using MessageID;
using UnityEngine;

public class UIGameMode_OpenGameContent : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Gamezone);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenGameContent, null, null);
	}
}
