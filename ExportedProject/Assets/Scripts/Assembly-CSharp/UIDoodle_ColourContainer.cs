using UnityEngine;

public class UIDoodle_ColourContainer : UI_Container
{
	public GameObject indicate;

	public UIDoodle_ColourVolume volumeIndicate;

	private UIDoodle_ColourBoxSlot curSelSlot;

	public UIDoodle_ColourBoxSlot CurSelSlot
	{
		get
		{
			return curSelSlot;
		}
	}

	public void RefreshCurSelSlot(UIDoodle_ColourBoxSlot slot, TUIControl control)
	{
		curSelSlot = slot;
		if (CurSelSlot != null && control != null)
		{
			indicate.active = true;
			Vector3 position = indicate.transform.position;
			position.x = control.gameObject.transform.position.x;
			indicate.transform.position = position;
			volumeIndicate.objVolumeIndicate.GetComponent<TUIMeshSprite>().color = ((TUIButtonClick)control).m_NormalObj.GetComponent<TUIMeshSprite>().color;
			volumeIndicate.objVolumeIndicate.active = true;
			volumeIndicate.objVolumeLabelIndicate.SetActive(true);
			volumeIndicate.RefreshVolume(CurSelSlot.Volume);
		}
		else if (CurSelSlot == null)
		{
			indicate.active = false;
			volumeIndicate.objVolumeIndicate.active = false;
			volumeIndicate.objVolumeLabelIndicate.SetActive(false);
		}
	}

	private void Awake()
	{
		PreInit();
		objBoxPerfab = Resources.Load("UI/Doodle/ColourBox") as GameObject;
	}

	private void Start()
	{
		SimulateCall_Init();
	}

	private new void Update()
	{
	}

	public override int ExtraInit(int nSlotCount)
	{
		int num = 0;
		foreach (UI_Box lstBox in lstBoxs)
		{
			UIDoodle_ColourBox uIDoodle_ColourBox = (UIDoodle_ColourBox)lstBox;
			UI_BoxSlot[] slots = uIDoodle_ColourBox.Slots;
			for (int i = 0; i < slots.Length; i++)
			{
				UIDoodle_ColourBoxSlot uIDoodle_ColourBoxSlot = (UIDoodle_ColourBoxSlot)slots[i];
				uIDoodle_ColourBoxSlot.SetBoxColorAndVolume(COMA_Color.colors[num++], 1f);
			}
		}
		return 0;
	}

	private void SimulateCall_Init()
	{
		CreateBox(COMA_Color.colors.Length, 0);
		RefreshContainer();
	}
}
