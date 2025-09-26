using MessageID;
using UnityEngine;

public class UIRPG_ButtonCloseCardLibrary : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UICardLibrary_CloseClick, null, null);
	}
}
