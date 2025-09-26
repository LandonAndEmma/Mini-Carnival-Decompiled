using System.Collections;
using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using Protocol.RPG.S2C;
using UnityEngine;

namespace NGUI_COMUI
{
	public class UIRPGCardCompoundMgr : UIEntity
	{
		[SerializeField]
		private UIRPGCardCompound_Container _cardCompundContainer;

		[SerializeField]
		private GameObject _popUpCardInfoObj;

		[SerializeField]
		private Transform _popUpCardInfoObjParent;

		[SerializeField]
		private UIDraggablePanel _draggablePanel;

		protected override void Load()
		{
			InitContainer();
			RegisterMessage(EUIMessageID.UIRPG_NotifyCombineCardResult, this, NotifyCombineCardResult);
			RegisterMessage(EUIMessageID.UIRPG_PopupCardInfoWindow, this, HandlePopupCardInfoWindow);
			RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, OnPopBoxClick_Yes);
		}

		protected override void UnLoad()
		{
			UnregisterMessage(EUIMessageID.UIRPG_NotifyCombineCardResult, this);
			UnregisterMessage(EUIMessageID.UIRPG_PopupCardInfoWindow, this);
			UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
		}

		private bool NotifyCombineCardResult(TUITelegram msg)
		{
			CombineCardResultCmd combineCardResultCmd = msg._pExtraInfo as CombineCardResultCmd;
			if (combineCardResultCmd.m_result == 0)
			{
				InitContainer();
				_cardCompundContainer.ClearSimpleBox();
				_cardCompundContainer.InitCompoundResult(combineCardResultCmd);
			}
			else if (combineCardResultCmd.m_result == 1)
			{
				string des = TUITool.StringFormat(Localization.instance.Get("shangdianjiemian_desc28"));
				UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, des);
				uIMessage_CommonBoxData.Mark = "LackofMoney";
				UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
			}
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

		public bool OnPopBoxClick_Yes(TUITelegram msg)
		{
			UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
			Debug.Log(uIMessage_CommonBoxData.MessageBoxID + " " + uIMessage_CommonBoxData.Mark);
			switch (uIMessage_CommonBoxData.Mark)
			{
			case "LackofMoney":
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenIAP, null, null);
				break;
			}
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
			Dictionary<ulong, bool> _dict = new Dictionary<ulong, bool>();
			for (int kk = 0; kk < rpgData.m_member_slot.Length; kk++)
			{
				if (rpgData.m_member_slot[kk].m_member != 0 && rpgData.m_member_slot[kk].m_unqiue != 0L)
				{
					Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^card equiped:" + rpgData.m_member_slot[kk].m_unqiue);
					_dict.Add(rpgData.m_member_slot[kk].m_unqiue, true);
				}
			}
			foreach (uint key in cardList.Keys)
			{
				RPGCareerUnit careerUnit = RPGGlobalData.Instance.CareerUnitPool._dict[(int)key];
				List<ulong> lst = cardList[key];
				for (int j = 0; j < lst.Count; j++)
				{
					if (!_dict.ContainsKey(lst[j]) && careerUnit.StarGrade < 6)
					{
						UIRPG_CardMgr_Card_BoxData data = new UIRPG_CardMgr_Card_BoxData(careerUnit.StarGrade, (int)key, careerUnit.CareerName)
						{
							ItemId = lst[j]
						};
						_cardCompundContainer.SetBoxData(_cardCompundContainer.AddBox(i), data);
						i++;
						if (i - curi >= maxFrameBox)
						{
							curi = i;
							yield return 0;
						}
					}
					else
					{
						Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^Except Card:" + lst[j]);
					}
				}
			}
			if (i <= 16)
			{
				_draggablePanel.scale = Vector3.zero;
			}
			else
			{
				_draggablePanel.scale = new Vector3(0f, 1f, 0f);
			}
		}

		public void InitContainer()
		{
			_cardCompundContainer.ClearContainer();
			_cardCompundContainer.InitContainer(UI_Container.EBoxSelType.Multi);
			StartCoroutine(MultiFrameAddContainerBox());
		}
	}
}
