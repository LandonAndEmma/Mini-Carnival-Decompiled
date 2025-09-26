using NGUI_COMUI;
using UnityEngine;

public class UIRPG_CardManageAddCouponBtn : MonoBehaviour
{
	[SerializeField]
	private UIRPGCardMgr _cardMgr;

	public void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		_cardMgr.PopUpObtainCoupon.SetActive(true);
	}
}
