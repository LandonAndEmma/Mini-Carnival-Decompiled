using MessageID;
using UnityEngine;

public class UIRPG_FreeCard_InfoBtn : MonoBehaviour
{
	[SerializeField]
	private UIRPG_BigCard _owerBox;

	public void OnClick()
	{
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_PopupCardInfoWindow, null, _owerBox.GetCardId());
	}
}
