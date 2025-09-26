using MessageID;
using UnityEngine;

public class UIRPG_MyTeam_SelCardInfoBtn : MonoBehaviour
{
	[SerializeField]
	private UIRPG_MyTeam_SelCardBox _box;

	public void OnClick()
	{
		UIRPG_MyTeam_SelCardBoxData uIRPG_MyTeam_SelCardBoxData = _box.BoxData as UIRPG_MyTeam_SelCardBoxData;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_PopupCardInfoWindow, null, (int)uIRPG_MyTeam_SelCardBoxData.CardId);
	}
}
