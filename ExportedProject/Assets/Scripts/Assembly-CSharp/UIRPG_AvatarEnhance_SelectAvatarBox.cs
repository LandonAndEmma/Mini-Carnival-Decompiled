using NGUI_COMUI;
using UnityEngine;

public class UIRPG_AvatarEnhance_SelectAvatarBox : NGUI_COMUI.UI_Box
{
	[SerializeField]
	private GameObject _preSelObj;

	public override void FormatBoxName(int i)
	{
		if (i > 9)
		{
			base.gameObject.name = "UIRPG_Avatar" + i;
		}
		else
		{
			base.gameObject.name = "UIRPG_Avatar0" + i;
		}
	}

	public override void BoxDataChanged()
	{
		UIRPG_AvatarEnhance_SelectAvatarBoxData uIRPG_AvatarEnhance_SelectAvatarBoxData = base.BoxData as UIRPG_AvatarEnhance_SelectAvatarBoxData;
		if (uIRPG_AvatarEnhance_SelectAvatarBoxData != null)
		{
			_mainTex.mainTexture = uIRPG_AvatarEnhance_SelectAvatarBoxData.Tex;
			if (_mainTex.mainTexture != null)
			{
				_mainTex.enabled = true;
			}
			else
			{
				_mainTex.enabled = false;
			}
			_preSelObj.SetActive(uIRPG_AvatarEnhance_SelectAvatarBoxData.IsSel ? true : false);
		}
	}

	public override void SetSelected()
	{
		base.SetSelected();
	}

	public override void SetLoseSelected()
	{
		base.SetLoseSelected();
	}
}
