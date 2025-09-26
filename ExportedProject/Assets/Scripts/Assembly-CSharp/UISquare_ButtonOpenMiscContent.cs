using MessageID;
using UnityEngine;

public class UISquare_ButtonOpenMiscContent : MonoBehaviour
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

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UISquare_OpenMiscContentButtonOnClick, null, null);
	}
}
