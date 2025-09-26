using NGUI_COMUI;
using Protocol;

public class UIRPG_AvatarEnhance_SelectAvatarBoxData : NGUI_COMUI.UI_BoxData
{
	private bool _isSel;

	private BagItem _bagItem;

	public bool IsSel
	{
		get
		{
			return _isSel;
		}
		set
		{
			_isSel = value;
		}
	}

	public BagItem BagItemData
	{
		get
		{
			return _bagItem;
		}
		set
		{
			_bagItem = value;
		}
	}

	public UIRPG_AvatarEnhance_SelectAvatarBoxData(ulong itemId, string unit)
	{
		base.ItemId = itemId;
		base.Unit = unit;
	}
}
