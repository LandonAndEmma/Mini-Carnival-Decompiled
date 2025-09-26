using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using NGUI_COMUI;
using Protocol.RPG.C2S;
using Protocol.RPG.S2C;
using UnityEngine;

public class UIRPGCardCompound_Container : NGUI_COMUI.UI_Container
{
	[SerializeField]
	private UIRPGCardCompoundMgr _mgr;

	[SerializeField]
	private List<UIRPG_CardMgr_Card_Box_Simple> _lstCardBoxSimple = new List<UIRPG_CardMgr_Card_Box_Simple>();

	[SerializeField]
	private UISprite _goldSprite;

	[SerializeField]
	private UILabel _labelCompoundFee;

	[SerializeField]
	private GameObject _btnCompound;

	[SerializeField]
	private Transform _compoundResultTran;

	protected override void Load()
	{
		ClearSimpleBox();
		base.Load();
		RegisterMessage(EUIMessageID.UIRPGCardCompound_BtnClick, this, CardCompound_BtnClick);
		RegisterMessage(EUIMessageID.UIDataBuffer_RoleData_RoleInfoChanged, this, HandleRoleDataRoleInfoChanged);
		RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, OnPopBoxClick_Yes);
	}

	protected override void UnLoad()
	{
		base.UnLoad();
		UnregisterMessage(EUIMessageID.UIRPGCardCompound_BtnClick, this);
		UnregisterMessage(EUIMessageID.UIDataBuffer_RoleData_RoleInfoChanged, this);
		UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
	}

	private bool OnPopBoxClick_Yes(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
		Debug.Log(uIMessage_CommonBoxData.MessageBoxID + " " + uIMessage_CommonBoxData.Mark);
		switch (uIMessage_CommonBoxData.Mark)
		{
		case "Lack of Money":
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenIAP, null, null);
			break;
		}
		return true;
	}

	private bool CardCompound_BtnClick(TUITelegram msg)
	{
		if (_preSelBoxLst.Count < 6)
		{
			string des = TUITool.StringFormat(Localization.instance.Get("cardfactory_desc2"), 6);
			UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(1, des);
			uIMessage_CommonBoxData.Mark = "cardfactory_desc2";
			UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
			return true;
		}
		if (COMA_Pref.Instance.NG2_1_FirstEnterSquare)
		{
			UIDataBufferCenter.Instance.CurNGIndex = 6;
			int card_id = Random.Range(9, 16);
			NotifyCardAddCmd notifyCardAddCmd = new NotifyCardAddCmd();
			notifyCardAddCmd.m_unique_id = 1uL;
			notifyCardAddCmd.m_card_id = (uint)card_id;
			bool flag = true;
			Dictionary<uint, List<ulong>> card_list = UIDataBufferCenter.Instance.RPGData.m_card_list;
			if (card_list.ContainsKey(notifyCardAddCmd.m_card_id))
			{
				List<ulong> list = card_list[notifyCardAddCmd.m_card_id];
				if (list == null)
				{
					list = new List<ulong>();
				}
				list.Add(notifyCardAddCmd.m_unique_id);
				flag = false;
			}
			else
			{
				List<ulong> list2 = new List<ulong>();
				list2.Add(notifyCardAddCmd.m_unique_id);
				card_list.Add(notifyCardAddCmd.m_card_id, list2);
			}
			notifyCardAddCmd.m_new = (byte)(flag ? 1u : 0u);
			for (int i = 0; i < 6; i++)
			{
				UIRPG_CardMgr_Card_BoxData uIRPG_CardMgr_Card_BoxData = _lstCardBoxSimple[i].BoxData as UIRPG_CardMgr_Card_BoxData;
				uint cardId = (uint)uIRPG_CardMgr_Card_BoxData.CardId;
				card_list[cardId].RemoveAt(0);
			}
			_mgr.InitContainer();
			ClearSimpleBox();
			CombineCardResultCmd combineCardResultCmd = new CombineCardResultCmd();
			combineCardResultCmd.m_result = 0;
			combineCardResultCmd.m_card_id = notifyCardAddCmd.m_card_id;
			combineCardResultCmd.m_unique_id = 1uL;
			InitCompoundResult(combineCardResultCmd);
		}
		else
		{
			UIRPG_CardMgr_Card_BoxData uIRPG_CardMgr_Card_BoxData2 = _lstCardBoxSimple[0].BoxData as UIRPG_CardMgr_Card_BoxData;
			if (uIRPG_CardMgr_Card_BoxData2 != null)
			{
				int fee = RPGGlobalData.Instance.CompoundFeePool._cardToCardList[uIRPG_CardMgr_Card_BoxData2.CardGrade - 1]._fee;
				if (fee > UIDataBufferCenter.Instance.playerInfo.m_gold)
				{
					string des2 = TUITool.StringFormat(Localization.instance.Get("shangdianjiemian_desc28"));
					UIMessage_CommonBoxData uIMessage_CommonBoxData2 = new UIMessage_CommonBoxData(0, des2);
					uIMessage_CommonBoxData2.Mark = "Lack of Money";
					UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData2);
					return true;
				}
				UIGolbalStaticFun.PopBlockOnlyMessageBox();
				RequestCombineCardCmd requestCombineCardCmd = new RequestCombineCardCmd();
				for (int j = 0; j < 6; j++)
				{
					UIRPG_CardMgr_Card_BoxData uIRPG_CardMgr_Card_BoxData3 = _lstCardBoxSimple[j].BoxData as UIRPG_CardMgr_Card_BoxData;
					requestCombineCardCmd.m_card_list[j] = new RequestCombineCardCmd.Card();
					requestCombineCardCmd.m_card_list[j].m_card_id = (uint)uIRPG_CardMgr_Card_BoxData3.CardId;
					requestCombineCardCmd.m_card_list[j].m_unique_id = uIRPG_CardMgr_Card_BoxData3.ItemId;
					requestCombineCardCmd.m_star = (byte)uIRPG_CardMgr_Card_BoxData3.CardGrade;
				}
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, requestCombineCardCmd);
				COMA_HTTP_DataCollect.Instance.SendCompoundCardCount("1");
			}
		}
		return true;
	}

	public bool HandleRoleDataRoleInfoChanged(TUITelegram msg)
	{
		UpdateSimpleBox();
		return true;
	}

	public void InitCompoundResult(CombineCardResultCmd item)
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.RPG_Card_upgrade);
		GameObject gameObject = Object.Instantiate(Resources.Load("UI/RPG_Prefabs/UIRPG_GetCard")) as GameObject;
		UIRPG_BigCard component = gameObject.GetComponent<UIRPG_BigCard>();
		gameObject.transform.parent = _compoundResultTran;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		component.InitCard(item);
	}

	public void ClearSimpleBox()
	{
		Debug.LogWarning("-------------------------------------ClearSimpleBox");
		for (int i = 0; i < _lstCardBoxSimple.Count; i++)
		{
			_lstCardBoxSimple[i].BoxData = null;
		}
		_labelCompoundFee.enabled = false;
		_goldSprite.gameObject.SetActive(false);
	}

	private void UpdateSimpleBox()
	{
		for (int i = 0; i < _preSelBoxLst.Count; i++)
		{
			UIRPG_CardMgr_Card_BoxData uIRPG_CardMgr_Card_BoxData = _preSelBoxLst[i].BoxData as UIRPG_CardMgr_Card_BoxData;
			UIRPG_CardMgr_Card_BoxData uIRPG_CardMgr_Card_BoxData2 = new UIRPG_CardMgr_Card_BoxData(uIRPG_CardMgr_Card_BoxData.CardGrade, uIRPG_CardMgr_Card_BoxData.CardId, uIRPG_CardMgr_Card_BoxData.CardName);
			uIRPG_CardMgr_Card_BoxData2.ItemId = uIRPG_CardMgr_Card_BoxData.ItemId;
			_lstCardBoxSimple[i].BoxData = uIRPG_CardMgr_Card_BoxData2;
		}
		for (int j = _preSelBoxLst.Count; j < _lstCardBoxSimple.Count; j++)
		{
			_lstCardBoxSimple[j].BoxData = null;
		}
		if (_preSelBoxLst.Count >= 1)
		{
			bool flag = true;
			if (COMA_Pref.Instance.NG2_1_FirstEnterSquare && UIDataBufferCenter.Instance.CurNGIndex == 5)
			{
				flag = false;
			}
			_goldSprite.gameObject.SetActive(flag);
			_labelCompoundFee.enabled = flag;
			UIRPG_CardMgr_Card_BoxData uIRPG_CardMgr_Card_BoxData3 = _lstCardBoxSimple[0].BoxData as UIRPG_CardMgr_Card_BoxData;
			int fee = RPGGlobalData.Instance.CompoundFeePool._cardToCardList[uIRPG_CardMgr_Card_BoxData3.CardGrade - 1]._fee;
			_labelCompoundFee.text = fee.ToString();
			if (UIDataBufferCenter.Instance.playerInfo.m_gold >= fee)
			{
				_labelCompoundFee.color = Color.white;
			}
			else
			{
				_labelCompoundFee.color = Color.red;
			}
		}
		else if (_preSelBoxLst.Count == 0)
		{
			_labelCompoundFee.enabled = false;
			_goldSprite.gameObject.SetActive(false);
		}
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}

	protected override bool IsCanSelBox(NGUI_COMUI.UI_Box box, out NGUI_COMUI.UI_Box loseSel)
	{
		if (base.BoxSelType == EBoxSelType.Multi)
		{
			UIRPG_CardMgr_Card_BoxData uIRPG_CardMgr_Card_BoxData = box.BoxData as UIRPG_CardMgr_Card_BoxData;
			if (uIRPG_CardMgr_Card_BoxData != null)
			{
				if (box != _curSelBox && !IsExistInPreList(box) && GetInPreListCount() < 6)
				{
					loseSel = null;
					if (uIRPG_CardMgr_Card_BoxData.LimitSel)
					{
						return false;
					}
					return true;
				}
				if (IsExistInPreList(box))
				{
					loseSel = box;
					return false;
				}
				loseSel = null;
				return false;
			}
			loseSel = null;
			return false;
		}
		loseSel = null;
		return false;
	}

	protected override void ProcessBoxSelected(NGUI_COMUI.UI_Box box)
	{
		base.ProcessBoxSelected(box);
		UIRPG_CardMgr_Card_BoxData uIRPG_CardMgr_Card_BoxData = box.BoxData as UIRPG_CardMgr_Card_BoxData;
		for (int i = 0; i < base.LstBoxs.Count; i++)
		{
			UIRPG_CardMgr_Card_BoxData uIRPG_CardMgr_Card_BoxData2 = base.LstBoxs[i].BoxData as UIRPG_CardMgr_Card_BoxData;
			if (uIRPG_CardMgr_Card_BoxData2.CardGrade != uIRPG_CardMgr_Card_BoxData.CardGrade)
			{
				uIRPG_CardMgr_Card_BoxData2.LimitSel = true;
				uIRPG_CardMgr_Card_BoxData2.SetDirty();
			}
		}
		UpdateSimpleBox();
	}

	protected override void ProcessBoxLoseSelected(NGUI_COMUI.UI_Box box)
	{
		base.ProcessBoxLoseSelected(box);
		if (GetInPreListCount() == 0)
		{
			for (int i = 0; i < base.LstBoxs.Count; i++)
			{
				UIRPG_CardMgr_Card_BoxData uIRPG_CardMgr_Card_BoxData = base.LstBoxs[i].BoxData as UIRPG_CardMgr_Card_BoxData;
				uIRPG_CardMgr_Card_BoxData.LimitSel = false;
				uIRPG_CardMgr_Card_BoxData.SetDirty();
			}
		}
		UpdateSimpleBox();
	}

	protected override void ProcessBoxCanntSelected(NGUI_COMUI.UI_Box box)
	{
	}

	public override void DataSort()
	{
		List<NGUI_COMUI.UI_BoxData> list = new List<NGUI_COMUI.UI_BoxData>();
		for (int i = 0; i < base.LstBoxs.Count; i++)
		{
			UIRPG_CardMgr_Card_BoxData uIRPG_CardMgr_Card_BoxData = base.LstBoxs[i].BoxData as UIRPG_CardMgr_Card_BoxData;
			if (uIRPG_CardMgr_Card_BoxData != null && uIRPG_CardMgr_Card_BoxData.DataType == 3)
			{
				list.Add(base.LstBoxs[i].BoxData);
			}
		}
		list.Sort();
		Debug.Log(list.Count);
		int num = 0;
		for (int num2 = list.Count - 1; num2 >= 0; num2--)
		{
			base.LstBoxs[num++].BoxData = list[num2];
		}
	}
}
