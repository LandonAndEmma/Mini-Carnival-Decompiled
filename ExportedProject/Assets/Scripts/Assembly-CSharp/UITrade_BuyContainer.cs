using UnityEngine;

public class UITrade_BuyContainer : UI_Container
{
	private void Awake()
	{
		PreInit();
		objBoxPerfab = Resources.Load("UI/Trade/AvatarShopBuyBox") as GameObject;
	}

	private void Start()
	{
	}

	private new void Update()
	{
	}

	public override int CreateBox(int nCount, int nType)
	{
		if (objBoxPerfab == null)
		{
			Debug.LogError("BoxPerfab Not Assigned!");
			return -1;
		}
		if (lstBoxs.Count != 0)
		{
			ClearBoxListData();
			Debug.LogWarning("lstBoxs IS not Empty!");
		}
		int nSlotCount = 0;
		int num = 0;
		for (int i = 0; i < nCount; i++)
		{
			GameObject gameObject = Object.Instantiate(objBoxPerfab, new Vector3(-1000f, 0f, 0f), Quaternion.identity) as GameObject;
			UI_Box component = gameObject.GetComponent<UI_Box>();
			if (component == null)
			{
				Debug.LogError("UI_Box Component Is Not Exist!");
				return -1;
			}
			if (i == 0)
			{
				UI_BoxSlot[] slots = component.Slots;
				for (int j = 0; j < slots.Length; j++)
				{
					UITrade_BuyBoxSlot uITrade_BuyBoxSlot = (UITrade_BuyBoxSlot)slots[j];
					uITrade_BuyBoxSlot.SlotType = UITrade_BuyBoxSlot.ESlotType.Official;
				}
				Debug.Log("Official:" + i);
			}
			nSlotCount = component.Slots.Length;
			for (int k = 0; k < component.Slots.Length; k++)
			{
				UI_BoxSlot uI_BoxSlot = component.Slots[k];
				uI_BoxSlot.SetID(num++);
				lstBoxDatas.Add(null);
			}
			lstBoxs.Add(component);
		}
		return ExtraInit(nSlotCount);
	}
}
