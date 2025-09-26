using MC_UIToolKit;
using UnityEngine;

public class UISquare_ButtonChangeSquare : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, Localization.instance.Get("guangchang_desc1"));
		uIMessage_CommonBoxData.Mark = "ChangeSquare";
		UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Refresh);
	}
}
