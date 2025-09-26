using System.Collections;
using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using Protocol.RPG.C2S;
using Protocol.RPG.S2C;
using UIGlobal;
using UnityEngine;

namespace NGUI_COMUI
{
	public class UIRPGCardMgr : UIEntity
	{
		[SerializeField]
		private UIRPGCardManage_Container _cardMgrContainer;

		[SerializeField]
		private UILabel _couponCount_extractCardLabel;

		[SerializeField]
		private GameObject _popUpCardInfoObj;

		[SerializeField]
		private GameObject _popUpObtainCoupon;

		[SerializeField]
		private Transform _popUpCardInfoObjParent;

		public GameObject PopUpObtainCoupon
		{
			get
			{
				return _popUpObtainCoupon;
			}
		}

		protected override void Load()
		{
			InitContainer();
			_couponCount_extractCardLabel.text = RPGGlobalData.Instance.RpgMiscUnit._couponCount_extractCard.ToString();
			RegisterMessage(EUIMessageID.UICardMgr_UnlockBtnClick, this, UnLockBtnClick);
			RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, OnPopBoxClick_Yes);
			RegisterMessage(EUIMessageID.UIRPG_CardCapacityChange, this, CardCapacityChanged);
			RegisterMessage(EUIMessageID.UIRPG_CardCapactiyChangeError, this, HandleCapacityChangeError);
			RegisterMessage(EUIMessageID.UICardMgr_GotoCardLibraryClick, this, GotoCardLibraryClick);
			RegisterMessage(EUIMessageID.UIRPG_PopupCardInfoWindow, this, HandlePopupCardInfoWindow);
			RegisterMessage(EUIMessageID.UIRPG_Ani_MyTeamBackToSquare, this, BackToSquare);
		}

		protected override void UnLoad()
		{
			UnregisterMessage(EUIMessageID.UICardMgr_UnlockBtnClick, this);
			UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
			UnregisterMessage(EUIMessageID.UIRPG_CardCapacityChange, this);
			UnregisterMessage(EUIMessageID.UIRPG_CardCapactiyChangeError, this);
			UnregisterMessage(EUIMessageID.UICardMgr_GotoCardLibraryClick, this);
			UnregisterMessage(EUIMessageID.UIRPG_PopupCardInfoWindow, this);
			UnregisterMessage(EUIMessageID.UIRPG_Ani_MyTeamBackToSquare, this);
		}

