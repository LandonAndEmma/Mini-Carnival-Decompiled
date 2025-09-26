using UnityEngine;

public class UI_BoxSlot : UICOM
{
	public enum ESlotState
	{
		Locked = 0,
		UnLocked = 1
	}

	protected int nID;

	protected ESlotState slotState = ESlotState.UnLocked;

	protected UI_BoxData boxData;

	public UI_BoxData BoxData
	{
		get
		{
			return boxData;
		}
		set
		{
			if (value == null && boxData != null)
			{
				boxData.Ower = null;
			}
			boxData = value;
			if (boxData != null)
			{
				boxData.Ower = this;
			}
			NotifyDataUpdate();
		}
	}

	public void SetID(int n)
	{
		nID = n;
	}

	public int GetID()
	{
		return nID;
	}

	public bool IsLocked()
	{
		return slotState == ESlotState.Locked;
	}

	public int SetSlot()
	{
		SetSlot(ESlotState.Locked);
		return 0;
	}

	public int SetSlot(ESlotState state)
	{
		slotState = state;
		NotifyDataUpdate();
		return 0;
	}

	private void Start()
	{
	}

	private new void Update()
	{
	}

	public virtual int NotifyDataUpdate()
	{
		if (BoxData == null)
		{
			ProcessNullData();
			return -1;
		}
		return 0;
	}

	protected virtual void ProcessNullData()
	{
	}

	protected virtual UIMessageHandler GetTUIMessageHandler(bool bHasAniLayer)
	{
		Transform parent = base.gameObject.transform.parent;
		if (parent != null)
		{
			Transform parent2 = parent.parent;
			if (parent2 != null)
			{
				Transform parent3 = parent2.parent;
				if (parent3 != null)
				{
					if (bHasAniLayer)
					{
						Transform parent4 = parent3.parent;
						if (parent4 != null)
						{
							Transform parent5 = parent4.parent;
							if (parent5 != null)
							{
								Transform parent6 = parent5.parent;
								if (parent6 != null)
								{
									return parent6.GetComponent<UIMessageHandler>();
								}
							}
						}
					}
					else
					{
						Transform parent7 = parent3.parent;
						if (parent7 != null)
						{
							Transform parent8 = parent7.parent;
							if (parent8 != null)
							{
								return parent8.GetComponent<UIMessageHandler>();
							}
						}
					}
				}
			}
		}
		return null;
	}

	protected virtual UIMessageHandler GetTUIMessageHandler()
	{
		return GetTUIMessageHandler(false);
	}
}
