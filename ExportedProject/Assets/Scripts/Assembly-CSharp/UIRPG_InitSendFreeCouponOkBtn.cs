using UnityEngine;

public class UIRPG_InitSendFreeCouponOkBtn : MonoBehaviour
{
	[SerializeField]
	private GameObject _parentObj;

	public void OnClick()
	{
		Object.Destroy(_parentObj);
	}
}
