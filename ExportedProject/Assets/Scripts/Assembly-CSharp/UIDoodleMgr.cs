using System;
using UnityEngine;

public class UIDoodleMgr : UIMessageHandler
{
	public UIDoodleMgr_Container doodleMgrContainer;

	[SerializeField]
	private UIDoodleMgr_PreviewBoard previewBoard;

	public COMA_PlayerSelfCharacter playerOnShowCom;

	[SerializeField]
	private UI_NewcomersWizardMask _wizardMask7;

	[SerializeField]
	private UI_NewcomersWizardMgr _wizardMgr7;

	[SerializeField]
	private UI_NewcomersWizardMask _wizardMask8;

	[SerializeField]
	private UI_NewcomersWizardMgr _wizardMgr8;

	[SerializeField]
	private UI_NewGuideBoard _newGuideBoard;

	private UIDoodleMgr_InventoryBoxSlot curSel;

	private UIDoodleMgr_InventoryBoxSlot preSel;

	[SerializeField]
	private TUILabel _equipLabel;

	public UIDoodleMgr_InventoryBoxSlot CurSelSlot
	{
		get
		{
			return curSel;
		}
	}

	public void HandleEventButtonNewGuide(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButtonNewGuide-CommandClick");
			if (_newGuideBoard != null)
			{
				_newGuideBoard.ProcessEvent();
			}
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void ProcessTeachingDoodle()
	{
		if (_wizardMask7 != null)
		{
			Debug.Log("---ProcessTeachingDoodle");
			UIDoodleMgr_InventoryBoxSlot uIDoodleMgr_InventoryBoxSlot = (UIDoodleMgr_InventoryBoxSlot)doodleMgrContainer.LstBoxs[0].Slots[3];
			Debug.Log("---ProcessTeachingDoodle1");
			COMA_Scene_Inventroy.selectedSlot = uIDoodleMgr_InventoryBoxSlot;
			Debug.Log("---ProcessTeachingDoodle2");
			uIDoodleMgr_InventoryBoxSlot.ProcessSelected(true);
			Debug.Log("---ProcessTeachingDoodle3");
			COMA_Sys.Instance.nTeachingId++;
			if (_wizardMgr7 != null)
			{
				_wizardMgr7.ResetUIControllersZ();
				_wizardMgr7.gameObject.SetActive(false);
			}
			_wizardMask8.gameObject.SetActive(true);
			_wizardMgr8.gameObject.SetActive(true);
			_wizardMgr8.RefreshUIControllers();
		}
	}

	private void ProcessGuideBoard1()
	{
		_newGuideBoard.InitLabel(new Vector2(-96f, -20f), "NoviceProcess_33");
		_newGuideBoard.AddProceHandler(ProcessGuideBoard2);
	}

	private void ProcessGuideBoard2()
	{
		_newGuideBoard.InitLabel(new Vector2(-96f, -20f), "NoviceProcess_34");
		_newGuideBoard.AddProceHandler(ProcessGuideBoard3);
	}

	private void ProcessGuideBoard3()
	{
		_wizardMgr8.ResetUIControllersZ();
		_wizardMgr8.gameObject.SetActive(false);
		_newGuideBoard.EndLabel();
		_wizardMask8.gameObject.SetActive(false);
		COMA_Pref.Instance.bNew_Package = false;
		COMA_Pref.Instance.Save(true);
	}

	public void ProcessEnterEnd()
	{
		if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 7 && _wizardMgr7 != null)
		{
			_wizardMgr7.gameObject.SetActive(true);
			_wizardMgr7.RefreshUIControllers();
		}
		if (COMA_Pref.Instance.bNew_Package)
		{
			_newGuideBoard.InitLabel(new Vector2(-96f, -20f), "NoviceProcess_32");
			_newGuideBoard.AddProceHandler(ProcessGuideBoard1);
			_wizardMgr8.gameObject.SetActive(true);
			_wizardMgr8.RefreshUIControllers(-220f);
		}
	}

	public void NeedMoreCoin(string param)
	{
		EnterIAPUI("UI.DoodleMgr", null);
	}

