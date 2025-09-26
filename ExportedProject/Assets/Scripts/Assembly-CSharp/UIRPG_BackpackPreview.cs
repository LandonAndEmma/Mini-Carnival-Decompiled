using MessageID;
using UnityEngine;

public class UIRPG_BackpackPreview : UIEntity
{
	[SerializeField]
	private Renderer[] _partRenderers;

	private int _index;

	[SerializeField]
	private Camera _cmr;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UI_Notify2DCharc, this, HandleNotify2DCharc);
		RegisterMessage(EUIMessageID.UIRPG_Backpack_PartPreview, this, HandleBackpackPartPreview);
		RegisterMessage(EUIMessageID.UIRPG_BackPack_PartDelOrDecompose, this, HandleBackPackPartDelOrDecompose);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UI_Notify2DCharc, this);
		UnregisterMessage(EUIMessageID.UIRPG_Backpack_PartPreview, this);
		UnregisterMessage(EUIMessageID.UIRPG_BackPack_PartDelOrDecompose, this);
	}

	public bool HandleNotify2DCharc(TUITelegram msg)
	{
		if ((int)msg._pExtraInfo == 6)
		{
			float y = (float)msg._pExtraInfo2 * -314f * 2f / (float)Screen.width;
			Quaternion quaternion = Quaternion.Euler(0f, y, 0f);
			_partRenderers[_index].transform.parent.transform.rotation *= quaternion;
		}
		return true;
	}

	public bool HandleBackpackPartPreview(TUITelegram msg)
	{
		Debug.Log("public bool HandleBackpackPartPreview(TUITelegram msg) public bool HandleBackpackPartPreview(TUITelegram msg)");
		for (int i = 0; i < _partRenderers.Length; i++)
		{
			_partRenderers[i].enabled = false;
			_partRenderers[i].gameObject.transform.parent.transform.localPosition = new Vector3(0f, 0f, 1f);
			_partRenderers[i].gameObject.transform.parent.transform.localRotation = Quaternion.identity;
			_partRenderers[i].gameObject.transform.parent.transform.localScale = Vector3.one;
		}
		UIRPG_BackPack_Avatar_BoxData uIRPG_BackPack_Avatar_BoxData = msg._pExtraInfo as UIRPG_BackPack_Avatar_BoxData;
		if (uIRPG_BackPack_Avatar_BoxData != null)
		{
			float orthographicSize = 1f;
			switch (uIRPG_BackPack_Avatar_BoxData.EquipAvatar.m_part)
			{
			case 1:
				_index = 0;
				orthographicSize = 0.7f;
				break;
			case 2:
				_index = 1;
				orthographicSize = 0.7f;
				break;
			case 3:
				_index = 2;
				orthographicSize = 0.5f;
				break;
			}
			_partRenderers[_index].enabled = true;
			_cmr.orthographicSize = orthographicSize;
			UIDataBufferCenter.Instance.FetchTexture2DByMD5(uIRPG_BackPack_Avatar_BoxData.Unit, delegate(Texture2D tex)
			{
				_partRenderers[_index].material.mainTexture = tex;
			});
		}
		return true;
	}

	public bool HandleBackPackPartDelOrDecompose(TUITelegram msg)
	{
		_partRenderers[_index].enabled = false;
		return true;
	}
}
