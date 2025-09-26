using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using Protocol.RPG.C2S;
using UnityEngine;

public class UISquare_GotoTeamMgr : UIEntity
{
	[SerializeField]
	private UISprite _sprite_light;

	[SerializeField]
	private List<UISprite> _sprite_iconLst = new List<UISprite>();

	private List<int> _sprite_iconDepthLst = new List<int>();

	private bool _bNeting;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_SquareBoardBtnDown, this, SquareBoardBtnDown);
		RegisterMessage(EUIMessageID.UIRPG_NG_2_1_End, this, NG_2_1_End);
		if (COMA_Pref.Instance.NG2_1_FirstEnterSquare && UIDataBufferCenter.Instance.CurNGIndex == 6)
		{
			Vector3 localPosition = base.transform.localPosition;
			localPosition.z = -150f;
			base.transform.localPosition = localPosition;
			_sprite_iconDepthLst.Clear();
			_sprite_light.enabled = true;
			Debug.Log("*********************************************1");
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
		UnregisterMessage(EUIMessageID.UIRPG_NG_2_1_End, this);
	}

	private bool SquareBoardBtnDown(TUITelegram msg)
	{
		int num = (int)msg._pExtraInfo;
		int num2 = num;
		if (num2 != 2)
		{
		}
		return true;
	}

	private bool NG_2_1_End(TUITelegram msg)
	{
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UISquare_GotoTeamMgrClick, null, null);
		return true;
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		if (_sprite_iconDepthLst.Count == _sprite_iconLst.Count)
		{
			for (int i = 0; i < _sprite_iconLst.Count; i++)
			{
				_sprite_iconLst[i].depth = _sprite_iconDepthLst[i];
			}
		}
		_sprite_light.enabled = false;
		Debug.Log("*********************************************0");
		if (COMA_Pref.Instance.NG2_1_FirstEnterSquare)
		{
			UIGolbalStaticFun.PopBlockOnlyMessageBox();
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_SquareBoardBtnDown, null, 55);
			FirstRpgFinishCmd firstRpgFinishCmd = new FirstRpgFinishCmd();
			Dictionary<uint, List<ulong>> card_list = UIDataBufferCenter.Instance.RPGData.m_card_list;
			foreach (uint key in card_list.Keys)
			{
				int count = card_list[key].Count;
				for (int j = 0; j < count; j++)
				{
					firstRpgFinishCmd.m_card_list.Add(key);
				}
			}
			firstRpgFinishCmd.m_card_num = (byte)firstRpgFinishCmd.m_card_list.Count;
			Debug.Log("Send Card NUM:" + firstRpgFinishCmd.m_card_num);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, firstRpgFinishCmd);
			UIDataBufferCenter.Instance.RPGData.m_card_list.Clear();
			_bNeting = true;
		}
		else
		{
			UIMessageDispatch.Instance.PostMessage(EUIMessageID.UISquare_GotoTeamMgrClick, null, null);
		}
	}
}
