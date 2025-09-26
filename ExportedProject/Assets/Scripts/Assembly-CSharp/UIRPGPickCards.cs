using System.Collections;
using System.Collections.Generic;
using MessageID;
using Protocol.RPG.C2S;
using Protocol.RPG.S2C;
using UnityEngine;

public class UIRPGPickCards : UIEntity
{
	private List<int> _ngFirstPickCards = new List<int>();

	private bool _pickCardResultOk;

	private int _openCardCount;

	private float _fEnterTime;

	private bool _aniEnd;

	[SerializeField]
	private UIRPG_BigCard[] _rpgBigCard = new UIRPG_BigCard[5];

	[SerializeField]
	private GameObject _objOk;

	[SerializeField]
	private GameObject _popUpCardInfoObj;

	[SerializeField]
	private Transform _popUpCardInfoObjParent;

	private void InitPickCard()
	{
		_pickCardResultOk = false;
		_openCardCount = 0;
		_fEnterTime = 0f;
		_aniEnd = false;
	}

	public void PickCardsAniEnd()
	{
		_aniEnd = true;
	}

	private IEnumerator PlayOpen2Audio()
	{
		yield return new WaitForSeconds(1.1f);
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.RPG_GetCard_open02);
	}

	private IEnumerator PlayOpen3Audio()
	{
		yield return new WaitForSeconds(2.2f);
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.RPG_GetCard_open03);
	}

	protected override void Load()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.RPG_GetCard_open01);
		StartCoroutine(PlayOpen2Audio());
		StartCoroutine(PlayOpen3Audio());
		InitPickCard();
		if (!COMA_Pref.Instance.NG2_1_FirstEnterSquare)
		{
			RequestPickCardsCmd extraInfo = new RequestPickCardsCmd();
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, extraInfo);
			_objOk.SetActive(false);
		}
		else
		{
			_objOk.SetActive(false);
			if (UIDataBufferCenter.Instance.CurNGIndex == 2)
			{
				_pickCardResultOk = true;
				int num = _ngFirstPickCards[Random.Range(0, _ngFirstPickCards.Count)];
				Debug.Log("Must Card ID=" + num);
				int[] outArray = new int[3];
				COMA_Tools.GetRandomArray_AppointSize(7, ref outArray);
				int item = Random.Range(1, 25);
				List<uint> list = new List<uint>();
				list.Add((uint)num);
				for (int i = 0; i < 3; i++)
				{
					list.Add((uint)(outArray[i] + 1));
				}
				list.Add((uint)item);
				for (int j = 0; j < list.Count; j++)
				{
					NotifyCardAddCmd notifyCardAddCmd = new NotifyCardAddCmd();
					notifyCardAddCmd.m_unique_id = (uint)(10 * UIDataBufferCenter.Instance.CurNGIndex + j);
					notifyCardAddCmd.m_card_id = list[j];
					bool flag = true;
					Dictionary<uint, List<ulong>> card_list = UIDataBufferCenter.Instance.RPGData.m_card_list;
					if (card_list.ContainsKey(notifyCardAddCmd.m_card_id))
					{
						List<ulong> list2 = card_list[notifyCardAddCmd.m_card_id];
						if (list2 == null)
						{
							list2 = new List<ulong>();
						}
						list2.Add(notifyCardAddCmd.m_unique_id);
						flag = false;
					}
					else
					{
						List<ulong> list3 = new List<ulong>();
						list3.Add(notifyCardAddCmd.m_unique_id);
						card_list.Add(notifyCardAddCmd.m_card_id, list3);
					}
					notifyCardAddCmd.m_new = (byte)(flag ? 1u : 0u);
					RequestPickCardsResultCmd.Item item2 = new RequestPickCardsResultCmd.Item();
					item2.m_card_id = notifyCardAddCmd.m_card_id;
					item2.m_new = notifyCardAddCmd.m_new;
					item2.m_unique_id = notifyCardAddCmd.m_unique_id;
					RefreshCard(j, item2);
				}
			}
			else if (UIDataBufferCenter.Instance.CurNGIndex == 3)
			{
				_pickCardResultOk = true;
				int num2 = Random.Range(17, 25);
				Debug.Log("Must Card ID=" + num2);
				int[] outArray2 = new int[3];
				COMA_Tools.GetRandomArray_AppointSize(7, ref outArray2);
				int item3 = Random.Range(1, 25);
				List<uint> list4 = new List<uint>();
				list4.Add((uint)num2);
				for (int k = 0; k < 3; k++)
				{
					list4.Add((uint)(outArray2[k] + 1));
				}
				list4.Add((uint)item3);
				for (int l = 0; l < list4.Count; l++)
				{
					NotifyCardAddCmd notifyCardAddCmd2 = new NotifyCardAddCmd();
					notifyCardAddCmd2.m_unique_id = (uint)(10 * UIDataBufferCenter.Instance.CurNGIndex + l);
					notifyCardAddCmd2.m_card_id = list4[l];
					bool flag2 = true;
					Dictionary<uint, List<ulong>> card_list2 = UIDataBufferCenter.Instance.RPGData.m_card_list;
					if (card_list2.ContainsKey(notifyCardAddCmd2.m_card_id))
					{
						List<ulong> list5 = card_list2[notifyCardAddCmd2.m_card_id];
						if (list5 == null)
						{
							list5 = new List<ulong>();
						}
						list5.Add(notifyCardAddCmd2.m_unique_id);
						flag2 = false;
					}
					else
					{
						List<ulong> list6 = new List<ulong>();
						list6.Add(notifyCardAddCmd2.m_unique_id);
						card_list2.Add(notifyCardAddCmd2.m_card_id, list6);
					}
					notifyCardAddCmd2.m_new = (byte)(flag2 ? 1u : 0u);
					RequestPickCardsResultCmd.Item item4 = new RequestPickCardsResultCmd.Item();
					item4.m_card_id = notifyCardAddCmd2.m_card_id;
					item4.m_new = notifyCardAddCmd2.m_new;
					item4.m_unique_id = notifyCardAddCmd2.m_unique_id;
					RefreshCard(l, item4);
				}
			}
		}
		RegisterMessage(EUIMessageID.UIRPG_NotifyPickCardsResult, this, PickCardsResult);
		RegisterMessage(EUIMessageID.UIRPG_PopupCardInfoWindow, this, HandlePopupCardInfoWindow);
		RegisterMessage(EUIMessageID.UIContainer_BoxOnClick, this, CardClick);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIRPG_NotifyPickCardsResult, this);
		UnregisterMessage(EUIMessageID.UIRPG_PopupCardInfoWindow, this);
		UnregisterMessage(EUIMessageID.UIContainer_BoxOnClick, this);
	}

	public bool CardClick(TUITelegram msg)
	{
		if (!_pickCardResultOk)
		{
			return true;
		}
		if (!_aniEnd)
		{
			return true;
		}
		UIRPG_BigCard uIRPG_BigCard = msg._pExtraInfo as UIRPG_BigCard;
		if (!uIRPG_BigCard.IsTurnOverd())
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.RPG_GetCard);
			uIRPG_BigCard.TurnCard();
			_openCardCount++;
			if (_openCardCount >= 5)
			{
				_objOk.SetActive(true);
			}
		}
		return true;
	}

	public bool HandlePopupCardInfoWindow(TUITelegram msg)
	{
		int cardId = (int)msg._pExtraInfo;
		GameObject gameObject = Object.Instantiate(_popUpCardInfoObj) as GameObject;
		gameObject.transform.parent = _popUpCardInfoObjParent;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		UIRPG_CardInfo component = gameObject.GetComponent<UIRPG_CardInfo>();
		component.CardId = cardId;
		component.DisplayData();
		return true;
	}

	private bool PickCardsResult(TUITelegram msg)
	{
		RequestPickCardsResultCmd requestPickCardsResultCmd = msg._pExtraInfo as RequestPickCardsResultCmd;
		if (requestPickCardsResultCmd.m_result == 0)
		{
			_pickCardResultOk = true;
			for (int i = 0; i < requestPickCardsResultCmd.m_card_list.Count; i++)
			{
				RefreshCard(i, requestPickCardsResultCmd.m_card_list[i]);
			}
		}
		return true;
	}

	private void RefreshCard(int i, RequestPickCardsResultCmd.Item item)
	{
		_rpgBigCard[i].InitCard(item);
	}

	private void Awake()
	{
		_ngFirstPickCards.Clear();
		_ngFirstPickCards.Add(26);
		_ngFirstPickCards.Add(27);
		_ngFirstPickCards.Add(29);
		_ngFirstPickCards.Add(30);
		_ngFirstPickCards.Add(31);
		_ngFirstPickCards.Add(36);
		_ngFirstPickCards.Add(37);
		_ngFirstPickCards.Add(38);
		_ngFirstPickCards.Add(39);
	}

	protected override void Tick()
	{
	}
}
