using UnityEngine;

public class UIRPG_AvatarEnhance_SuccessOkBtn : MonoBehaviour
{
	[SerializeField]
	private UIRPG_AvatarEnhanceMgr _avatarEnhanceMgr;

	public void OnClick()
	{
		_avatarEnhanceMgr.AvatarEnhanceSuccessPopupObj.SetActive(false);
	}
}
