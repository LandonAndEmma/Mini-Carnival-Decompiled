using MessageID;
using UnityEngine;

public class UIRPG_CardManage_ObtainCouponBuyBtn : MonoBehaviour
{
	public enum ObtainCouponNum
	{
		Five = 0,
		Fifty = 1
	}

	[SerializeField]
	private ObtainCouponNum _couponNum;

	public void OnClick()
	{
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_ObtainCouponBtnClick, null, _couponNum);
	}
}
