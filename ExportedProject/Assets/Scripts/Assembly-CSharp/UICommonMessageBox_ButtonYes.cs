using MessageID;
using NGUI_COMUI;
using UnityEngine;

public class UICommonMessageBox_ButtonYes : MonoBehaviour
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
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UICOMBox_YesClick, null, _box.BoxData);
	}
}
