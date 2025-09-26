using System.Collections.Generic;
using UnityEngine;

public class UI_Container : UICOM
{
	protected TUIScrollList scrollListCmp;

	protected TUIClipBinder clipBinderCmp;

	protected GameObject objBoxPerfab;

	protected List<UI_Box> lstBoxs = new List<UI_Box>();

	protected List<UI_BoxData> lstBoxDatas = new List<UI_BoxData>();

	public List<UI_Box> LstBoxs
	{
		get
		{
			return lstBoxs;
		}
	}

	public List<UI_BoxData> LstBoxDatas
	{
		get
		{
			return lstBoxDatas;
		}
	}

	protected void PreInit()
	{
		scrollListCmp = base.gameObject.GetComponent<TUIScrollList>();
		clipBinderCmp = base.gameObject.GetComponent<TUIClipBinder>();
	}

	protected void ClearBoxListData()
	{
		foreach (UI_Box lstBox in lstBoxs)
		{
			Object.Destroy(lstBox.gameObject);
		}
		lstBoxs.Clear();
	}

	protected void ClearBoxDataListData()
	{
		lstBoxDatas.Clear();
	}

	protected void ClearListData()
	{
		ClearBoxListData();
		ClearBoxDataListData();
	}

	protected virtual int ConnectData()
	{
		return ConnectData(true);
	}

	protected virtual int ConnectData(bool bIgnoreLock)
	{
		int num = 0;
		foreach (UI_Box lstBox in lstBoxs)
		{
			int num2 = lstBox.Slots.Length;
			for (int i = 0; i < num2; i++)
			{
				if (num >= lstBoxDatas.Count)
				{
					Debug.LogWarning("BoxData End of the early!");
					return 1;
				}
				lstBox.Slots[i].BoxData = lstBoxDatas[num++];
			}
		}
		return num - 1;
	}

	protected int AddBoxToContainer()
	{
		int count = lstBoxs.Count;
		scrollListCmp.Clear(true);
		for (int i = 0; i < count; i++)
		{
			TUIControl component = lstBoxs[i].GetComponent<TUIControl>();
			if (component == null)
			{
				Debug.LogError("Lack of TUIControl component!");
			}
			scrollListCmp.Add(component);
		}
		scrollListCmp.ScrollListTo(0f);
		clipBinderCmp.SetClipRect();
		return count;
	}

	public virtual int CreateBox(int nCount, int nType)
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
			for (int j = 0; j < component.Slots.Length; j++)
			{
				UI_BoxSlot uI_BoxSlot = component.Slots[j];
				uI_BoxSlot.SetID(num++);
				lstBoxDatas.Add(null);
			}
			lstBoxs.Add(component);
			nSlotCount = component.Slots.Length;
		}
		return ExtraInit(nSlotCount);
	}

	public virtual int ExtraInit(int nSlotCount)
	{
		return lstBoxs.Count * nSlotCount;
	}

	public virtual int AddBoxData(int index, UI_BoxData boxData)
	{
		if (index < 0 || index >= lstBoxDatas.Count)
		{
			Debug.Log("lstBoxDatas : Out of range!");
			return -1;
		}
		lstBoxDatas[index] = boxData;
		return lstBoxDatas.Count;
	}

	public virtual int RefreshContainer()
	{
		int num = ConnectData();
		if (num < 0)
		{
			Debug.LogWarning("Connect Data Exception: " + num + " !");
			return -1;
		}
		AddBoxToContainer();
		return 0;
	}

	public virtual int ExitContainer()
	{
		ClearListData();
		scrollListCmp.EndRefresh();
		scrollListCmp.Clear(true);
		scrollListCmp.ScrollListTo(0f);
		clipBinderCmp.SetClipRect();
		return 0;
	}
}
