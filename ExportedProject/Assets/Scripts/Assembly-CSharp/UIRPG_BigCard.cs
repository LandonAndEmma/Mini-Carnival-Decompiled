using NGUI_COMUI;
using Protocol.RPG.S2C;
using UnityEngine;

public class UIRPG_BigCard : NGUI_COMUI.UI_Box
{
	[SerializeField]
	private UISprite _new;

	[SerializeField]
	private UISprite[] _grade;

	[SerializeField]
	private UILabel _cardId;

	[SerializeField]
	private UILabel _cardName;

	[SerializeField]
	private GameObject _info;

	[SerializeField]
	private GameObject _infoBtn;

	[SerializeField]
	private GameObject _bg;

	[SerializeField]
	private UITexture _texCard;

	[SerializeField]
	private UISprite[] _cardColorSprite;

	private bool _bTurnOver;

	private void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public override void FormatBoxName(int i)
	{
		if (i > 9)
		{
			base.gameObject.name = "UIRPG_BigCard" + i;
		}
		else
		{
			base.gameObject.name = "UIRPG_BigCard0" + i;
		}
	}

	public override void BoxDataChanged()
	{
		UIRPG_BigCardData uIRPG_BigCardData = base.BoxData as UIRPG_BigCardData;
		if (uIRPG_BigCardData != null)
		{
			_infoBtn.SetActive(false);
			_info.SetActive(true);
			_bg.SetActive(false);
			_new.enabled = false;
			_cardId.text = uIRPG_BigCardData.CardId.ToString();
			_cardId.gameObject.SetActive(false);
			byte starGrade = RPGGlobalData.Instance.CareerUnitPool._dict[uIRPG_BigCardData.CardId].StarGrade;
			for (byte b = 0; b < _grade.Length; b++)
			{
				_grade[b].enabled = ((starGrade > b) ? true : false);
			}
			string path = "UI/RPGCardTextures/RPG_Big_" + uIRPG_BigCardData.CardId;
			Texture texture = Resources.Load(path) as Texture;
			if (texture != null)
			{
				_texCard.mainTexture = texture;
				_texCard.enabled = true;
			}
			_cardName.text = UIRPG_DataBufferCenter.GetCardCareerNameByCardId(uIRPG_BigCardData.CardId);
			SetColor(uIRPG_BigCardData.CardId);
		}
	}

	public bool IsTurnOverd()
	{
		return _bTurnOver;
	}

	public void InitCard(RequestPickCardsResultCmd.Item item)
	{
		Debug.Log("InitCard:" + item.m_card_id + "------" + base.gameObject.name);
		_bTurnOver = false;
		_info.SetActive(false);
		_bg.SetActive(true);
		_new.enabled = ((item.m_new != 0) ? true : false);
		_cardId.text = item.m_card_id.ToString();
		_cardId.gameObject.SetActive(false);
		byte starGrade = RPGGlobalData.Instance.CareerUnitPool._dict[(int)item.m_card_id].StarGrade;
		for (byte b = 0; b < _grade.Length; b++)
		{
			_grade[b].enabled = ((starGrade > b) ? true : false);
		}
		string path = "UI/RPGCardTextures/RPG_Big_" + item.m_card_id;
		Texture texture = Resources.Load(path) as Texture;
		if (texture != null)
		{
			_texCard.mainTexture = texture;
			_texCard.enabled = true;
		}
		_cardName.text = UIRPG_DataBufferCenter.GetCardCareerNameByCardId((int)item.m_card_id);
		SetColor((int)item.m_card_id);
	}

	public void InitCard(CombineCardResultCmd item)
	{
		_info.SetActive(true);
		_bg.SetActive(false);
		_new.enabled = !UIRPG_CardLibraryMgr.IsOwnCard((int)item.m_card_id);
		_cardId.text = item.m_card_id.ToString();
		_cardId.gameObject.SetActive(false);
		byte starGrade = RPGGlobalData.Instance.CareerUnitPool._dict[(int)item.m_card_id].StarGrade;
		for (byte b = 0; b < _grade.Length; b++)
		{
			_grade[b].enabled = ((starGrade > b) ? true : false);
		}
		string path = "UI/RPGCardTextures/RPG_Big_" + item.m_card_id;
		Texture texture = Resources.Load(path) as Texture;
		if (texture != null)
		{
			_texCard.mainTexture = texture;
			_texCard.enabled = true;
		}
		_cardName.text = UIRPG_DataBufferCenter.GetCardCareerNameByCardId((int)item.m_card_id);
		SetColor((int)item.m_card_id);
	}

	public int GetCardId()
	{
		return int.Parse(_cardId.text);
	}

	private void SetColor(int cardId)
	{
		for (int i = 0; i < _cardColorSprite.Length; i++)
		{
			_cardColorSprite[i].color = UIRPG_DataBufferCenter.GetCardColorByGrade(RPGGlobalData.Instance.CareerUnitPool._dict[cardId].StarGrade);
		}
	}

	public static Color GetCardEffectColorByGrade(byte grade)
	{
		Color result = Color.red;
		switch (grade)
		{
		case 3:
			result = new Color(0f, 20f / 51f, 1f);
			break;
		case 2:
			result = new Color(0f, 2f / 3f, 5f / 51f);
			break;
		case 4:
			result = new Color(37f / 85f, 0f, 0.8f);
			break;
		case 5:
			result = new Color(1f, 37f / 85f, 0f);
			break;
		case 6:
			result = new Color(1f, 0.05490196f, 0f);
			break;
		}
		return result;
	}

	public bool TurnOver()
	{
		_info.SetActive(true);
		_bg.SetActive(false);
		GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/RPG_Flash_PFB/UIRPG_Big_total")) as GameObject;
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		Object.Destroy(gameObject, 1.5f);
		if (RPGGlobalData.Instance.CareerUnitPool._dict[GetCardId()].StarGrade > 1)
		{
			GameObject gameObject2 = Object.Instantiate(Resources.Load("Particle/effect/RPG_Flash_PFB/UIRPG_Big_color")) as GameObject;
			gameObject2.transform.parent = base.transform;
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
			ParticleSystem[] componentsInChildren = gameObject2.GetComponentsInChildren<ParticleSystem>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Color cardEffectColorByGrade = GetCardEffectColorByGrade(RPGGlobalData.Instance.CareerUnitPool._dict[GetCardId()].StarGrade);
				componentsInChildren[i].renderer.material.color = cardEffectColorByGrade;
			}
			Object.Destroy(gameObject2, 1.5f);
		}
		if (RPGGlobalData.Instance.CareerUnitPool._dict[GetCardId()].StarGrade >= 4)
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.RPG_GetCard_elite);
		}
		if (_new.enabled)
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.RPG_GetCard_new);
		}
		return false;
	}

	public void TurnCard()
	{
		if (!_bTurnOver)
		{
			_bTurnOver = true;
			base.animation.Play();
			SceneTimerInstance.Instance.Add(0.15f, TurnOver);
		}
	}
}
