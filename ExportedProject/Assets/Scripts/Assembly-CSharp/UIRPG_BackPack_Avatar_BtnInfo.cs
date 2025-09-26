using MessageID;
using NGUI_COMUI;
using UnityEngine;

public class UIRPG_BackPack_Avatar_BtnInfo : MonoBehaviour
{
	[SerializeField]
	private NGUI_COMUI.UI_Box _ownerBox;

	public NGUI_COMUI.UI_Box OwnerBox
	{
		get
		{
			if (_ownerBox == null)
			{
				_ownerBox = base.transform.parent.gameObject.GetComponent<NGUI_COMUI.UI_Box>();
			}
			return _ownerBox;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnClick()
	{
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UIRPGAvatar_AvatarInfoBtnClick, null, _ownerBox);
	}
}
