using NGUI_COMUI;
using UnityEngine;

public class UIBackpack_SellListPreviewBox : NGUI_COMUI.UI_Box
{
	[SerializeField]
	private UILabel _dbgLabelType;

	[SerializeField]
	private UITexture _uiTex;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public override void FormatBoxName(int i)
	{
		if (i > 9)
		{
			base.gameObject.name = "UISellListPreviewBox" + i;
		}
		else
		{
			base.gameObject.name = "UISellListPreviewBox0" + i;
		}
	}

	public override void BoxDataChanged()
	{
		UIBackpack_SellListPreviewBoxData uIBackpack_SellListPreviewBoxData = base.BoxData as UIBackpack_SellListPreviewBoxData;
		if (uIBackpack_SellListPreviewBoxData == null)
		{
			SetLoseSelected();
			return;
		}
		_dbgLabelType.enabled = false;
		_uiTex.enabled = true;
		_uiTex.mainTexture = uIBackpack_SellListPreviewBoxData.Tex;
	}
}
