using System.Collections.Generic;
using MessageID;
using UnityEngine;

public class UISquare_GotoCompoundCard : UIEntity
{
	[SerializeField]
	private UISprite _sprite_light;

	[SerializeField]
	private List<UIWidget> _sprite_iconLst = new List<UIWidget>();

	private List<int> _sprite_iconDepthLst = new List<int>();

	[SerializeField]
	private GameObject _owner;

	private bool SquareBoardBtnDown(TUITelegram msg)
	{
		int num = (int)msg._pExtraInfo;
		int num2 = num;
		if (num2 != 2)
		{
		}
		return true;
	}

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_SquareBoardBtnDown, this, SquareBoardBtnDown);
		if (COMA_Pref.Instance.NG2_1_FirstEnterSquare && UIDataBufferCenter.Instance.CurNGIndex == 4)
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
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_SquareBoardBtnDown, this);
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		_owner.SetActive(false);
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
		if (COMA_Pref.Instance.NG2_1_FirstEnterSquare && UIDataBufferCenter.Instance.CurNGIndex == 4)
		{
			UIDataBufferCenter.Instance.CurNGIndex = 5;
		}
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UISquare_GotoCompoundCardClick, null, null);
	}
}
