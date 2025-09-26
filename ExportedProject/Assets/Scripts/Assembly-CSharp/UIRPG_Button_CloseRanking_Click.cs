using MessageID;
using UnityEngine;

public class UIRPG_Button_CloseRanking_Click : MonoBehaviour
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
		if (Application.loadedLevelName == "UI.RPG.Map")
		{
			UIRPG_DataBufferCenter.isPreSceneMap = false;
			UIDataBufferCenter.Instance.CurBattleLevelLV = -1;
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPGRanking_GotoSquare, null, null);
	}
}
