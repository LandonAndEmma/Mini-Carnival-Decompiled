using MessageID;
using NGUI_COMUI;
using UnityEngine;

public class UIJoinGameMessageBox_ButtonNo : MonoBehaviour
{
	[SerializeField]
	private NGUI_COMUI.UI_Box _box;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		Object.Destroy(_box.gameObject);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIJoinGameBox_NoClick, null, _box.BoxData);
	}
}
