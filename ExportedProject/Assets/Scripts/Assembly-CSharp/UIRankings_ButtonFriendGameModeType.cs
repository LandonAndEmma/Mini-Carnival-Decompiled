using MessageID;
using UnityEngine;

public class UIRankings_ButtonFriendGameModeType : MonoBehaviour
{
	[SerializeField]
	protected UIRankings.EGameModeType _curType;

	private bool _bShouldCheckOnClick;

	public UIRankings.EGameModeType CurType
	{
		get
		{
			return _curType;
		}
		set
		{
			_curType = value;
		}
	}

	private void Awake()
	{
		if (!(base.name == "00_Avatar") && !(base.name == "00_AAA_RPG"))
		{
			int curType = (int)CurType;
			if (COMA_Login.Instance != null && !COMA_Login.Instance.IsModeRankOn(curType))
			{
				Object.DestroyObject(base.gameObject);
				UIGrid component = base.transform.parent.GetComponent<UIGrid>();
				component.repositionNow = true;
			}
		}
	}

	private void OnEnable()
	{
		_bShouldCheckOnClick = true;
	}

	private void OnDisable()
	{
		_bShouldCheckOnClick = false;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (_bShouldCheckOnClick)
		{
			if (GetComponent<UICheckbox_Ranking>().startsChecked)
			{
				OnClick();
			}
			_bShouldCheckOnClick = false;
		}
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		if (base.name == "00_Avatar")
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRankings_FriendGameModeSelChanged, null, 901, base.gameObject);
		}
		else
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRankings_FriendGameModeSelChanged, null, CurType, base.gameObject);
		}
	}
}
