using NGUI_COMUI;
using UnityEngine;

public class UIRPG_CardManage_ObtainCouponCloseBtn : MonoBehaviour
{
	[SerializeField]
	private UIRPGCardMgr _cardMgr;

	public void OnClick()
	{
		_cardMgr.PopUpObtainCoupon.SetActive(false);
	}
}