	public void ProcessUnlockSlot(int nIndex)
	{
		int unlockPrice = COMA_Package.unlockPrice;
		if (COMA_Pref.Instance.GetCrystal() < unlockPrice)
		{
			UI_MsgBox uI_MsgBox = TUI_MsgBox.Instance.MessageBox(109);
			uI_MsgBox.AddProceYesHandler(NeedMoreCoin);
			return;
		}
		COMA_Pref.Instance.AddCrystal(-unlockPrice);
		RefreshGoldAndCrystal();
		doodleMgrContainer.LstBoxs[nIndex / 4].Slots[nIndex % 4].SetSlot(UI_BoxSlot.ESlotState.UnLocked);
		COMA_Package.slotUnlocked++;
		COMA_HTTP_DataCollect.Instance.SendCrystalInfo(COMA_Pref.Instance.GetCrystal().ToString(), unlockPrice.ToString(), "unlock slot");
		COMA_HTTP_DataCollect.Instance.SendUnlockPackage(unlockPrice.ToString(), "1");
	}

	private void Awake()
	{
		if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 7 && _wizardMask7 != null)
		{
			_wizardMask7.ProceSingleBtn += ProcessTeachingDoodle;
			_wizardMask7.gameObject.SetActive(true);
		}
		TUITextManager.Instance().Parser("UI/language.en", "UI/language.en");
	}

	private void Start()
	{
		if (_goldLabel != null)
		{
			_goldLabel.Text = COMA_Pref.Instance.GetGold().ToString();
		}
		if (_gemLabel != null)
		{
			_gemLabel.Text = COMA_Pref.Instance.GetCrystal().ToString();
		}
		if (_newGuideBoard != null)
		{
			_newGuideBoard.EndLabel();
		}
		if (COMA_Pref.Instance.bNew_Package && _wizardMask8 != null)
		{
			_wizardMask8.gameObject.SetActive(true);
		}
	}

	private new void Update()
	{
	}

	public void HandleEventButton_IapEntry(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_IapEntry-CommandClick");
			EnterIAPUI("UI.DoodleMgr", _aniControl);
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventContainer_Move(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType != TUIScrollList.CommandMove)
		{
		}
	}

	public void HandleEventButton_back(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Back);
			COMA_Pref.Instance.Save(true);
			_aniControl.PlayExitAni("UI.MainMenu");
			COMA_Pref.Instance.UpPlayerTextureUpdate();
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void ProcessEquipStateChanged(bool bEquip)
	{
		if (bEquip)
		{
			_equipLabel.TextID = "jiechuzhuangbei_beibaojiemian";
		}
		else
		{
			_equipLabel.TextID = "zhuangbei_beibaojiemian";
		}
	}

	public void ProcessSelChange(UIDoodleMgr_InventoryBoxSlot boxSlot)
	{
		Debug.Log("Button_Doodle---CommandClick");
		curSel = boxSlot;
		if (curSel != preSel)
		{
			if (preSel != null)
			{
				preSel.ProcessSelected(false);
			}
			preSel = curSel;
		}
		if (curSel != null && curSel.BoxData != null)
		{
			_equipLabel.TextID = "zhuangbei_beibaojiemian";
		}
		if (COMA_Pref.Instance.package.pack[curSel.GetID()] == null)
		{
			return;
		}
		if (COMA_Pref.Instance.package.pack[curSel.GetID()].serialName.StartsWith("Head"))
		{
			playerOnShowCom.bodyObjs[0].renderer.material.mainTexture = COMA_Pref.Instance.package.pack[curSel.GetID()].texture;
			return;
		}
		if (COMA_Pref.Instance.package.pack[curSel.GetID()].serialName.StartsWith("Body"))
		{
			playerOnShowCom.bodyObjs[1].renderer.material.mainTexture = COMA_Pref.Instance.package.pack[curSel.GetID()].texture;
			return;
		}
		if (COMA_Pref.Instance.package.pack[curSel.GetID()].serialName.StartsWith("Leg"))
		{
			playerOnShowCom.bodyObjs[2].renderer.material.mainTexture = COMA_Pref.Instance.package.pack[curSel.GetID()].texture;
			return;
		}
		playerOnShowCom.CreateAccouterment(COMA_Pref.Instance.package.pack[curSel.GetID()].serialName);
		if (curSel != null && curSel.BoxData != null)
		{
			if (((UIDoodleMgr_InventoryBoxData)curSel.BoxData).IsEquiped())
			{
				_equipLabel.TextID = "jiechuzhuangbei_beibaojiemian";
			}
			else
			{
				_equipLabel.TextID = "zhuangbei_beibaojiemian";
			}
		}
	}

	private void DiscardItem(string param)
	{
		COMA_Pref.Instance.package.pack[CurSelSlot.GetID()].Delete();
		COMA_Pref.Instance.package.pack[CurSelSlot.GetID()] = null;
		CurSelSlot.BoxData = null;
	}

	public void HandleEventButton_Del(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			if (!(CurSelSlot == null) && CurSelSlot.BoxData != null)
			{
				UIDoodleMgr_InventoryBoxData uIDoodleMgr_InventoryBoxData = (UIDoodleMgr_InventoryBoxData)CurSelSlot.BoxData;
				if (uIDoodleMgr_InventoryBoxData.IsSelling())
				{
					Debug.LogWarning("it is selling!!");
				}
				else if (uIDoodleMgr_InventoryBoxData.IsEquiped())
				{
					Debug.LogWarning("it is equiped!!");
					TUI_MsgBox.Instance.MessageBox(115);
				}
				else
				{
					COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_DiscardItem);
					UI_MsgBox uI_MsgBox = TUI_MsgBox.Instance.MessageBox(111);
					uI_MsgBox.AddProceYesHandler(DiscardItem);
				}
			}
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_Edit(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			if (CurSelSlot == null || CurSelSlot.BoxData == null)
			{
				break;
			}
			UIDoodleMgr_InventoryBoxData uIDoodleMgr_InventoryBoxData = (UIDoodleMgr_InventoryBoxData)CurSelSlot.BoxData;
			if (uIDoodleMgr_InventoryBoxData.IsSelling() || COMA_Pref.Instance.package.pack[CurSelSlot.GetID()].part > 0)
			{
				break;
			}
			if (COMA_Pref.Instance.package.pack[CurSelSlot.GetID()].num == -1)
			{
				UI_MsgBox uI_MsgBox = TUI_MsgBox.Instance.MessageBox(125);
				break;
			}
			if (COMA_Pref.Instance.package.pack[CurSelSlot.GetID()].num == 0)
			{
				UI_MsgBox uI_MsgBox2 = TUI_MsgBox.Instance.MessageBox(133);
				break;
			}
			if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 8)
			{
				_wizardMgr8.ResetUIControllersZ();
				_wizardMgr8.gameObject.SetActive(false);
				COMA_Sys.Instance.nTeachingId++;
			}
			_aniControl.PlayExitAni("UI.Doodle1");
			COMA_HTTP_DataCollect.Instance.strTime1 = DateTime.Now.ToString();
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_Equip(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			if (CurSelSlot == null || CurSelSlot.BoxData == null)
			{
				break;
			}
			UIDoodleMgr_InventoryBoxData uIDoodleMgr_InventoryBoxData = (UIDoodleMgr_InventoryBoxData)CurSelSlot.BoxData;
			if (COMA_Pref.Instance.package.pack[CurSelSlot.GetID()].serialName.StartsWith("Head"))
			{
				UIDoodleMgr_InventoryBoxData uIDoodleMgr_InventoryBoxData2 = (UIDoodleMgr_InventoryBoxData)doodleMgrContainer.LstBoxDatas[COMA_Pref.Instance.TInPack[0]];
				uIDoodleMgr_InventoryBoxData2.InventoryItemsState = UIDoodleMgr_InventoryBoxData.EInventoryItemState.MeetIdle;
				uIDoodleMgr_InventoryBoxData.InventoryItemsState = UIDoodleMgr_InventoryBoxData.EInventoryItemState.MeetEquiped;
				COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[0]].state = COMA_PackageItem.PackageItemStatus.None;
				COMA_Pref.Instance.TInPack[0] = CurSelSlot.GetID();
				COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[0]].state = COMA_PackageItem.PackageItemStatus.Equiped;
				break;
			}
			if (COMA_Pref.Instance.package.pack[CurSelSlot.GetID()].serialName.StartsWith("Body"))
			{
				UIDoodleMgr_InventoryBoxData uIDoodleMgr_InventoryBoxData3 = (UIDoodleMgr_InventoryBoxData)doodleMgrContainer.LstBoxDatas[COMA_Pref.Instance.TInPack[1]];
				uIDoodleMgr_InventoryBoxData3.InventoryItemsState = UIDoodleMgr_InventoryBoxData.EInventoryItemState.MeetIdle;
				uIDoodleMgr_InventoryBoxData.InventoryItemsState = UIDoodleMgr_InventoryBoxData.EInventoryItemState.MeetEquiped;
				COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[1]].state = COMA_PackageItem.PackageItemStatus.None;
				COMA_Pref.Instance.TInPack[1] = CurSelSlot.GetID();
				COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[1]].state = COMA_PackageItem.PackageItemStatus.Equiped;
				break;
			}
			if (COMA_Pref.Instance.package.pack[CurSelSlot.GetID()].serialName.StartsWith("Leg"))
			{
				UIDoodleMgr_InventoryBoxData uIDoodleMgr_InventoryBoxData4 = (UIDoodleMgr_InventoryBoxData)doodleMgrContainer.LstBoxDatas[COMA_Pref.Instance.TInPack[2]];
				uIDoodleMgr_InventoryBoxData4.InventoryItemsState = UIDoodleMgr_InventoryBoxData.EInventoryItemState.MeetIdle;
				uIDoodleMgr_InventoryBoxData.InventoryItemsState = UIDoodleMgr_InventoryBoxData.EInventoryItemState.MeetEquiped;
				COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[2]].state = COMA_PackageItem.PackageItemStatus.None;
				COMA_Pref.Instance.TInPack[2] = CurSelSlot.GetID();
				COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[2]].state = COMA_PackageItem.PackageItemStatus.Equiped;
				break;
			}
			int num = -1;
			string serialName = COMA_Pref.Instance.package.pack[CurSelSlot.GetID()].serialName;
			if (serialName.StartsWith("HT"))
			{
				num = 0;
			}
			else if (serialName.StartsWith("HF"))
			{
				num = 1;
			}
			else if (serialName.StartsWith("HB"))
			{
				num = 2;
			}
			else if (serialName.StartsWith("HL"))
			{
				num = 3;
			}
			else if (serialName.StartsWith("HR"))
			{
				num = 4;
			}
			else if (serialName.StartsWith("CF"))
			{
				num = 5;
			}
			else if (serialName.StartsWith("CB"))
			{
				num = 6;
			}
			if (COMA_Pref.Instance.AInPack[num] == CurSelSlot.GetID())
			{
				ProcessEquipStateChanged(false);
				UIDoodleMgr_InventoryBoxData uIDoodleMgr_InventoryBoxData5 = (UIDoodleMgr_InventoryBoxData)doodleMgrContainer.LstBoxDatas[COMA_Pref.Instance.AInPack[num]];
				uIDoodleMgr_InventoryBoxData5.InventoryItemsState = UIDoodleMgr_InventoryBoxData.EInventoryItemState.MeetIdle;
				COMA_Pref.Instance.package.pack[COMA_Pref.Instance.AInPack[num]].state = COMA_PackageItem.PackageItemStatus.None;
				playerOnShowCom.DestroyAccouterment(COMA_Pref.Instance.package.pack[COMA_Pref.Instance.AInPack[num]].serialName);
				COMA_Pref.Instance.AInPack[num] = -1;
				break;
			}
			ProcessEquipStateChanged(true);
			if (COMA_Pref.Instance.AInPack[num] >= 0)
			{
				UIDoodleMgr_InventoryBoxData uIDoodleMgr_InventoryBoxData6 = (UIDoodleMgr_InventoryBoxData)doodleMgrContainer.LstBoxDatas[COMA_Pref.Instance.AInPack[num]];
				uIDoodleMgr_InventoryBoxData6.InventoryItemsState = UIDoodleMgr_InventoryBoxData.EInventoryItemState.MeetIdle;
			}
			uIDoodleMgr_InventoryBoxData.InventoryItemsState = UIDoodleMgr_InventoryBoxData.EInventoryItemState.MeetEquiped;
			if (COMA_Pref.Instance.AInPack[num] >= 0)
			{
				COMA_Pref.Instance.package.pack[COMA_Pref.Instance.AInPack[num]].state = COMA_PackageItem.PackageItemStatus.None;
			}
			COMA_Pref.Instance.AInPack[num] = CurSelSlot.GetID();
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.AInPack[num]].state = COMA_PackageItem.PackageItemStatus.Equiped;
			playerOnShowCom.CreateAccouterment(COMA_Pref.Instance.package.pack[COMA_Pref.Instance.AInPack[num]].serialName);
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_Copy(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	private void UnLockedLevel(string param)
	{
		Debug.Log("process unlocked level!");
	}

	public void HandleEventButton_PlayerRotate(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		float y = (0f - wparam) * COMA_Sys.Instance.sensitivity * 314f * 2f / (float)Screen.width;
		Quaternion quaternion = Quaternion.Euler(0f, y, 0f);
		playerOnShowCom.transform.rotation *= quaternion;
	}
}
