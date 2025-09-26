using MessageID;
using UnityEngine;

public class UIMarket_BoxRefresh_Charge : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Refresh);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_BoxRefresh_Charge, null, null);
	}
}
