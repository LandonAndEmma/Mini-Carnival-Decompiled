using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class COMA_Server_Award : MonoBehaviour
{
	private static COMA_Server_Award _instance;

	protected string serverName_Bonus = "svr_bonus";

	protected string serverName_BonusRanking = "svr_bonusRanking";

	protected string actionName_acceptDailyBonus = "Callofminiavatar/AcceptDailyBonus";

	protected string actionName_acceptServerBonus = "Callofminiavatar/AcceptServerBonus";

	protected string actionName_acceptRankingBonus = "comavatarranking/AcceptRankingBonus";

	[NonSerialized]
	public int daylyBonus;

	public List<UI_OneAwardData.SWorldRankingAward> lst_awards = new List<UI_OneAwardData.SWorldRankingAward>();

	public List<UI_OneAwardData.SWorldRankingAward> lst_ranking_awards = new List<UI_OneAwardData.SWorldRankingAward>();

	[NonSerialized]
	public float rankingAwardsInterval = 60f;

	public static COMA_Server_Award Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
	}

	private void OnEnable()
	{
		_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnDisable()
	{
		_instance = null;
	}

	private void Start()
	{
	}

	public void InitServer(string addr, float timeout, string key)
	{
		HttpClient.Instance().AddServer(serverName_Bonus, addr, timeout, key);
	}

	public void InitServerRanking(string addr, float timeout, string key)
	{
		HttpClient.Instance().AddServer(serverName_BonusRanking, addr, timeout, key);
	}

	public void AcceptDailyBonus(string GID)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", GID);
		string text = JsonMapper.ToJson(hashtable);
		Debug.Log(text);
		HttpClient.Instance().SendRequest(serverName_Bonus, actionName_acceptDailyBonus, text, base.gameObject.name, "COMA_Server_Award", "ReceiveFunction", string.Empty);
	}

	public void AcceptServerBonus(string GID)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", GID);
		string text = JsonMapper.ToJson(hashtable);
		Debug.Log(text);
		HttpClient.Instance().SendRequest(serverName_Bonus, actionName_acceptServerBonus, text, base.gameObject.name, "COMA_Server_Award", "ReceiveFunction", string.Empty);
	}

	public void AcceptRankingBonus(string GID)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", GID);
		string text = JsonMapper.ToJson(hashtable);
		Debug.Log(text);
		HttpClient.Instance().SendRequest(serverName_BonusRanking, actionName_acceptRankingBonus, text, base.gameObject.name, "COMA_Server_Award", "ReceiveFunction", string.Empty);
	}

	public void GetAllRewards()
	{
		COMA_Pref.Instance.AddCrystal(daylyBonus);
		for (int i = 0; i < lst_awards.Count; i++)
		{
			if (lst_awards[i].nAwardNum > 0)
			{
				COMA_Pref.Instance.AddCrystal(lst_awards[i].nAwardNum);
				continue;
			}
			COMA_PackageItem cOMA_PackageItem = new COMA_PackageItem();
			cOMA_PackageItem.serialName = lst_awards[i].serialName;
			cOMA_PackageItem.itemName = lst_awards[i].itemName;
			cOMA_PackageItem.part = lst_awards[i].part;
			cOMA_PackageItem.state = COMA_PackageItem.PackageItemStatus.None;
			int anItem = COMA_Pref.Instance.GetAnItem(cOMA_PackageItem);
			StartCoroutine(COMA_Version.Instance.CreateIconInPackage(anItem));
		}
		for (int j = 0; j < lst_ranking_awards.Count; j++)
		{
			if (lst_ranking_awards[j].nAwardNum > 0)
			{
				COMA_Pref.Instance.AddCrystal(lst_ranking_awards[j].nAwardNum);
				continue;
			}
			COMA_PackageItem cOMA_PackageItem2 = new COMA_PackageItem();
			cOMA_PackageItem2.serialName = lst_ranking_awards[j].serialName;
			cOMA_PackageItem2.itemName = lst_ranking_awards[j].itemName;
			cOMA_PackageItem2.part = lst_ranking_awards[j].part;
			cOMA_PackageItem2.state = COMA_PackageItem.PackageItemStatus.None;
			int anItem2 = COMA_Pref.Instance.GetAnItem(cOMA_PackageItem2);
			StartCoroutine(COMA_Version.Instance.CreateIconInPackage(anItem2));
		}
		AcceptDailyBonus(COMA_Server_ID.Instance.GID);
		AcceptServerBonus(COMA_Server_ID.Instance.GID);
		AcceptRankingBonus(COMA_Server_ID.Instance.GID);
		daylyBonus = 0;
		lst_awards.Clear();
		lst_ranking_awards.Clear();
		COMA_Pref.Instance.Save(true);
	}

	public void ReceiveFunction(int taskId, int result, string server, string action, string response, object param)
	{
		if (result != 0)
		{
			Debug.LogError("result : " + result + " : " + server + " " + action);
			if (action == actionName_acceptDailyBonus)
			{
				AcceptDailyBonus(COMA_Server_ID.Instance.GID);
			}
			else if (action == actionName_acceptServerBonus)
			{
				AcceptServerBonus(COMA_Server_ID.Instance.GID);
			}
			else if (action == actionName_acceptRankingBonus)
			{
				AcceptRankingBonus(COMA_Server_ID.Instance.GID);
			}
			return;
		}
		Debug.LogWarning("领奖服务器返回 : " + response);
		if (server == serverName_Bonus)
		{
			if (!(action == actionName_acceptDailyBonus) && !(action == actionName_acceptServerBonus))
			{
			}
		}
		else if (server == serverName_BonusRanking && !(action == actionName_acceptRankingBonus))
		{
		}
	}
}
