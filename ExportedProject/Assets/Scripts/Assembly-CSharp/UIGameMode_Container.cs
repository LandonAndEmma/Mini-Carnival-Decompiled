using MC_UIToolKit;
using NGUI_COMUI;
using UnityEngine;

public class UIGameMode_Container : NGUI_COMUI.UI_Container
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
				return true;
			}
			loseSel = null;
			return false;
		}
		loseSel = null;
		return false;
	}

	protected override void ProcessBoxSelected(NGUI_COMUI.UI_Box box)
	{
		if (!COMA_CommonOperation.Instance.bSelectModeLock)
		{
			COMA_CommonOperation.Instance.bSelectModeLock = true;
			base.ProcessBoxSelectedGameMode(box);
			string[] array = box.name.Split('_');
			COMA_CommonOperation.Instance.seletectGameModeIndex = int.Parse(array[1]);
			Debug.Log("gameModeIndex:" + COMA_CommonOperation.Instance.seletectGameModeIndex);
			if (COMA_CommonOperation.Instance.seletectGameModeIndex == 10)
			{
				COMA_NetworkConnect.Instance.TryToEnterRoom(COMA_CommonOperation.Instance.seletectGameModeIndex.ToString());
				return;
			}
			UIGolbalStaticFun.PopBlockForTUIMessageBox();
			Application.LoadLevelAdditive("UI.GameModeInfo");
		}
	}

	protected override void ProcessBoxLoseSelected(NGUI_COMUI.UI_Box box)
	{
		base.ProcessBoxLoseSelected(box);
	}

	protected override void ProcessBoxCanntSelected(NGUI_COMUI.UI_Box box)
	{
	}
}
