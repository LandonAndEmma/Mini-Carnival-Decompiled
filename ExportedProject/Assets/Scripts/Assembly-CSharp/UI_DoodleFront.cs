using UnityEngine;

public class UI_DoodleFront : UIMessageHandler
{
	[SerializeField]
	private UIDoodle1 _msgProce;

	private void Start()
	{
	}

	private new void Update()
	{
	}

	public void HandleEventButton_front(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			_msgProce.NotifyFrontState();
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		case 1:
			BtnScale(control);
			break;
		case 2:
			BtnRestoreScale(control);
			break;
		}
	}
}
