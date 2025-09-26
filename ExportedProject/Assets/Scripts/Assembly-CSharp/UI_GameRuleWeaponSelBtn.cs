using UnityEngine;

public class UI_GameRuleWeaponSelBtn : MonoBehaviour
{
	public enum ETradeType
	{
		Free = 0,
		Gold = 1,
		Gem = 2
	}

	[SerializeField]
	private GameObject _sel;

	[SerializeField]
	private TUILabel _Money;

	[SerializeField]
	private TUILabel _Des;

	[SerializeField]
	private TUIMeshSprite _pic;

	[SerializeField]
	private GameObject[] _tradeTypeIcons;

	public ETradeType _tradeType;

	private bool _bNeedIAP;

	public string PropDesID
	{
		set
		{
			_Des.TextID = value;
		}
	}

	public int Money
	{
		get
		{
			if (_Money != null)
			{
				return int.Parse(_Money.Text);
			}
			return -1;
		}
		set
		{
			_tradeType = ETradeType.Free;
			int num = Mathf.Abs(value);
			if (_tradeTypeIcons.Length > 1)
			{
				if (value < 0)
				{
					_tradeType = ETradeType.Gem;
					_tradeTypeIcons[0].SetActive(false);
					_tradeTypeIcons[1].SetActive(true);
				}
				else if (value > 0)
				{
					_tradeType = ETradeType.Gold;
					_tradeTypeIcons[0].SetActive(true);
					_tradeTypeIcons[1].SetActive(false);
				}
			}
			if (_Money != null)
			{
				_Money.Text = num.ToString();
			}
		}
	}

	public string Pic
	{
		set
		{
			if (_pic != null)
			{
				_pic.m_texture = value;
				_pic.NeedUpdate = true;
			}
		}
	}

	public Texture2D WeaponIcon
	{
		set
		{
			if (_pic != null && value != null)
			{
				_pic.UseCustomize = true;
				_pic.CustomizeTexture = value;
				_pic.CustomizeRect = new Rect(0f, 0f, _pic.CustomizeTexture.width, _pic.CustomizeTexture.height);
			}
		}
	}

	public bool IsNeedIAP()
	{
		return _bNeedIAP;
	}

	private void RefreshMoney(int nHasMoney)
	{
		Debug.Log("Money=" + Money);
		if (Money > nHasMoney)
		{
			_Money.color = new Color(1f, 0f, 0f);
			_bNeedIAP = true;
		}
		else
		{
			_Money.color = new Color(0.13f, 0.13f, 0.13f);
			_bNeedIAP = false;
		}
	}

	public void RefreshMoney(string strParam)
	{
		string[] array = strParam.Split(',');
		if (array.Length > 1)
		{
			int nHasMoney = int.Parse(array[0]);
			int nHasMoney2 = int.Parse(array[1]);
			if (_tradeType == ETradeType.Gold)
			{
				RefreshMoney(nHasMoney);
			}
			else if (_tradeType == ETradeType.Gem)
			{
				RefreshMoney(nHasMoney2);
			}
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void WeaponSel(bool bSel)
	{
		_sel.SetActive(bSel);
	}
}
