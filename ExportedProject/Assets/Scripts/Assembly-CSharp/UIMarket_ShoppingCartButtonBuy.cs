using MessageID;
using UnityEngine;

public class UIMarket_ShoppingCartButtonBuy : MonoBehaviour
{
	[SerializeField]
	private UICheckbox checkBox;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_PurchaseShopItems, null, checkBox.isChecked);
	}
}
