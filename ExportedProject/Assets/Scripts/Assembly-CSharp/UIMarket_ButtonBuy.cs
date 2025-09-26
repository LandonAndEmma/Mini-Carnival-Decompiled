using MessageID;
using UnityEngine;

public class UIMarket_ButtonBuy : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UIMarket_GotoShoppingCart, null, null);
	}
}
