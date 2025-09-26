using MessageID;
using UnityEngine;

public class UI_NGRPG_CardMgr : UIEntity
{
	[SerializeField]
	private UISprite _sprite_light1;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_RPG_CardMgr_Click1, this, NGCardMgrClick1);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_RPG_CardMgr_Click1, this);
	}

	private bool NGCardMgrClick1(TUITelegram msg)
	{
		_sprite_light1.enabled = false;
		return true;
	}

	private void Awake()
	{
	}

	private void Start()
	{
		_sprite_light1.enabled = false;
		if (COMA_Pref.Instance.NG2_1_FirstEnterSquare)
		{
			if (UIDataBufferCenter.Instance.CurNGIndex == 2)
			{
				_sprite_light1.enabled = true;
			}
			if (UIDataBufferCenter.Instance.CurNGIndex != 3)
			{
			}
		}
	}

	protected override void Tick()
	{
	}
}
