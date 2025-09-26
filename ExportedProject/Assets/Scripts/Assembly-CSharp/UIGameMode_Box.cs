using NGUI_COMUI;
using UnityEngine;

public class UIGameMode_Box : NGUI_COMUI.UI_Box
{
	public UISprite spriteCom;

	public UISprite iconCom;

	public GameObject tag_hot;

	public GameObject tag_new;

	public UILabel label_onlineNumber;

	public override void FormatBoxName(int i)
	{
		if (i > 9)
		{
			base.gameObject.name = "UIGameModeBox" + i;
		}
		else
		{
			base.gameObject.name = "UIGameModeBox0" + i;
		}
	}

	public override void BoxDataChanged()
	{
		UIGameMode_BoxData uIGameMode_BoxData = base.BoxData as UIGameMode_BoxData;
		if (uIGameMode_BoxData == null)
		{
			SetLoseSelected();
			return;
		}
		GameObject obj = base.gameObject;
		obj.name = obj.name + "_" + uIGameMode_BoxData._gameModeID;
		spriteCom.spriteName = "map_" + uIGameMode_BoxData._gameModeID;
		iconCom.spriteName = "moshi_" + uIGameMode_BoxData._gameModeID;
		if (COMA_Login.Instance.IsModeNew(uIGameMode_BoxData._gameModeID))
		{
			tag_new.SetActive(true);
		}
		else if (COMA_Login.Instance.IsModeHot(uIGameMode_BoxData._gameModeID))
		{
			tag_hot.SetActive(true);
		}
		string key = uIGameMode_BoxData._gameModeID.ToString();
		int num = 0;
		if (UIDataBufferCenter.Instance._gameModeNum.ContainsKey(key))
		{
			num = (int)UIDataBufferCenter.Instance._gameModeNum[key];
		}
		if (label_onlineNumber != null)
		{
			int num2 = Random.Range(3, 5);
			int num3 = num * num2;
			if (num3 > 9999)
			{
				label_onlineNumber.text = "9999+";
			}
			else
			{
				label_onlineNumber.text = num3.ToString();
			}
		}
	}
}
