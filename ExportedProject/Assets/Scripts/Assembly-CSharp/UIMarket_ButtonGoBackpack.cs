using MessageID;
using UnityEngine;

public class UIMarket_ButtonGoBackpack : MonoBehaviour
{
	private void OnClick()
	{
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UIMarket_GotoBackpackClick, null, null);
	}
}
