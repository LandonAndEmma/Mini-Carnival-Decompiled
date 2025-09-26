using UnityEngine;

public class UIRPGATTRanking : MonoBehaviour
{
	[SerializeField]
	private UISprite _frame;

	[SerializeField]
	private UISprite _frameDup;

	[SerializeField]
	private UILabel _labelCareerId;

	[SerializeField]
	private UILabel _labelCareerIdDup;

	[SerializeField]
	private UISprite _spriteCareer;

	[SerializeField]
	private UISprite _spriteCareerDup;

	[SerializeField]
	private bool _firstPos;

	private Color _frameColor;

	[SerializeField]
	private GameObject _objDup;

	[SerializeField]
	private GameObject _objOri;

	[SerializeField]
	private UIRPGATTRankingMgr _mgr;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public static int GetIconIDByCardId(int cardId)
	{
		int result = cardId;
		switch (cardId)
		{
		case 500:
			result = 5;
			break;
		case 501:
			result = 3;
			break;
		case 502:
			result = 11;
			break;
		case 503:
			result = 13;
			break;
		case 504:
			result = 14;
			break;
		case 505:
			result = 18;
			break;
		case 506:
			result = 17;
			break;
		case 507:
			result = 25;
			break;
		case 508:
			result = 26;
			break;
		case 509:
			result = 16;
			break;
		case 510:
			result = 28;
			break;
		case 511:
			result = 36;
			break;
		case 512:
			result = 37;
			break;
		case 513:
			result = 39;
			break;
		case 514:
			result = 41;
			break;
		case 515:
			result = 42;
			break;
		case 516:
			result = 43;
			break;
		case 517:
			result = 44;
			break;
		case 518:
			result = 47;
			break;
		case 519:
			result = 48;
			break;
		}
		return result;
	}

	public void RefreshUI(RPGEntity entity)
	{
		if (entity == null)
		{
			base.gameObject.SetActive(false);
			_frame.color = new Color(1f, 1f, 1f);
			_labelCareerId.text = string.Empty;
			_labelCareerIdDup.text = string.Empty;
			_spriteCareer.enabled = false;
			if (_spriteCareerDup != null)
			{
				_spriteCareerDup.enabled = false;
			}
			return;
		}
		base.gameObject.SetActive(true);
		_frameColor = ((!entity.IsPlayer()) ? Color.red : Color.blue);
		if (!_firstPos)
		{
			_spriteCareer.enabled = true;
		}
		_frame.color = _frameColor;
		_labelCareerId.text = entity.CareerUnit.CareerId.ToString();
		_labelCareerIdDup.text = _labelCareerId.text;
		_spriteCareer.spriteName = "RPG_SS_" + GetIconIDByCardId(entity.CareerUnit.CareerId);
	}

	public void HidePos()
	{
		_labelCareerId.enabled = false;
		_labelCareerIdDup.enabled = true;
		_spriteCareer.enabled = false;
		_spriteCareerDup.enabled = true;
		if (_objDup != null)
		{
			_objDup.SetActive(true);
		}
		_objOri.SetActive(false);
		if (_spriteCareerDup != null)
		{
			_spriteCareerDup.spriteName = _spriteCareer.spriteName;
		}
		if (_frameDup != null)
		{
			_frameDup.color = _frameColor;
		}
	}

	public void HideDup()
	{
		_labelCareerIdDup.enabled = false;
		_spriteCareerDup.enabled = false;
		if (_objDup != null)
		{
			_objDup.SetActive(true);
		}
	}

	public void ResetPos()
	{
		_objOri.SetActive(true);
		_labelCareerId.enabled = true;
		_spriteCareer.enabled = true;
		_labelCareerId.transform.localPosition = new Vector3(2f, 6f, 0f);
		_labelCareerId.transform.localScale = new Vector3(24f, 24f, 1f);
		_objOri.transform.localPosition = new Vector3(0f, 3f, 0f);
		_objOri.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	public void JumpEnd()
	{
		_mgr.JumpEnd();
	}
}
