using UnityEngine;

public class UITrade_SellContainer : UI_Container
{
	private void Awake()
	{
		PreInit();
		objBoxPerfab = Resources.Load("UI/Trade/AvatarShopSellBox") as GameObject;
	}

	private void Start()
	{
	}

	private new void Update()
	{
	}

	public override int ExtraInit(int nSlotCount)
	{
		int num = 0;
		int num2 = 0;
		foreach (UI_Box lstBox in lstBoxs)
		{
			num2++;
			int num3 = 0;
			UITrade_SellBox uITrade_SellBox = (UITrade_SellBox)lstBox;
			UI_BoxSlot[] slots = uITrade_SellBox.Slots;
			foreach (UI_BoxSlot uI_BoxSlot in slots)
			{
				num3++;
				Random.seed = ((int)Time.timeSinceLevelLoad + num3 * num2) * 10245;
				UITrade_SellBoxSlot uITrade_SellBoxSlot = (UITrade_SellBoxSlot)uI_BoxSlot;
				UI_BoxSlot.ESlotState eSlotState = UI_BoxSlot.ESlotState.UnLocked;
				uITrade_SellBoxSlot.SetSlot(eSlotState);
				if (eSlotState == UI_BoxSlot.ESlotState.UnLocked)
				{
					num++;
				}
				else
				{
					Debug.Log("Locked!");
				}
			}
		}
		return num;
	}
}
