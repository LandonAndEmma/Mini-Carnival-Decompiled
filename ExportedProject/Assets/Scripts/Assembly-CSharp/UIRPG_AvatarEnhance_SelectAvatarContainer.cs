using NGUI_COMUI;
using UnityEngine;

public class UIRPG_AvatarEnhance_SelectAvatarContainer : NGUI_COMUI.UI_Container
{
	[SerializeField]
	private GameObject _selObj;

	protected override void Tick()
	{
	}

	protected override bool IsCanSelBox(NGUI_COMUI.UI_Box box, out NGUI_COMUI.UI_Box loseSel)
	{
		if (base.BoxSelType == EBoxSelType.Single)
		{
			if (box.BoxData != null)
			{
				if (box != _curSelBox)
				{
					loseSel = _curSelBox;
					return true;
				}
				loseSel = null;
				return false;
			}
			loseSel = null;
			return false;
		}
		loseSel = null;
		return false;
	}

	protected override void ProcessBoxSelected(NGUI_COMUI.UI_Box box)
	{
		box.SetSelected();
		_selObj.SetActive(true);
	}
}
