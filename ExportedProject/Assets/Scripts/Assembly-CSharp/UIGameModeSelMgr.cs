using NGUI_COMUI;
using UnityEngine;

public class UIGameModeSelMgr : MonoBehaviour
{
	[SerializeField]
	private UIGameMode_Container _container;

	private void Start()
	{
		int num = COMA_Login.Instance.orders.Length;
		_container.InitContainer(NGUI_COMUI.UI_Container.EBoxSelType.Single);
		_container.InitBoxs(num, true);
		for (int i = 0; i < num; i++)
		{
			UIGameMode_BoxData uIGameMode_BoxData = new UIGameMode_BoxData();
			uIGameMode_BoxData._gameModeID = COMA_Login.Instance.orders[i];
			_container.SetBoxData(i, uIGameMode_BoxData);
		}
		UIDataBufferCenter.Instance.GetGameModeNum();
	}

	private void Update()
	{
	}
}
