using MessageID;
using UnityEngine;

public class UISquare_ButtonCloseMiscContent : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UISquare_CloseMiscContentButtonOnClick, null, null);
	}
}
