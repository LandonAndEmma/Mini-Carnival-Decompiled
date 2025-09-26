using MessageID;
using UnityEngine;

public class UIRPG_MapNewPlayerController : UIEntity
{
	[SerializeField]
	private GameObject _firstPointObj;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_RPG_BtnDown, this, HandleUING_RPG_BtnDown);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_RPG_BtnDown, this);
	}

	public void Start()
	{
		if (COMA_Pref.Instance.NG2_1_FirstEnterMap)
		{
			_firstPointObj.SetActive(true);
		}
	}

	public bool HandleUING_RPG_BtnDown(TUITelegram msg)
	{
		int num = (int)msg._pExtraInfo;
		if (num == 1000)
		{
			_firstPointObj.SetActive(false);
		}
		return true;
	}
}
