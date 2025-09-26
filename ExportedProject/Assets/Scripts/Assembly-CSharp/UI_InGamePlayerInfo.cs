using UnityEngine;

public class UI_InGamePlayerInfo : MonoBehaviour
{
	[SerializeField]
	private TUILabel _nameLabel;

	[SerializeField]
	private TUILabel _starNumLabel;

	[SerializeField]
	private GameObject _objDiedIcon;

	[SerializeField]
	private TUIMeshSprite _texIcon;

	[SerializeField]
	private GameObject _objHp;

	[SerializeField]
	private float _fHpPicLen = 103f;

	public UIPlayerInfo _playInfo = new UIPlayerInfo();

	private void Start()
	{
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
		if (_starNumLabel != null)
		{
			_starNumLabel.Text = _playInfo.Num.ToString();
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
