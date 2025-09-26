using UnityEngine;

public class UIDoodleMgr_InventoryBoxSlot : UI_BoxSlot
{
	[SerializeField]
	private GameObject bkNormal;

	[SerializeField]
	private GameObject bkSelected;

	[SerializeField]
	private GameObject bkLackLV;

	[SerializeField]
	private GameObject lockPic;

	[SerializeField]
	private GameObject doodlePic;

	[SerializeField]
	private GameObject equippedPic;

	[SerializeField]
	private GameObject sellingPic;

	[SerializeField]
	private GameObject modelsPic;

	private bool bCurSelected;

	public bool CurSelected
	{
		get
		{
			return bCurSelected;
		}
		set
		{
			bCurSelected = value;
			NotifyDataUpdate();
		}
	}

	public void SetIconPic(Texture2D pic)
	{
		TUIMeshSprite component = doodlePic.GetComponent<TUIMeshSprite>();
		component.UseCustomize = true;
		component.CustomizeTexture = pic;
		component.CustomizeRect = new Rect(0f, 0f, pic.width, pic.height);
		doodlePic.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
	}

	public void SetIconPic(string pic)
	{
		TUIMeshSprite component = doodlePic.GetComponent<TUIMeshSprite>();
		component.UseCustomize = false;
		component.texture = pic;
		doodlePic.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	public void DeleteIconPic()
	{
		TUIMeshSprite component = doodlePic.GetComponent<TUIMeshSprite>();
		Object.DestroyObject(component.CustomizeTexture);
		component.CustomizeTexture = null;
		component.UseCustomize = false;
	}

	public void NeedMoreCoin(string param)
	{
		EnterIAPUI("UI.Market", null);
	}

	private void UnLockedSlot(string param)
	{
		UIDoodleMgr uIDoodleMgr = (UIDoodleMgr)GetTUIMessageHandler(true);
		if (uIDoodleMgr != null)
		{
			uIDoodleMgr.ProcessUnlockSlot(COMA_Package.slotUnlocked);
		}
	}

	public UI_BoxSlot ProcessSelected(bool bSel)
	{
		if (IsLocked())
		{
			CurSelected = false;
			bkSelected.active = false;
			UI_MsgBox uI_MsgBox = TUI_MsgBox.Instance.MessageBox(228, null, 1, COMA_Package.unlockPrice);
			uI_MsgBox.AddProceYesHandler(UnLockedSlot);
			return null;
		}
		CurSelected = bSel;
		bkSelected.active = bSel;
		if (!CurSelected)
		{
			return null;
		}
		UIDoodleMgr uIDoodleMgr = (UIDoodleMgr)GetTUIMessageHandler(true);
		if (uIDoodleMgr != null)
		{
			uIDoodleMgr.ProcessSelChange(this);
		}
		return this;
	}

	private void Awake()
	{
		ProcessSelected(false);
	}

	private void Start()
	{
	}

	private new void Update()
	{
	}

	protected override void ProcessNullData()
	{
		base.ProcessNullData();
		lockPic.active = false;
		equippedPic.active = false;
		doodlePic.active = false;
		sellingPic.active = false;
		bkNormal.active = true;
		bkSelected.active = false;
		bkLackLV.active = false;
		modelsPic.SetActive(false);
	}

	public override int NotifyDataUpdate()
	{
		if (IsLocked())
		{
			lockPic.active = true;
			equippedPic.active = false;
			doodlePic.active = false;
			sellingPic.active = false;
			bkNormal.active = true;
			bkSelected.active = false;
			bkLackLV.active = false;
			return -2;
		}
		lockPic.active = false;
		UIDoodleMgr_InventoryBoxData uIDoodleMgr_InventoryBoxData = (UIDoodleMgr_InventoryBoxData)base.BoxData;
		if (base.NotifyDataUpdate() == -1)
		{
			return -1;
		}
		if (uIDoodleMgr_InventoryBoxData.IsLackLV())
		{
			bkLackLV.active = true;
			bkNormal.active = false;
		}
		else
		{
			bkLackLV.active = false;
			bkNormal.active = true;
		}
		if (uIDoodleMgr_InventoryBoxData.IsEquiped())
		{
			equippedPic.active = true;
			sellingPic.active = false;
		}
		else if (uIDoodleMgr_InventoryBoxData.IsSelling())
		{
			equippedPic.active = false;
			sellingPic.active = true;
		}
		else if (uIDoodleMgr_InventoryBoxData.IsIdle())
		{
			equippedPic.active = false;
			sellingPic.active = false;
		}
		if (uIDoodleMgr_InventoryBoxData.CanSell)
		{
			modelsPic.SetActive(true);
		}
		else
		{
			modelsPic.SetActive(false);
		}
		doodlePic.active = true;
		return 0;
	}

	public void HandleEventButton_Doodle(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("Button_Doodle---CommandClick");
			COMA_Scene_Inventroy.selectedSlot = this;
			ProcessSelected(true);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		}
	}
}
