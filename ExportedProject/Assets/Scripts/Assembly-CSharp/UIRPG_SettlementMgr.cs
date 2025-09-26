using System.Collections;
using MC_UIToolKit;
using Protocol.RPG.S2C;
using UnityEngine;

public class UIRPG_SettlementMgr : MonoBehaviour
{
	public delegate void SpecialAward();

	[SerializeField]
	private GameObject _win;

	[SerializeField]
	private GameObject _winEffect;

	[SerializeField]
	private GameObject _defeat;

	[SerializeField]
	private GameObject _btnBack;

	private bool[] _anisEnd = new bool[3];

	[SerializeField]
	private UILabel _levelUpLabel;

	private GameObject _objAudioNum;

	private float _initTime;

	private bool activeSuccessNumAni;

	[SerializeField]
	private UILabel _labelMedal;

	[SerializeField]
	private UILabel _labelGold;

	[SerializeField]
	private UILabel _labelExp;

	[SerializeField]
	private UILabel _labelGem;

	private CNumAni _medalAni;

	private CNumAni _goldAni;

	private CNumAni _expAni;

	private CNumAni _gemAni;

	public SpecialAward OnSpecialAward;

	[SerializeField]
	private Transform _decoTrans;

	[SerializeField]
	private GameObject _levelUpPopupObj;

	private int _gemNum;

	private int _cardId;

	private string _deco = string.Empty;

	private void Awake()
	{
		_btnBack.SetActive(false);
	}

	private void Start()
	{
	}

	private bool ProcessLvUp()
	{
		if (UIDataBufferCenter.Instance.RPGData.m_rpg_level > UIRPG_DataBufferCenter._challangePrePersonLv)
		{
			UIRPG_DataBufferCenter._challangePrePersonLv = UIDataBufferCenter.Instance.RPGData.m_rpg_level;
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.RPG_levelup);
			_levelUpPopupObj.SetActive(true);
			if (UIRPG_DataBufferCenter.GetAvailableMemberSlot() > UIRPG_DataBufferCenter._cardMemberNumPre)
			{
				UIRPG_DataBufferCenter._cardMemberNumPre = UIRPG_DataBufferCenter.GetAvailableMemberSlot();
				_levelUpLabel.text = TUITool.StringFormat(Localization.instance.Get("rpgresult_desc3"), UIRPG_DataBufferCenter.GetColorStringByRPGAndValue("00aaff", UIDataBufferCenter.Instance.RPGData.m_rpg_level));
			}
			else
			{
				_levelUpLabel.text = TUITool.StringFormat(Localization.instance.Get("rpgresult_desc2"), UIRPG_DataBufferCenter.GetColorStringByRPGAndValue("00aaff", UIDataBufferCenter.Instance.RPGData.m_rpg_level));
			}
			return true;
		}
		return false;
	}

	private void Update()
	{
		if (!activeSuccessNumAni)
		{
			return;
		}
		_anisEnd[0] = _medalAni.UpdateAni();
		_anisEnd[1] = _goldAni.UpdateAni();
		_anisEnd[2] = _expAni.UpdateAni();
		bool flag = false;
		if (_anisEnd[0] && _anisEnd[1] && _anisEnd[2])
		{
			flag = true;
		}
		if (flag)
		{
			if (_objAudioNum != null)
			{
				Object.DestroyObject(_objAudioNum);
				_objAudioNum = null;
			}
			activeSuccessNumAni = false;
			if (!ProcessLvUp() && OnSpecialAward != null)
			{
				OnSpecialAward();
			}
			_btnBack.SetActive(true);
		}
	}

	private IEnumerator HideWinEffect()
	{
		yield return new WaitForSeconds(2f);
		_winEffect.SetActive(false);
	}

	public void Init(bool success)
	{
		Time.timeScale = 1f;
		base.gameObject.SetActive(true);
		_win.SetActive(success);
		_defeat.SetActive(!success);
		if (success)
		{
			_winEffect.SetActive(true);
		}
		else
		{
			_btnBack.SetActive(true);
		}
		_initTime = Time.time;
		for (int i = 0; i < _anisEnd.Length; i++)
		{
			_anisEnd[i] = false;
		}
	}

	public void SetAward(int medal, int gold, int exp, int gem)
	{
		_objAudioNum = null;
		Debug.Log(medal + "," + gold + "," + exp + "," + gem);
		float num = 2.3f - (Time.time - _initTime);
		float num2 = ((!(num <= 0f)) ? (Time.time + num) : Time.time);
		float num3 = ((!(num <= 0f)) ? 0.16f : 0f);
		_medalAni = new CNumAni(0u, (uint)medal, num2, _labelMedal);
		_goldAni = new CNumAni(0u, (uint)gold, num2 + num3, _labelGold);
		_expAni = new CNumAni(0u, (uint)exp, num2 + num3 * 2f, _labelExp);
		activeSuccessNumAni = true;
		_medalAni._fDurTime = 1.5f;
		_goldAni._fDurTime = 1.5f;
		_expAni._fDurTime = 1.5f;
		StartCoroutine(PlayScrollAni(num2 - Time.time));
	}

	private IEnumerator PlayScrollAni(float f)
	{
		if (f <= 0f)
		{
			f = 0f;
		}
		yield return new WaitForSeconds(f);
		_objAudioNum = Object.Instantiate(Resources.Load("SoundEvent/2.1/Scrollup")) as GameObject;
		if (_objAudioNum != null)
		{
			Debug.LogWarning("-----------------------------------Play Scrollup");
			ITAudioEvent com = _objAudioNum.GetComponent<ITAudioEvent>();
			com.Trigger();
		}
	}

	public void CloseLvBox()
	{
		_levelUpPopupObj.SetActive(false);
		if (OnSpecialAward != null)
		{
			OnSpecialAward();
		}
	}

	public void HandleSpecialAward()
	{
		if (_gemNum > 0)
		{
			Debug.Log("----------------SpecialAward GemNum=" + _gemNum);
		}
		else if (_deco != string.Empty)
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.RPG_Card_upgrade);
			UIGetItemBoxData data = new UIGetItemBoxData(UIGetItemBoxData.EGetItemType.Rpg_DropDeco, "deco_" + _deco);
			UIGolbalStaticFun.PopGetItemBox(data);
		}
		else if (_cardId != 0)
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.RPG_Card_upgrade);
			GameObject gameObject = Object.Instantiate(Resources.Load("UI/RPG_Prefabs/UIRPG_GetCard")) as GameObject;
			UIRPG_BigCard component = gameObject.GetComponent<UIRPG_BigCard>();
			gameObject.transform.parent = _decoTrans;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			CombineCardResultCmd combineCardResultCmd = new CombineCardResultCmd();
			combineCardResultCmd.m_card_id = (uint)_cardId;
			component.InitCard(combineCardResultCmd);
		}
	}

	public void SetSpecialAward(int gemNum, int cardId, string deco)
	{
		_gemNum = gemNum;
		_cardId = cardId;
		_deco = deco;
		if (_gemNum > 0 || _deco != string.Empty || _cardId != 0)
		{
			OnSpecialAward = HandleSpecialAward;
		}
	}
}
