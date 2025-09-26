using UnityEngine;

public class UIIngame_Pause : UIMessageHandler
{
	[SerializeField]
	private TUIMeshSprite _bk;

	private Texture2D _iconTexs;

	private void Start()
	{
		if (_iconTexs == null)
		{
			_iconTexs = new Texture2D(1136, 768, TextureFormat.ARGB32, false);
			for (int i = 0; i < 1136; i++)
			{
				for (int j = 0; j < 768; j++)
				{
					_iconTexs.SetPixel(i, j, new Color(0f, 0f, 0f, 0.5f));
				}
			}
			_iconTexs.Apply(false);
		}
		_bk.UseCustomize = true;
		_bk.CustomizeTexture = _iconTexs;
		_bk.CustomizeRect = new Rect(0f, 0f, 1136f, 768f);
	}

	private new void Update()
	{
	}

	public void HandleEventButton_quit(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			OpenClikPlugin.Hide();
			COMA_Pref.Instance.Save(true);
			Debug.Log("Button_quit-CommandClick");
			if (COMA_CommonOperation.Instance.IsMode_Castle(Application.loadedLevelName))
			{
				COMA_CommonOperation.Instance.GamePause(false);
			}
			if (COMA_CommonOperation.Instance.IsPlayingMultiMode())
			{
				COMA_Pref.Instance.AddRankScoreOfCurrentScene(-20);
			}
			COMA_NetworkConnect.Instance.BackFromScene();
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_resume(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			OpenClikPlugin.Hide();
			COMA_Pref.Instance.Save(true);
			Debug.Log("Button_resume-CommandClick");
			base.gameObject.SetActive(false);
			if (COMA_CommonOperation.Instance.IsMode_Castle(Application.loadedLevelName))
			{
				COMA_CommonOperation.Instance.GamePause(false);
			}
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}
}
