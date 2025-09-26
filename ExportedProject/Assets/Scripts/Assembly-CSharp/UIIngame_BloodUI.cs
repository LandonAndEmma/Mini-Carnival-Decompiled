using UnityEngine;

public class UIIngame_BloodUI : MonoBehaviour
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
	private TUILabel _num_kill;

	[SerializeField]
	private TUILabel _num_die;

	[SerializeField]
	private TUILabel _num_selfTeamKill;

	[SerializeField]
	private TUILabel _num_opponentKill;

	[SerializeField]
	private Animation _hpAni;

	public float _fHpPicLen = 28f;

	[SerializeField]
	private UI_3DModeToTexture _modeToTex;

	public UIIngameBloodInfo _info;

	private static UIIngame_BloodUI _instance;

	private float _fPreHpNum = 1f;

	public static UIIngame_BloodUI Instance
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
		_info.Tex2D = _modeToTex.GetTexById(COMA_Network.Instance.TNetInstance.Myself.SitIndex, _info.DelayAssignment);
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
		float hP = _info.HP;
		if (_Hp != null)
		{
			hP = Mathf.Clamp01(hP);
			if (hP < _fPreHpNum && _hpAni != null)
			{
				_hpAni.Play();
			}
			_fPreHpNum = hP;
			float x = _fHpPicLen * (1f - hP) * -0.5f;
			Vector3 localPosition = _Hp.transform.localPosition;
			localPosition.x = x;
			_Hp.transform.localPosition = localPosition;
			Vector3 localScale = _Hp.transform.localScale;
			localScale.x = hP;
			_Hp.transform.localScale = localScale;
		}
		_diedIcon.SetActive(false);
		if (_headIcon != null)
		{
			_headIcon.CustomizeTexture = _info.Tex2D;
		}
		if (_num_die != null)
		{
			_num_die.Text = _info.NumDie.ToString();
		}
		if (_num_kill != null)
		{
			_num_kill.Text = _info.NumKill.ToString();
		}
		if (_num_selfTeamKill != null)
		{
			_num_selfTeamKill.Text = _info.NumSelfTeamKill.ToString();
		}
		if (_num_opponentKill != null)
		{
			_num_opponentKill.Text = _info.NumOpponentKill.ToString();
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
