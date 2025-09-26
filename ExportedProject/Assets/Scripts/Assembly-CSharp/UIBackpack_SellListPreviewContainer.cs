using NGUI_COMUI;

public class UIBackpack_SellListPreviewContainer : NGUI_COMUI.UI_Container
{
	protected override void Load()
	{
		base.Load();
	}

	protected override void UnLoad()
	{
		base.UnLoad();
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}

	protected override bool IsCanSelBox(NGUI_COMUI.UI_Box box, out NGUI_COMUI.UI_Box loseSel)
	{
		loseSel = null;
		return false;
	}
}
