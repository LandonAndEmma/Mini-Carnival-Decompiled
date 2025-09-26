using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using Protocol.RPG.S2C;
using UnityEngine;

[RequireComponent(typeof(UIPanel))]
public class UIPopupDialogEntity : UIEntity
{
	private int _gemId;

	[SerializeField]
	private List<UIMessageBoxMgr.EMessageBoxType> _LstBlock = new List<UIMessageBoxMgr.EMessageBoxType>();

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UI_OpenGameContent, this, OpenGameContent);
		RegisterMessage(EUIMessageID.UI_OpenIAP, this, OpenIAP);
		RegisterMessage(EUIMessageID.UI_PopupMessageBox, this, PopupMessageBox);
		RegisterMessage(EUIMessageID.UIRPG_NotifyGemNumChanged, this, NotifyGemNumChanged);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UI_OpenGameContent, this);
		UnregisterMessage(EUIMessageID.UI_OpenIAP, this);
		UnregisterMessage(EUIMessageID.UI_PopupMessageBox, this);
		UnregisterMessage(EUIMessageID.UIRPG_NotifyGemNumChanged, this);
	}

	private bool NotifyGemNumChanged(TUITelegram msg)
	{
		if (base.transform.root.name != "UI.RPG")
		{
			return true;
		}
		NotifyGemCmd notifyGemCmd = msg._pExtraInfo as NotifyGemCmd;
		_gemId = notifyGemCmd.m_gem_id;
		RPGRefree.Instance.SettlementMgr.OnSpecialAward = RPGSpecial_GemAward;
		return true;
	}

	private void RPGSpecial_GemAward()
	{
		UIGetItemBoxData data = new UIGetItemBoxData(UIGetItemBoxData.EGetItemType.Gem, 1, _gemId);
		UIGolbalStaticFun.PopGetItemBox(data);
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.RPG_Card_upgrade);
	}

	private bool OpenGameContent(TUITelegram msg)
	{
		if ((bool)base.transform.FindChild("GameModeSelect"))
		{
			return true;
		}
		GameObject gameObject = Object.Instantiate(Resources.Load("PopupDialog/GameModeSelect"), new Vector3(-1000f, 0f, 0f), Quaternion.identity) as GameObject;
		gameObject.name = "GameModeSelect";
		gameObject.transform.parent = base.transform;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		COMA_CommonOperation.Instance.bSelectModeLock = false;
		return true;
	}

	private bool OpenIAP(TUITelegram msg)
	{
		if ((bool)base.transform.FindChild("IAP"))
		{
			return true;
		}
		GameObject gameObject = Object.Instantiate(Resources.Load("PopupDialog/IAP"), new Vector3(-1000f, 0f, 0f), Quaternion.identity) as GameObject;
		gameObject.name = "IAP";
		gameObject.transform.parent = base.transform;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		return true;
	}

	private bool PopupMessageBox(TUITelegram msg)
	{
		UIMessage_BoxData uIMessage_BoxData = msg._pExtraInfo as UIMessage_BoxData;
		UIMessageBoxMgr uIMessageBoxMgr = msg._pSender as UIMessageBoxMgr;
		if (_LstBlock.Contains(uIMessage_BoxData.Type))
		{
			if (uIMessage_BoxData.Type != UIMessageBoxMgr.EMessageBoxType.GetItems || (uIMessage_BoxData.DataType != 4 && uIMessage_BoxData.DataType != 1 && uIMessage_BoxData.DataType != 5))
			{
				UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_MessageBoxDestory, null, uIMessage_BoxData.Channel);
				return true;
			}
			if (Application.loadedLevelName.StartsWith("COMA_Scene_RPG") && uIMessage_BoxData.DataType == 1)
			{
				return false;
			}
		}
		GameObject gameObject = Object.Instantiate(uIMessageBoxMgr.GetMessageBoxPrefabByType((int)uIMessage_BoxData.Type), new Vector3(-1000f, 0f, 0f), Quaternion.identity) as GameObject;
		gameObject.transform.parent = base.transform;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		UIMessage_Box component = gameObject.GetComponent<UIMessage_Box>();
		if (component != null)
		{
			component.BoxData = uIMessage_BoxData;
		}
		UIMessage_BoxEntity component2 = gameObject.GetComponent<UIMessage_BoxEntity>();
		if (component2 != null)
		{
			component2.MsgBoxData = uIMessage_BoxData;
		}
		return true;
	}

	public void BlockMsgBox(UIMessageBoxMgr.EMessageBoxType type)
	{
		if (!_LstBlock.Contains(type))
		{
			_LstBlock.Add(type);
		}
	}

	public void UnBlockMsgBox(UIMessageBoxMgr.EMessageBoxType type)
	{
		if (_LstBlock.Contains(type))
		{
			_LstBlock.Remove(type);
		}
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}
}
