using UnityEngine;

public class UIDoodleMgr_InventoryBoxData : UI_BoxData
{
	public enum EInventoryItemState
	{
		MeetEquiped = 513,
		MeetSelling = 514,
		MeetIdle = 516,
		LackSelling = 258,
		LackIdle = 260
	}

	private EInventoryItemState[] _arrayState = new EInventoryItemState[5]
	{
		EInventoryItemState.MeetEquiped,
		EInventoryItemState.MeetSelling,
		EInventoryItemState.MeetIdle,
		EInventoryItemState.LackSelling,
		EInventoryItemState.LackIdle
	};

	private EInventoryItemState inventoryItemsState;

	public EInventoryItemState InventoryItemsState
	{
		get
		{
			return inventoryItemsState;
		}
		set
		{
			inventoryItemsState = value;
			DataChanged();
		}
	}

	public UIDoodleMgr_InventoryBoxData()
	{
		inventoryItemsState = _arrayState[Random.Range(0, 5)];
	}

	public bool IsSelling()
	{
		int num = (int)InventoryItemsState;
		int num2 = 1;
		num2 <<= 1;
		return ((num2 & num) != 0) ? true : false;
	}

	public bool IsEquiped()
	{
		int num = (int)InventoryItemsState;
		int num2 = 1;
		num2 = num2;
		return ((num2 & num) != 0) ? true : false;
	}

	public bool IsIdle()
	{
		int num = (int)InventoryItemsState;
		int num2 = 1;
		num2 <<= 2;
		return ((num2 & num) != 0) ? true : false;
	}

	public bool IsLackLV()
	{
		int num = (int)InventoryItemsState;
		int num2 = 1;
		num2 <<= 8;
		return ((num2 & num) != 0) ? true : false;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
