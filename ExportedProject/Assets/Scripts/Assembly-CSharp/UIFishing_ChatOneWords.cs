using UnityEngine;

public class UIFishing_ChatOneWords : MonoBehaviour
{
	[SerializeField]
	private TUILabel _name;

	[SerializeField]
	private TUILabel _words;

	[SerializeField]
	private TUILabel _btnLabel;

	[SerializeField]
	private GameObject _objShieldBtn;

	[SerializeField]
	private GameObject _objBlockIcon;

	[SerializeField]
	private GameObject _objUnBlockIcon;

	public string _strOwerId = string.Empty;

	private bool _InShiledList;

	public string Name
	{
		set
		{
			_name.Text = value + ":";
		}
	}

	public string Words
	{
		get
		{
			return _words.Text;
		}
		set
		{
			if (COMA_PlayerSelf.Instance != null && ((COMA_PlayerSelf_Fishing)COMA_PlayerSelf.Instance).IsInShieldingList(_strOwerId))
			{
				_words.Text = string.Empty;
			}
			else
			{
				_words.Text = value;
			}
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (!(COMA_PlayerSelf.Instance != null))
		{
			return;
		}
		string text = COMA_PlayerSelf.Instance.id.ToString();
		if (_strOwerId == text)
		{
			_objShieldBtn.SetActive(false);
			return;
		}
		_objShieldBtn.SetActive(true);
		if (((COMA_PlayerSelf_Fishing)COMA_PlayerSelf.Instance).IsInShieldingList(_strOwerId))
		{
			_InShiledList = true;
			_objBlockIcon.SetActive(false);
			_objUnBlockIcon.SetActive(true);
		}
		else
		{
			_InShiledList = false;
			_objBlockIcon.SetActive(true);
			_objUnBlockIcon.SetActive(false);
		}
	}

	public void HandleEventButton_Shield(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			if (!(COMA_PlayerSelf.Instance == null))
			{
				if (_InShiledList)
				{
					((COMA_PlayerSelf_Fishing)COMA_PlayerSelf.Instance).DelShieldingId(_strOwerId);
				}
				else
				{
					((COMA_PlayerSelf_Fishing)COMA_PlayerSelf.Instance).AddShieldingId(_strOwerId);
				}
			}
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}
}
