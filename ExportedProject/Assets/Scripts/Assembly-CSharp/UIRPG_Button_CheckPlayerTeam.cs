using MessageID;
using NGUI_COMUI;
using UnityEngine;

public class UIRPG_Button_CheckPlayerTeam : MonoBehaviour
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
		Debug.Log(_box.BoxData.ItemId);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_DragOtherPlayerTeamInfo, null, _box.BoxData.ItemId);
	}
}
