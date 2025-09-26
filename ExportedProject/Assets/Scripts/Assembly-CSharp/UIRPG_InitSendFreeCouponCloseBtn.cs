using UnityEngine;

public class UIRPG_InitSendFreeCouponCloseBtn : MonoBehaviour
{
	[SerializeField]
	private GameObject _parentObj;

	public void OnClick()
	{
		Object.Destroy(_parentObj);
	}
}
