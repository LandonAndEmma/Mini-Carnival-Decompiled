using UnityEngine;

public class UIDoodleMgr_Container : UI_Container
{
	private int nSimulateBoxCount = COMA_Package.maxCount / 4;

	private Vector3 page0Pos = new Vector3(112f, 55.73529f, -10f);

	private void Awake()
	{
		PreInit();
		objBoxPerfab = Resources.Load("UI/DoodleMgr/InventoryBox") as GameObject;
	}

	private void Start()
	{
		Init();
	}

	private new void Update()
	{
	}

	private void FixedUpdate()
	{
	}

	private void SimulateCall_Init()
	{
		int num = CreateBox(nSimulateBoxCount, 0);
		for (int i = 0; i < nSimulateBoxCount; i++)
		{
			UI_Box uI_Box = lstBoxs[i];
			for (int j = 0; j < uI_Box.Slots.Length; j++)
			{
				UI_BoxSlot uI_BoxSlot = uI_Box.Slots[j];
				if (!uI_BoxSlot.IsLocked())
				{
					Random.seed = ((int)Time.timeSinceLevelLoad + i) * 10245;
					UIDoodleMgr_InventoryBoxData boxData = new UIDoodleMgr_InventoryBoxData();
					AddBoxData(uI_BoxSlot.GetID(), boxData);
				}
			}
		}
		RefreshContainer();
	}

	public void Init()
	{
		int num = 0;
		Debug.Log("nSimulateBoxCount=" + nSimulateBoxCount);
		int num2 = CreateBox(nSimulateBoxCount, 0);
		for (int i = 0; i < nSimulateBoxCount; i++)
		{
			UI_Box uI_Box = lstBoxs[i];
			for (int j = 0; j < uI_Box.Slots.Length; j++)
			{
				UI_BoxSlot uI_BoxSlot = uI_Box.Slots[j];
				UIDoodleMgr_InventoryBoxData uIDoodleMgr_InventoryBoxData = null;
				int num3 = i * 4 + j;
				if (COMA_Pref.Instance.package.pack[num3] != null)
				{
					uIDoodleMgr_InventoryBoxData = new UIDoodleMgr_InventoryBoxData();
					if (COMA_Pref.Instance.package.pack[num3].state == COMA_PackageItem.PackageItemStatus.None)
					{
						uIDoodleMgr_InventoryBoxData.InventoryItemsState = UIDoodleMgr_InventoryBoxData.EInventoryItemState.MeetIdle;
					}
					else if (COMA_Pref.Instance.package.pack[num3].state == COMA_PackageItem.PackageItemStatus.Equiped)
					{
						uIDoodleMgr_InventoryBoxData.InventoryItemsState = UIDoodleMgr_InventoryBoxData.EInventoryItemState.MeetEquiped;
					}
					uIDoodleMgr_InventoryBoxData.CanSell = ((COMA_Pref.Instance.package.pack[num3].num > 1) ? true : false);
					UIDoodleMgr_InventoryBoxSlot uIDoodleMgr_InventoryBoxSlot = (UIDoodleMgr_InventoryBoxSlot)uI_BoxSlot;
					if (COMA_Pref.Instance.package.pack[num3].iconTextureName != string.Empty)
					{
						uIDoodleMgr_InventoryBoxSlot.SetIconPic(COMA_Pref.Instance.package.pack[num3].iconTextureName);
					}
					else
					{
						uIDoodleMgr_InventoryBoxSlot.SetIconPic(COMA_Pref.Instance.package.pack[num3].iconTexture);
					}
					if (COMA_Pref.Instance.package.pack[num3].part > 0)
					{
						num++;
					}
				}
				if (uI_BoxSlot.GetID() >= COMA_Package.slotUnlocked)
				{
					uI_BoxSlot.SetSlot();
				}
				AddBoxData(uI_BoxSlot.GetID(), uIDoodleMgr_InventoryBoxData);
			}
		}
		RefreshContainer();
		COMA_Achievement.Instance.Rich = num;
	}

	public override int ExtraInit(int nSlotCount)
	{
		int num = 0;
		int num2 = 0;
		foreach (UI_Box lstBox in lstBoxs)
		{
			num2++;
			int num3 = 0;
			UIDoodleMgr_InventoryBox uIDoodleMgr_InventoryBox = (UIDoodleMgr_InventoryBox)lstBox;
			UI_BoxSlot[] slots = uIDoodleMgr_InventoryBox.Slots;
			foreach (UI_BoxSlot uI_BoxSlot in slots)
			{
				num3++;
				UIDoodleMgr_InventoryBoxSlot uIDoodleMgr_InventoryBoxSlot = (UIDoodleMgr_InventoryBoxSlot)uI_BoxSlot;
				UI_BoxSlot.ESlotState slot = UI_BoxSlot.ESlotState.UnLocked;
				uIDoodleMgr_InventoryBoxSlot.SetSlot(slot);
				num++;
			}
		}
		return num;
	}
}
