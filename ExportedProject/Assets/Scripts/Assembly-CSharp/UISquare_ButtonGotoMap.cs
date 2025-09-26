using System.Collections.Generic;
using MessageID;
using UnityEngine;

public class UISquare_ButtonGotoMap : UIEntity
{
	[SerializeField]
	private UISprite _sprite_light;

	[SerializeField]
	private List<UISprite> _sprite_iconLst = new List<UISprite>();

	private List<int> _sprite_iconDepthLst = new List<int>();

	protected override void Load()
	{
		if (UIDataBufferCenter.Instance.CurNGIndex == 7)
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
	}

	private void OnClick()
	{
		if (_sprite_iconDepthLst.Count == _sprite_iconLst.Count)
		{
			for (int i = 0; i < _sprite_iconLst.Count; i++)
			{
				_sprite_iconLst[i].depth = _sprite_iconDepthLst[i];
			}
		}
		_sprite_light.enabled = false;
		if (UIDataBufferCenter.Instance.CurNGIndex == 7)
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_SquareBoardBtnDown, null, 66);
		}
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UISquare_GotoMap, null, null);
	}
}
