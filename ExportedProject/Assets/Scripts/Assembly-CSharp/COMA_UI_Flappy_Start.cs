using UnityEngine;

public class COMA_UI_Flappy_Start : UIMessageHandler
{
	private void Start()
	{
	}

	private new void Update()
	{
	}

	public void HandleEventButton_ClickToStart(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("Button_Start-CommandClick");
			COMA_Flappy_SceneController.Instance.StartIndeed();
		}
	}
}
