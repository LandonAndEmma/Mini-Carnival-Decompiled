using MessageID;
using UnityEngine;

public class UIBackpackSellItems_ButtonSell : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIBackpack_SellItems, null, null);
	}
}
