using System.Collections.Generic;
using MessageID;
using UnityEngine;

public class UISquare_GotoCardMgr : UIEntity
{
	[SerializeField]
	private UISprite _sprite_light;

	[SerializeField]
	private List<UISprite> _sprite_iconLst = new List<UISprite>();

	private List<int> _sprite_iconDepthLst = new List<int>();

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_SquareBoardBtnDown, this, SquareBoardBtnDown);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_SquareBoardBtnDown, this);
	}

	private bool SquareBoardBtnDown(TUITelegram msg)
	{
		int num = (int)msg._pExtraInfo;
		int num2 = num;
		if (num2 == 2)
		{
			Vector3 localPosition = base.transform.localPosition;
			localPosition.z = -150f;
			base.transform.localPosition = localPosition;
			_sprite_iconDepthLst.Clear();
			_sprite_light.enabled = true;
			for (int i = 0; i < _sprite_iconLst.Count; i++)
			{
				_sprite_iconDepthLst.Add(_sprite_iconLst[i].depth);
				_sprite_iconLst[i].depth += 600;
			}
		}
		return true;
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		Vector3 localPosition = base.transform.localPosition;
		localPosition.z = 0f;
		base.transform.localPosition = localPosition;
		if (_sprite_iconDepthLst.Count == _sprite_iconLst.Count)
		{
			for (int i = 0; i < _sprite_iconLst.Count; i++)
			{
				_sprite_iconLst[i].depth = _sprite_iconDepthLst[i];
			}
		}
		_sprite_light.enabled = false;
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UISquare_GotoCardMgrClick, null, null);
	}
}
