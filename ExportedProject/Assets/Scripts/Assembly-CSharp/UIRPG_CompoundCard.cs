using MessageID;
using UnityEngine;

public class UIRPG_CompoundCard : MonoBehaviour
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
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPGCardCompound_BtnClick, null, null);
	}
}
