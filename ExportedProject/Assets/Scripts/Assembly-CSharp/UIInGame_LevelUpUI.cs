using UnityEngine;

public class UIInGame_LevelUpUI : MonoBehaviour
{
	[SerializeField]
	private TUILabel _level;

	public int LevelNum
	{
		set
		{
			if (_level != null)
			{
				_level.Text = value.ToString();
			}
		}
	}

	private void Start()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.Game_LevelUp);
	}

	private void Update()
	{
	}

	public void HandleEventButtonOk(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			COMA_Pref.Instance.AddGold(500);
			COMA_Pref.Instance.Save(true);
			COMA_NetworkConnect.Instance.BackFromScene();
			base.gameObject.SetActive(false);
		}
	}
}
