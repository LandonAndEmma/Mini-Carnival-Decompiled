using UnityEngine;

public class UI_ACPopupBox : MonoBehaviour
{
	[SerializeField]
	private TUIMeshSprite _acIcon;

	[SerializeField]
	private TUILabel _acCaption;

	private void Awake()
	{
		TUITextManager.Instance().Parser("UI/language.en", "UI/language.en");
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Popup(int nId)
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_AchievementPop);
		base.gameObject.SetActive(true);
		if ((bool)base.animation)
		{
			base.animation.Play();
		}
		_acIcon.texture = UI_GlobalData._strACIcons[nId];
		_acCaption.Text = TUITextManager.Instance().GetString(UI_GlobalData._strACCaption[nId - 1]);
		SceneTimerInstance.Instance.Add(2.5f, ExitBox);
	}

	private bool ExitBox()
	{
		base.animation.Play("UI_ACPopUpBox_Exit");
		return false;
	}

	public void Hide()
	{
		Object.Destroy(base.gameObject);
	}
}
