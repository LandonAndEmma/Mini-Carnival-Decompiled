using UnityEngine;

public class UI_OtherPlayerFishingInfo : MonoBehaviour
{
	[SerializeField]
	private TUILabel _acCaption;

	[SerializeField]
	private Animation _aniCmp;

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

	public void Popup(string str)
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_AchievementPop);
		base.gameObject.SetActive(true);
		if ((bool)_aniCmp)
		{
			_aniCmp.Play();
		}
		_acCaption.Text = str;
		SceneTimerInstance.Instance.Add(2.5f, ExitBox);
	}

	private bool ExitBox()
	{
		_aniCmp.Play("UI_ACPopUpBox_Exit_TUI");
		return false;
	}

	public void Hide()
	{
		int iDByName = TFishingAddressBook.Instance.GetIDByName(1);
		TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 1005, TTelegram.SEND_MSG_IMMEDIATELY, null);
		Object.Destroy(base.gameObject);
	}
}
