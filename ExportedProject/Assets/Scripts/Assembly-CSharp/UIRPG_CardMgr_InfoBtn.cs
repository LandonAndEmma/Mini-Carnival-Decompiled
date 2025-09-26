using MessageID;
using UnityEngine;

public class UIRPG_CardMgr_InfoBtn : MonoBehaviour
{
	[SerializeField]
	private UIRPG_CardMgr_Card_Box _owerBox;

	public void OnClick()
	{
		UIRPG_CardMgr_Card_BoxData uIRPG_CardMgr_Card_BoxData = _owerBox.BoxData as UIRPG_CardMgr_Card_BoxData;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_PopupCardInfoWindow, null, uIRPG_CardMgr_Card_BoxData.CardId);
	}
}
