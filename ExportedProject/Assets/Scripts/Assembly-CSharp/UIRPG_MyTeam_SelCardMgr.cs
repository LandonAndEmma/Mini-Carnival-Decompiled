using System.Collections;
using System.Collections.Generic;
using MessageID;
using NGUI_COMUI;
using Protocol.RPG.S2C;
using UnityEngine;

public class UIRPG_MyTeam_SelCardMgr : UIEntity
{
	[SerializeField]
	private UIRPG_MyTeam_SelCardContainer _selCardContainer;

	[SerializeField]
	private UIRPG_MyTeamMgr _myTeamMgr;

	[SerializeField]
	private GameObject _popUpCardInfoObj;

	[SerializeField]
	private Transform _popUpCardInfoObjParent;

	[SerializeField]
	private UIDraggablePanel _draggablePanel;

	[SerializeField]
	private UILabel _maxCardLabel;

	private bool _selBtnStat;

	public UIRPG_MyTeam_SelCardContainer SelCardContainer
	{
		get
		{
			return _selCardContainer;
		}
	}

	public bool SelBtnStat
	{
		get
		{
			return _selBtnStat;
		}
		set
		{
			_selBtnStat = value;
		}
	}

	protected override void Load()
	{
		_maxCardLabel.text = UIRPG_DataBufferCenter.GetAvailableMemberSlot().ToString();
		InitContainer();
		RegisterMessage(EUIMessageID.UIRPG_PopupCardInfoWindow, this, HandlePopupCardInfoWindow);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIRPG_PopupCardInfoWindow, this);
	}

	public IEnumerator MultiFrameAddContainerBox()
	{
		NotifyRPGDataCmd rpgData = UIDataBufferCenter.Instance.RPGData;
		Dictionary<uint, List<ulong>> cardList = rpgData.m_card_list;
		int i = 0;
		int maxFrameBox = 20;
		int curi = i;
		foreach (uint key in cardList.Keys)
		{
			RPGCareerUnit careerUnit = RPGGlobalData.Instance.CareerUnitPool._dict[(int)key];
			List<ulong> lst = cardList[key];
			for (int j = 0; j < lst.Count; j++)
			{
				UIRPG_MyTeam_SelCardBoxData data = new UIRPG_MyTeam_SelCardBoxData(lst[j], key, careerUnit.StarGrade);
				data.SpriteName = "RPG_Small_" + data.CardId;
				data.IsSel = (_myTeamMgr._dict.ContainsKey(data.ItemId) ? true : false);
				data.IsCaptain = ((_myTeamMgr.CaptainTag == data.ItemId) ? true : false);
				_selCardContainer.SetBoxData(_selCardContainer.AddBox(i), data);
				i++;
				if (i - curi >= maxFrameBox)
				{
					curi = i;
					yield return 0;
				}
			}
		}
		if (i <= 18)
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
		_selCardContainer.ClearContainer();
		_selCardContainer.InitContainer(NGUI_COMUI.UI_Container.EBoxSelType.Single);
		StartCoroutine(MultiFrameAddContainerBox());
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
}
