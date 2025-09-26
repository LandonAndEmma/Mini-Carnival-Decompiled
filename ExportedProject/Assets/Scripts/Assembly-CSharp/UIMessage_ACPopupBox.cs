using UnityEngine;

public class UIMessage_ACPopupBox : UIMessage_Box
{
	[SerializeField]
	private UILabel _labelDes;

	[SerializeField]
	private UISprite _spriteACIcon;

	private void Start()
	{
		UIACPopupBoxData uIACPopupBoxData = base.BoxData as UIACPopupBoxData;
		if (uIACPopupBoxData != null)
		{
			Popup(uIACPopupBoxData.ACId);
		}
	}

	private new void OnDestroy()
	{
		SceneTimerInstance.Instance.Remove(ExitBox);
	}

	protected void OnDisable()
	{
		CloseBlockBox(null);
	}

	protected void Update()
	{
	}

	public override void FormatBoxName(int i)
	{
	}

	public override void BoxDataChanged()
	{
		UIACPopupBoxData uIACPopupBoxData = base.BoxData as UIACPopupBoxData;
		if (uIACPopupBoxData != null)
		{
		}
	}

	private bool CloseBlockBox(TUITelegram msg)
	{
		Object.Destroy(base.gameObject);
		return true;
	}

	private void Popup(int nId)
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_AchievementPop);
		base.gameObject.SetActive(true);
		if ((bool)base.animation)
		{
			base.animation.Play();
		}
		_spriteACIcon.spriteName = UI_GlobalData._strACIcons[nId];
		_labelDes.text = TUITextManager.Instance().GetString(UI_GlobalData._strACCaption[nId - 1]);
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
