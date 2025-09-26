using UnityEngine;

public class UITheme : UIMessageHandler
{
	[SerializeField]
	private GameObject _theme;

	[SerializeField]
	private GameObject _connectInter;

	[SerializeField]
	private GameObject _touchToPlay;

	private void Awake()
	{
		TUITextManager.Instance().Parser("UI/language.en", "UI/language.en");
	}

	private void Start()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.Game_Cheer, null, 9f);
	}

	private new void Update()
	{
	}

	public void ProceMove()
	{
		_connectInter.SetActive(false);
		_touchToPlay.SetActive(true);
	}

	public void HandleEventButton_TouchToPlay(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			_touchToPlay.SetActive(false);
			_theme.GetComponent<UIGameThemeIconMgr>().Exit();
			COMA_Carnival_Camera.Instance.RealCameraMove();
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.Game_Camera, null, 4.5f);
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}
}
