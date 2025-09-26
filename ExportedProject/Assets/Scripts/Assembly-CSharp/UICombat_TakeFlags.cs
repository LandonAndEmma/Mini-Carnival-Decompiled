using UnityEngine;

public class UICombat_TakeFlags : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void HandleEventButton_pause(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("Button_pause-CommandClick");
		}
	}
}
