using MessageID;
using UnityEngine;

public class UISquare_ButtonGoAchievement : MonoBehaviour
{
	[SerializeField]
	private GameObject _newAchievementInfoObj;

	[SerializeField]
	private UILabel _newAchievementNumLabel;

	private void OnEnable()
	{
		int acceptableCount = COMA_Achievement.Instance.GetAcceptableCount();
		_newAchievementNumLabel.text = acceptableCount.ToString();
		_newAchievementInfoObj.SetActive((acceptableCount > 0) ? true : false);
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UISquare_GoAchievement, null, null);
	}
}
