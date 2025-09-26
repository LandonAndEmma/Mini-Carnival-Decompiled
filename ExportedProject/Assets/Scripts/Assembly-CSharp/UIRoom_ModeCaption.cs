using UnityEngine;

public class UIRoom_ModeCaption : MonoBehaviour
{
	public TUILabel modeCaption;

	private void Start()
	{
		SetRoomCaption(UI_GameMode.Instance.GameMode);
	}

	private void Update()
	{
	}

	public void SetRoomCaption(string str)
	{
		if (modeCaption != null)
		{
			modeCaption.Text = str;
		}
	}
}
