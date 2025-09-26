using MC_UIToolKit;
using UnityEngine;

public class UISquare_ButtonRate : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(2, Localization.instance.Get("guangchang_desc2"));
		uIMessage_CommonBoxData.Mark = "RateApp";
		UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
	}
}