		private bool UnLockBtnClick(TUITelegram msg)
		{
			string des = TUITool.StringFormat(Localization.instance.Get("beibaojiemian_desc7"), 1, RPGGlobalData.Instance.RpgMiscUnit._unitCardBagPrice);
			UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, des);
			uIMessage_CommonBoxData.Mark = "UnlockBagForCell";
			UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
			return true;
		}

		private bool OnPopBoxClick_Yes(TUITelegram msg)
		{
			UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
			Debug.Log(uIMessage_CommonBoxData.MessageBoxID + " " + uIMessage_CommonBoxData.Mark);
			switch (uIMessage_CommonBoxData.Mark)
			{
			case "UnlockBagForCell":
			{
				UIGolbalStaticFun.PopBlockOnlyMessageBox();
				BuyRpgBagCapacityCmd buyRpgBagCapacityCmd2 = new BuyRpgBagCapacityCmd();
				buyRpgBagCapacityCmd2.m_bag_type = 0;
				buyRpgBagCapacityCmd2.m_buy_num = 1;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, buyRpgBagCapacityCmd2);
				break;
			}
			case "UnlockBagForDrawCard":
			{
				int count = _cardMgrContainer.LstBoxs.Count;
				int num = 0;
				for (int i = 0; i < count; i++)
				{
					if (_cardMgrContainer.LstBoxs[i].BoxData.DataType == 0)
					{
						num++;
					}
				}
				UIGolbalStaticFun.PopBlockOnlyMessageBox();
				BuyRpgBagCapacityCmd buyRpgBagCapacityCmd = new BuyRpgBagCapacityCmd();
				buyRpgBagCapacityCmd.m_bag_type = 0;
				buyRpgBagCapacityCmd.m_buy_num = (byte)(RPGGlobalData.Instance.RpgMiscUnit._cardNum_extractCard - num);
				Debug.Log("cmd.m_buy_num : " + buyRpgBagCapacityCmd.m_buy_num);
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, buyRpgBagCapacityCmd);
				break;
			}
			case "AskBuyCrystals":
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenIAP, null, null);
				break;
			}
			return true;
		}

		private bool CardCapacityChanged(TUITelegram msg)
		{
			uint num = (uint)msg._pExtraInfo;
			Debug.Log("INew : " + num);
			int num2 = 0;
			for (int i = 0; i < _cardMgrContainer.LstBoxs.Count; i++)
			{
				if (_cardMgrContainer.LstBoxs[i].BoxData.DataType == 3)
				{
					num2++;
				}
			}
			for (int j = num2; j < num; j++)
			{
				Debug.Log("CardCapacityChanged i = " + j);
				UIRPG_CardMgr_Card_BoxData uIRPG_CardMgr_Card_BoxData = _cardMgrContainer.LstBoxs[j].BoxData as UIRPG_CardMgr_Card_BoxData;
				uIRPG_CardMgr_Card_BoxData.DataType = 0;
				uIRPG_CardMgr_Card_BoxData.SetDirty();
			}
			if (num < _cardMgrContainer.LstBoxs.Count && _cardMgrContainer.LstBoxs[(int)num].BoxData.DataType == 2)
			{
				_cardMgrContainer.LstBoxs[(int)num].BoxData.DataType = 1;
				_cardMgrContainer.LstBoxs[(int)num].BoxData.SetDirty();
			}
			return true;
		}

		private bool HandleCapacityChangeError(TUITelegram msg)
		{
			string des = TUITool.StringFormat(Localization.instance.Get("shangdianjiemian_desc28"));
			UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, des);
			uIMessage_CommonBoxData.Mark = "AskBuyCrystals";
			UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
			return true;
		}

		private bool GotoCardLibrary(object obj)
		{
			Debug.Log("GotoCardLibrary");
			UIGolbalStaticFun.CloseBlockForTUIMessageBox();
			TLoadScene extraInfo = new TLoadScene("UI.RPG.CardLibrary", ELoadLevelParam.LoadOnlyAnDestroyPre);
			UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
			if (COMA_Scene_PlayerController.Instance != null)
			{
				COMA_Scene_PlayerController.Instance.gameObject.SetActive(false);
			}
			return true;
		}

		private bool GotoCardLibraryClick(TUITelegram msg)
		{
			TLoadScene extraInfo = new TLoadScene("UI.RPG.CardLibrary", ELoadLevelParam.LoadOnlyAnDestroyPre);
			UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
			return true;
		}

		public bool HandlePopupCardInfoWindow(TUITelegram msg)
		{
			int cardId = (int)msg._pExtraInfo;
			GameObject gameObject = Object.Instantiate(_popUpCardInfoObj) as GameObject;
			gameObject.transform.parent = _popUpCardInfoObjParent;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			UIRPG_CardInfo component = gameObject.GetComponent<UIRPG_CardInfo>();
			component.CardId = cardId;
			component.DisplayData();
			return true;
		}

		private bool GotoSquare(object obj)
		{
			Debug.Log("GotoSquare");
			UIGolbalStaticFun.CloseBlockOnlyMessageBox();
			TLoadScene extraInfo = new TLoadScene("UI.Square", ELoadLevelParam.LoadOnlyAnDestroyPre);
			UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
			if (COMA_Scene_PlayerController.Instance != null)
			{
				COMA_Scene_PlayerController.Instance.gameObject.SetActive(true);
			}
			return true;
		}

		public bool BackToSquare(TUITelegram msg)
		{
			UIGolbalStaticFun.PopBlockOnlyMessageBox();
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoSquare);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
			return true;
		}

		private void Awake()
		{
		}

		private void Start()
		{
		}

		protected override void Tick()
		{
		}

		private IEnumerator MultiFrameAddContainerBox()
		{
			NotifyRPGDataCmd rpgData = UIDataBufferCenter.Instance.RPGData;
			Dictionary<uint, List<ulong>> cardList = rpgData.m_card_list;
			int i = 0;
			int maxFrameBox = 20;
			int curi = i;
			foreach (uint key in cardList.Keys)
			{
				Debug.LogWarning(key);
				RPGCareerUnit careerUnit = RPGGlobalData.Instance.CareerUnitPool._dict[(int)key];
				List<ulong> lst = cardList[key];
				for (int j = 0; j < lst.Count; j++)
				{
					UIRPG_CardMgr_Card_BoxData data = new UIRPG_CardMgr_Card_BoxData(careerUnit.StarGrade, (int)key, careerUnit.CareerName)
					{
						ItemId = lst[j]
					};
					_cardMgrContainer.SetBoxData(_cardMgrContainer.AddBox(i), data);
					i++;
					if (i - curi >= maxFrameBox)
					{
						curi = i;
						yield return 0;
					}
				}
			}
			yield return 0;
			curi = i;
			while (i < RPGGlobalData.Instance.RpgMiscUnit._maxCapacity_CardBag)
			{
				UIRPG_CardMgr_Card_BoxData data2 = new UIRPG_CardMgr_Card_BoxData
				{
					DataType = ((i >= UIDataBufferCenter.Instance.RPGData.m_card_capacity) ? 1 : 0)
				};
				if (data2.DataType == 1 && i - 1 >= 0 && i - 1 < _cardMgrContainer.LstBoxs.Count && (_cardMgrContainer.LstBoxs[i - 1].BoxData.DataType == 1 || _cardMgrContainer.LstBoxs[i - 1].BoxData.DataType == 2))
				{
					data2.DataType = 2;
				}
				_cardMgrContainer.SetBoxData(_cardMgrContainer.AddBox(i), data2);
				i++;
				if (i - curi >= maxFrameBox)
				{
					curi = i;
					yield return 0;
				}
			}
		}

		private void InitContainer()
		{
			_cardMgrContainer.InitContainer(UI_Container.EBoxSelType.Single);
			StartCoroutine(MultiFrameAddContainerBox());
		}
	}
}
