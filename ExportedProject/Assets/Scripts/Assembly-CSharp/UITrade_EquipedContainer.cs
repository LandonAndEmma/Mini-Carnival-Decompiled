using UnityEngine;

public class UITrade_EquipedContainer : UI_Container
{
	[SerializeField]
	private UI_PageIndicateMgr indicateMgr;

	protected TUIPageFrameEx pageFrameCmp;

	private int nSimulateBoxCount = 5;

	private Vector3 page0Pos = new Vector3(-1.945877f, 21.23535f, 0f);

	public void ProcePageChanged(int nCurPage)
	{
		indicateMgr.SetPage(nCurPage + 1);
	}

	private void Awake()
	{
		PreInit();
		pageFrameCmp = base.gameObject.GetComponent<TUIPageFrameEx>();
		objBoxPerfab = Resources.Load("UI/Trade/EquipedBox") as GameObject;
		pageFrameCmp.ProcePageChanged += ProcePageChanged;
	}

	private void Start()
	{
	}

	private new void Update()
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
				UITrade_EquipedBoxData boxData = new UITrade_EquipedBoxData();
				AddBoxData(uI_BoxSlot.GetID(), boxData);
			}
		}
		RefreshContainer();
	}

	public override int ExtraInit(int nSlotCount)
	{
		int count = lstBoxs.Count;
		indicateMgr.InitIndicates(count);
		return 0;
	}

	public override int RefreshContainer()
	{
		int num = ConnectData();
		if (num < 0)
		{
			Debug.LogWarning("Connect Data Exception: " + num + " !");
		}
		int count = lstBoxs.Count;
		pageFrameCmp.Clear(true);
		Vector3 position = page0Pos;
		for (int i = 0; i < count; i++)
		{
			TUIPageEx component = lstBoxs[i].GetComponent<TUIPageEx>();
			if (component == null)
			{
				Debug.LogError("Lack of TUIPageEx component!");
			}
			float x = component.size.x;
			position.x += (float)i * x;
			lstBoxs[i].gameObject.transform.position = position;
			pageFrameCmp.AddPage(component);
			position = page0Pos;
		}
		clipBinderCmp.SetClipRect();
		return 0;
	}

	public override int ExitContainer()
	{
		ClearListData();
		pageFrameCmp.Clear(true);
		clipBinderCmp.SetClipRect();
		indicateMgr.ClearIndicate();
		return 0;
	}
}
