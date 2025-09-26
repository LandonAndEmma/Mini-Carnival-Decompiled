using UnityEngine;

public class UIInGame_HungersPlayerInfo : MonoBehaviour
{
	public UIHungersPlayerInfo _playInfo = new UIHungersPlayerInfo();

	[SerializeField]
	private TUILabel _nameLabel;

	[SerializeField]
	private GameObject _objDiedIcon;

	[SerializeField]
	private TUIMeshSprite _texIcon;

	[SerializeField]
	private GameObject _objHp;

	[SerializeField]
	private float _fHpPicLen = 28f;

	private float _fOldHp = 1f;

	private void Start()
	{
		_fOldHp = 1f;
	}

	private void Update()
	{
	}

	public void RefreshUI()
	{
		if (_nameLabel != null)
		{
			_nameLabel.Text = _playInfo.Name;
		}
		float num = _playInfo.HP;
		if (_objHp != null)
		{
			num = Mathf.Clamp01(num);
			float x = _fHpPicLen * (1f - num) * -0.5f;
			Vector3 localPosition = _objHp.transform.localPosition;
			localPosition.x = x;
			_objHp.transform.localPosition = localPosition;
			Vector3 localScale = _objHp.transform.localScale;
			localScale.x = num;
			_objHp.transform.localScale = localScale;
			if (_playInfo.IsSelf)
			{
				_objHp.GetComponent<TUIMeshSprite>().color = new Color(0.31f, 0.584f, 0.02f);
				if (num < _fOldHp && _objHp.transform.parent != null && _objHp.transform.parent.animation != null)
				{
					_objHp.transform.parent.animation.Play();
				}
				_fOldHp = num;
			}
			else
			{
				_objHp.GetComponent<TUIMeshSprite>().color = new Color(0.73f, 0f, 0f);
			}
		}
		if (num <= 0f)
		{
			_objDiedIcon.SetActive(true);
		}
		else
		{
			_objDiedIcon.SetActive(false);
		}
		if (_texIcon != null)
		{
			_texIcon.CustomizeTexture = _playInfo.Tex2D;
		}
	}

	public void RefreshHeadIconUI()
	{
		if (_texIcon != null)
		{
			_texIcon.CustomizeTexture = _playInfo.Tex2D;
		}
	}
}
