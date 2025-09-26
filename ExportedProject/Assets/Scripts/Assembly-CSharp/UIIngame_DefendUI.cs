using UnityEngine;

public class UIIngame_DefendUI : MonoBehaviour
{
	[SerializeField]
	private GameObject _diedIcon;

	[SerializeField]
	private TUIMeshSprite _headIcon;

	[SerializeField]
	private GameObject _Hp;

	[SerializeField]
	private TUILabel _name;

	[SerializeField]
	private TUILabel _num;

	[SerializeField]
	private Animation _hpAni;

	public float _fHpPicLen = 28f;

	[SerializeField]
	private UI_3DModeToTexture _modeToTex;

	public UIIngameDefendInfo _info;

	private static UIIngame_DefendUI _instance;

	private float _fPreHpNum = 1f;

	public static UIIngame_DefendUI Instance
	{
		get
		{
			return _instance;
		}
	}

	private void OnEnable()
	{
		_instance = this;
	}

	private void OnDisable()
	{
		_instance = null;
	}

	private void Start()
	{
		_info.Tex2D = _modeToTex.GetTexById(0, _info.DelayAssignment);
		_info.UIInfo = this;
		RefreshUI();
	}

	private void Update()
	{
	}

	public void RefreshUI()
	{
		if (_name != null)
		{
			_name.Text = _info.Name;
		}
		float num = _info.HP;
		if (_Hp != null)
		{
			num = Mathf.Clamp01(num);
			if (num < _fPreHpNum && _hpAni != null)
			{
				_hpAni.Play();
			}
			_fPreHpNum = num;
			float x = _fHpPicLen * (1f - num) * -0.5f;
			Vector3 localPosition = _Hp.transform.localPosition;
			localPosition.x = x;
			_Hp.transform.localPosition = localPosition;
			Vector3 localScale = _Hp.transform.localScale;
			localScale.x = num;
			_Hp.transform.localScale = localScale;
		}
		if (num <= 0f)
		{
			_diedIcon.SetActive(true);
		}
		else
		{
			_diedIcon.SetActive(false);
		}
		if (_headIcon != null)
		{
			_headIcon.CustomizeTexture = _info.Tex2D;
		}
		if (_num != null)
		{
			_num.Text = _info.Num.ToString();
		}
	}

	public void RefreshHeadIconUI()
	{
		if (_headIcon != null)
		{
			_headIcon.CustomizeTexture = _info.Tex2D;
		}
	}
}
