using UnityEngine;

public class UI_DoodleLock : UIMessageHandler
{
	private enum ELockState
	{
		locked = 0,
		unlocked = 1
	}

	private ELockState _curState = ELockState.unlocked;

	[SerializeField]
	private GameObject[] _icons;

	[SerializeField]
	private UIDoodle1 _msgProce;

	public bool IsLocked()
	{
		return _curState == ELockState.locked;
	}

	private void Awake()
	{
		RefreshLockIconState();
	}

	private void Start()
	{
	}

	private new void Update()
	{
	}

	private void RefreshLockIconState()
	{
		if (_curState == ELockState.locked)
		{
			_icons[0].SetActive(true);
			_icons[1].SetActive(false);
		}
		else
		{
			_icons[0].SetActive(false);
			_icons[1].SetActive(true);
		}
	}

	public void HandleEventButton_lock(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			_curState = ((_curState == ELockState.locked) ? ELockState.unlocked : ELockState.locked);
			RefreshLockIconState();
			_msgProce.NotifyLockStateChanged();
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		case 1:
			BtnScale(control);
			break;
		case 2:
			BtnRestoreScale(control);
			break;
		}
	}
}
