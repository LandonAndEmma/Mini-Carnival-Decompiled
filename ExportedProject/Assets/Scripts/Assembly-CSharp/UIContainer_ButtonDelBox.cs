using MessageID;
using NGUI_COMUI;
using UnityEngine;

public class UIContainer_ButtonDelBox : MonoBehaviour
{
	[SerializeField]
	private NGUI_COMUI.UI_Box _ownerBox;

	[SerializeField]
	private UIMarket_CartBoxAniEvent _aniCmp;

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

	private void SendBoxOnDelete()
	{
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UIContainer_BoxOnDelete, null, OwnerBox);
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		if (_aniCmp != null)
		{
			_aniCmp.StartAni(SendBoxOnDelete);
		}
		else
		{
			SendBoxOnDelete();
		}
	}
}
