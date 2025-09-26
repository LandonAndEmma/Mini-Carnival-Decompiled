using UnityEngine;

public class COMA_UI_Flappy_End : UICOM
{
	[SerializeField]
	private TUILabel curScore;

	[SerializeField]
	private TUILabel weekScore;

	[SerializeField]
	private TUILabel bestScore;

	[SerializeField]
	private TUILabel goldNum;

	private new void OnEnable()
	{
		if (COMA_Pref.Instance.mode_flappy_weekScore < COMA_PlayerSelf.Instance.score)
		{
			COMA_Pref.Instance.mode_flappy_weekScore = COMA_PlayerSelf.Instance.score;
		}
		if (COMA_Pref.Instance.mode_flappy_bestScore < COMA_Pref.Instance.mode_flappy_weekScore)
		{
			COMA_Pref.Instance.mode_flappy_bestScore = COMA_Pref.Instance.mode_flappy_weekScore;
		}
		curScore.Text = COMA_PlayerSelf.Instance.score.ToString();
		weekScore.Text = COMA_Pref.Instance.mode_flappy_weekScore.ToString();
		bestScore.Text = COMA_Pref.Instance.mode_flappy_bestScore.ToString();
		goldNum.Text = curScore.Text;
		COMA_Pref.Instance.AddGold(int.Parse(goldNum.Text));
		COMA_Pref.Instance.Save(true);
		COMA_Pref.Instance.AddRankScoreOfCurrentScene(COMA_PlayerSelf.Instance.score);
	}

	private void Start()
	{
	}

	private new void Update()
	{
	}

	public void HandleEventButton_Again(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_Again-CommandClick");
			COMA_Flappy_SceneController.Instance.ReadyToStart();
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

	public void HandleEventButton_Quit(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_Quit-CommandClick");
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
}
