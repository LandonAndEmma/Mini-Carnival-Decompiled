using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using MessageID;
using Protocol.Rank.C2S;
using UnityEngine;

public class COMA_Server_Rank : MonoBehaviour
{
	public const int rankCount = 11;

	private static COMA_Server_Rank _instance;

	protected string serverName_Rank = "svr_rank";

	protected string actionName_RankWithMode = "comavatarranking/TopRanking";

	protected string actionName_RankOfFriends = "comavatarranking/TopFriendRanking";

	protected string actionName_SubmitScore = "comavatarranking/SetRanking";

	public UIRankingList_WorldRankingData[] selfRankWorlds = new UIRankingList_WorldRankingData[11];

	public List<UIRankingList_WorldRankingData>[] lst_rankWorlds = new List<UIRankingList_WorldRankingData>[11];

	public List<UIRankingList_FriendRankingData>[] lst_rankFriends = new List<UIRankingList_FriendRankingData>[11];

	public List<string> fishingWinnerID = new List<string>();

	public List<string> fishingWinnerName = new List<string>();

	public List<string> fishingWinnerWeight = new List<string>();

	private int rewardCountWorlds;

	[NonSerialized]
	public int lstCountWorlds = 50;

	[NonSerialized]
	public int lstCountFriends = 30;

	[NonSerialized]
	public int leftDays;

	[NonSerialized]
	public int leftHours;

	private float rankInterval;

	public static COMA_Server_Rank Instance
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
		HttpClient.Instance().AddServer(serverName_Rank, addr, timeout, key);
		COMA_Server_Award.Instance.InitServerRanking(addr, timeout, key);
	}

	private void Update()
	{
		rankInterval -= Time.deltaTime;
		if (rankInterval < 0f)
		{
			rankInterval = 0f;
		}
	}

	public void GetRankings()
	{
		if (!(rankInterval > 1f))
		{
			rankInterval = 120f;
			for (int i = 0; i < COMA_Version.Instance.orders.Length; i++)
			{
				GetRankWithMode(COMA_Server_ID.Instance.GID, lstCountWorlds, COMA_CommonOperation.Instance.SceneIDToRankID(COMA_Version.Instance.orders[i]));
				GetRankOfFriends(COMA_Server_ID.Instance.GID, lstCountFriends, COMA_CommonOperation.Instance.SceneIDToRankID(COMA_Version.Instance.orders[i]));
			}
		}
	}

	public void ChangeSelfScore(int scoreAdd, int id)
	{
		for (int i = 0; i < lst_rankFriends[id].Count; i++)
		{
			if (lst_rankFriends[id][i].RankNum != selfRankWorlds[id].RankNum || !(lst_rankFriends[id][i].PlayerName == selfRankWorlds[id].PlayerName) || lst_rankFriends[id][i].ScoreNum != selfRankWorlds[id].ScoreNum)
			{
				continue;
			}
			lst_rankFriends[id][i].ScoreNum += scoreAdd;
			for (int j = 0; j < i; j++)
			{
				if (lst_rankFriends[id][i].ScoreNum > lst_rankFriends[id][j].ScoreNum && lst_rankFriends[id][i].RankNum < lst_rankFriends[id][j].RankNum)
				{
					string playerName = lst_rankFriends[id][j].PlayerName;
					lst_rankFriends[id][j].PlayerName = lst_rankFriends[id][i].PlayerName;
					lst_rankFriends[id][i].PlayerName = playerName;
					int scoreNum = lst_rankFriends[id][j].ScoreNum;
					lst_rankFriends[id][j].ScoreNum = lst_rankFriends[id][i].ScoreNum;
					lst_rankFriends[id][i].ScoreNum = scoreNum;
				}
			}
			for (int num = lst_rankFriends[id].Count - 1; num > i; num--)
			{
				if (lst_rankFriends[id][i].ScoreNum < lst_rankFriends[id][num].ScoreNum && lst_rankFriends[id][i].RankNum > lst_rankFriends[id][num].RankNum)
				{
					string playerName2 = lst_rankFriends[id][num].PlayerName;
					lst_rankFriends[id][num].PlayerName = lst_rankFriends[id][i].PlayerName;
					lst_rankFriends[id][i].PlayerName = playerName2;
					int scoreNum2 = lst_rankFriends[id][num].ScoreNum;
					lst_rankFriends[id][num].ScoreNum = lst_rankFriends[id][i].ScoreNum;
					lst_rankFriends[id][i].ScoreNum = scoreNum2;
				}
			}
		}
		int k;
		for (k = 0; k < lst_rankWorlds[id].Count && (lst_rankWorlds[id][k].RankNum != selfRankWorlds[id].RankNum || !(lst_rankWorlds[id][k].PlayerName == selfRankWorlds[id].PlayerName) || lst_rankWorlds[id][k].ScoreNum != selfRankWorlds[id].ScoreNum); k++)
		{
		}
		selfRankWorlds[id].ScoreNum += scoreAdd;
		if (selfRankWorlds[id].ScoreNum < 0)
		{
			selfRankWorlds[id].ScoreNum = 0;
		}
		if (k < lst_rankWorlds[id].Count)
		{
			lst_rankWorlds[id][k].ScoreNum = selfRankWorlds[id].ScoreNum;
		}
		if (scoreAdd > 0)
		{
			for (int num2 = k - 1; num2 >= 0; num2--)
			{
				if (selfRankWorlds[id].ScoreNum > lst_rankWorlds[id][num2].ScoreNum)
				{
					if (num2 < lst_rankWorlds[id].Count - 1)
					{
						lst_rankWorlds[id][num2 + 1].PlayerName = lst_rankWorlds[id][num2].PlayerName;
						lst_rankWorlds[id][num2 + 1].ScoreNum = lst_rankWorlds[id][num2].ScoreNum;
					}
					lst_rankWorlds[id][num2].PlayerName = selfRankWorlds[id].PlayerName;
					lst_rankWorlds[id][num2].ScoreNum = selfRankWorlds[id].ScoreNum;
					selfRankWorlds[id].RankNum = lst_rankWorlds[id][num2].RankNum;
					selfRankWorlds[id].Award = lst_rankWorlds[id][num2].Award;
				}
			}
		}
		else
		{
			if (scoreAdd >= 0)
			{
				return;
			}
			for (int l = k + 1; l < lst_rankWorlds[id].Count; l++)
			{
				if (selfRankWorlds[id].ScoreNum < lst_rankWorlds[id][l].ScoreNum)
				{
					lst_rankWorlds[id][l - 1].PlayerName = lst_rankWorlds[id][l].PlayerName;
					lst_rankWorlds[id][l - 1].ScoreNum = lst_rankWorlds[id][l].ScoreNum;
					lst_rankWorlds[id][l].PlayerName = selfRankWorlds[id].PlayerName;
					lst_rankWorlds[id][l].ScoreNum = selfRankWorlds[id].ScoreNum;
					selfRankWorlds[id].RankNum = lst_rankWorlds[id][l].RankNum;
					selfRankWorlds[id].Award = lst_rankWorlds[id][l].Award;
				}
			}
		}
	}

	public void GetRankWithMode(string GID, int number, string rankID)
	{
		if (!(rankID == string.Empty))
		{
			GetRankListCmd getRankListCmd = new GetRankListCmd();
			getRankListCmd.m_rank_name = rankID;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, getRankListCmd);
		}
	}

	public void GetRankOfFriends(string GID, int number, string rankID)
	{
		if (!(rankID == string.Empty))
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("uuid", GID);
			hashtable.Add("num", number);
			hashtable.Add("rankId", rankID);
			string data = JsonMapper.ToJson(hashtable);
			HttpClient.Instance().SendRequest(serverName_Rank, actionName_RankOfFriends, data, base.gameObject.name, "COMA_Server_Rank", "ReceiveFunction", rankID);
		}
	}

	public void SubmitScore(string GID, int score, string rankID, string nickname)
	{
		if (!(rankID == string.Empty))
		{
			Debug.LogWarning("Submit Score: " + score);
			UpdateScoreCmd updateScoreCmd = new UpdateScoreCmd();
			updateScoreCmd.m_rank_name = rankID;
			updateScoreCmd.m_score = (uint)score;
			if (rankID == "ComAvatar011" || rankID == "ComAvatar007")
			{
				updateScoreCmd.m_op = 0;
			}
			else
			{
				updateScoreCmd.m_op = 1;
			}
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, updateScoreCmd);
		}
	}

	public void ReceiveFunction(int taskId, int result, string server, string action, string response, object param)
	{
		if (result != 0)
		{
			Debug.LogError("result : " + result + " : " + server + " " + action + " " + param);
			if (server == serverName_Rank)
			{
				string rankID = (string)param;
				GetRankWithMode(COMA_Server_ID.Instance.GID, lstCountWorlds, rankID);
			}
			else if (action == actionName_RankOfFriends)
			{
				string rankID2 = (string)param;
				GetRankOfFriends(COMA_Server_ID.Instance.GID, lstCountFriends, rankID2);
			}
		}
		else
		{
			if (!(server == serverName_Rank))
			{
				return;
			}
			Debug.Log(string.Concat("排行榜服务器返回 : ", action, "  ", param, "  ", response));
			JsonData jsonData = JsonMapper.ToObject<JsonData>(response);
			if (action == actionName_RankWithMode)
			{
				string text = (string)param;
				if (text.StartsWith("ComAvatar007") && response.Contains("\"win_users\":"))
				{
					IList list = jsonData["win_users"];
					fishingWinnerID.Clear();
					fishingWinnerName.Clear();
					fishingWinnerWeight.Clear();
					for (int i = 0; i < list.Count; i++)
					{
						JsonData jsonData2 = (JsonData)list[i];
						string text2 = jsonData2["uid"].ToString();
						string text3 = jsonData2["uname"].ToString();
						int num = int.Parse(jsonData2["score"].ToString());
						fishingWinnerID.Add(text2);
						fishingWinnerName.Add(text3);
						fishingWinnerWeight.Add(num.ToString());
						Debug.Log("uuid:" + text2 + "  name:" + text3);
					}
					if (list.Count > 0)
					{
						int iDByName = TFishingAddressBook.Instance.GetIDByName(1);
						TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 1016, TTelegram.SEND_MSG_IMMEDIATELY, null);
					}
				}
				if (text.StartsWith("ComAvatar011"))
				{
					Debug.LogWarning(text + "++++++++++++++++++++++++++++++++++++++");
				}
				int num2 = int.Parse(text.Substring(9)) - 1;
				IList list2 = jsonData["datas"];
				int num3 = int.Parse(jsonData["rankCount"].ToString());
				int num4 = int.Parse(jsonData["selfRank"].ToString());
				int num5 = int.Parse(jsonData["winCount"].ToString());
				IList list3 = jsonData["awards"];
				int num6 = (int)jsonData["rankLeftTime"];
				int num7 = int.Parse(jsonData["myscore"].ToString());
				if (text.StartsWith("ComAvatar011"))
				{
					COMA_Pref.Instance.mode_flappy_weekScore = num7;
				}
				if (text.StartsWith("ComAvatar011"))
				{
					Debug.Log("------------------------------------------------------++" + COMA_Pref.Instance.mode_flappy_weekScore);
				}
				Debug.Log("排行榜最大排名数:" + num3 + " 自己的排名:" + num4 + " 奖励的名次:" + num5 + " 自己的分数:" + num7);
				rewardCountWorlds = list3.Count;
				lst_rankWorlds[num2] = new List<UIRankingList_WorldRankingData>();
				for (int j = 0; j < list2.Count; j++)
				{
					JsonData jsonData3 = (JsonData)list2[j];
					string playerId = jsonData3["uuid"].ToString();
					string playerName = jsonData3["name"].ToString();
					int scoreNum = int.Parse(jsonData3["score"].ToString());
					int num8 = int.Parse(jsonData3["rank"].ToString());
					UIRankingList_WorldRankingData uIRankingList_WorldRankingData = new UIRankingList_WorldRankingData();
					uIRankingList_WorldRankingData.RankNum = j + 1;
					uIRankingList_WorldRankingData.PlayerName = playerName;
					uIRankingList_WorldRankingData.ScoreNum = scoreNum;
					uIRankingList_WorldRankingData.PlayerId = playerId;
					lst_rankWorlds[num2].Add(uIRankingList_WorldRankingData);
				}
				for (int k = lst_rankWorlds[num2].Count; k < lstCountWorlds; k++)
				{
					UIRankingList_WorldRankingData uIRankingList_WorldRankingData2 = new UIRankingList_WorldRankingData();
					uIRankingList_WorldRankingData2.RankNum = k + 1;
					uIRankingList_WorldRankingData2.PlayerName = "P" + UnityEngine.Random.Range(0, 1000000000).ToString("d9");
					uIRankingList_WorldRankingData2.ScoreNum = 0;
					uIRankingList_WorldRankingData2.PlayerId = string.Empty;
					lst_rankWorlds[num2].Add(uIRankingList_WorldRankingData2);
				}
				int num9 = Mathf.Min(lst_rankWorlds[num2].Count, list3.Count);
				for (int l = 0; l < num9; l++)
				{
					JsonData jsonData4 = (JsonData)list3[l];
					lst_rankWorlds[num2][l].Award.nAwardNum = (int)jsonData4["cr"];
					if (lst_rankWorlds[num2][l].Award.nAwardNum <= 0)
					{
						JsonData jsonData5 = jsonData4["item"];
						lst_rankWorlds[num2][l].Award.serialName = jsonData5["sn"].ToString();
						lst_rankWorlds[num2][l].Award.itemName = jsonData5["in"].ToString();
						lst_rankWorlds[num2][l].Award.part = int.Parse(jsonData5["part"].ToString());
					}
				}
				int num10 = num6 / 1000 / 3600;
				Debug.Log("领奖剩余时间(h):" + num10 + " " + num6);
				leftDays = num10 / 24;
				leftHours = num10 % 24;
				selfRankWorlds[num2] = new UIRankingList_WorldRankingData();
				selfRankWorlds[num2].RankNum = num4;
				selfRankWorlds[num2].ScoreNum = num7;
				selfRankWorlds[num2].PlayerName = COMA_Pref.Instance.nickname;
				COMA_Pref.Instance.SetRankScore(num7, num2);
				if (selfRankWorlds[num2].RankNum <= rewardCountWorlds)
				{
					int index = selfRankWorlds[num2].RankNum - 1;
					selfRankWorlds[num2].Award.nAwardNum = lst_rankWorlds[num2][index].Award.nAwardNum;
					selfRankWorlds[num2].Award.serialName = lst_rankWorlds[num2][index].Award.serialName;
					selfRankWorlds[num2].Award.itemName = lst_rankWorlds[num2][index].Award.itemName;
					selfRankWorlds[num2].Award.part = lst_rankWorlds[num2][index].Award.part;
				}
				if (text.StartsWith("ComAvatar001") && response.Contains("\"RankingBonus\":"))
				{
					COMA_Server_Award.Instance.lst_ranking_awards.Clear();
					IList list4 = jsonData["RankingBonus"];
					for (int m = 0; m < list4.Count; m++)
					{
						COMA_Server_Award.Instance.lst_ranking_awards.Clear();
						JsonData jsonData6 = (JsonData)list4[m];
						UI_OneAwardData.SWorldRankingAward sWorldRankingAward = new UI_OneAwardData.SWorldRankingAward();
						sWorldRankingAward.nAwardNum = int.Parse(jsonData6["cr"].ToString());
						if (sWorldRankingAward.nAwardNum <= 0)
						{
							JsonData jsonData7 = jsonData6["item"];
							sWorldRankingAward.serialName = jsonData7["sn"].ToString();
							sWorldRankingAward.itemName = jsonData7["in"].ToString();
							sWorldRankingAward.part = int.Parse(jsonData7["part"].ToString());
						}
						COMA_Server_Award.Instance.lst_ranking_awards.Add(sWorldRankingAward);
					}
				}
				if (UIRankingList.Instance != null)
				{
					UIRankingList.Instance.InitContainer_djzhu(num2, 0);
				}
				if (COMA_CommonOperation.Instance.IsMode_Fishing(Application.loadedLevelName))
				{
					int iDByName2 = TFishingAddressBook.Instance.GetIDByName(1);
					TMessageDispatcher.Instance.DispatchMsg(-1, iDByName2, 1015, TTelegram.SEND_MSG_IMMEDIATELY, null);
				}
			}
			else if (action == actionName_RankOfFriends)
			{
				string text4 = (string)param;
				int num11 = int.Parse(text4.Substring(9)) - 1;
				IList list5 = jsonData["datas"];
				lst_rankFriends[num11] = new List<UIRankingList_FriendRankingData>();
				for (int n = 0; n < list5.Count; n++)
				{
					JsonData jsonData8 = (JsonData)list5[n];
					string text5 = jsonData8["uuid"].ToString();
					string playerName2 = jsonData8["name"].ToString();
					int scoreNum2 = int.Parse(jsonData8["score"].ToString());
					int rankNum = int.Parse(jsonData8["rank"].ToString());
					UIRankingList_FriendRankingData uIRankingList_FriendRankingData = new UIRankingList_FriendRankingData();
					uIRankingList_FriendRankingData.RankNum = rankNum;
					uIRankingList_FriendRankingData.PlayerName = playerName2;
					uIRankingList_FriendRankingData.ScoreNum = scoreNum2;
					lst_rankFriends[num11].Add(uIRankingList_FriendRankingData);
				}
				if (UIRankingList.Instance != null)
				{
					UIRankingList.Instance.InitContainer_djzhu(num11, 1);
				}
			}
			else if (!(action == actionName_SubmitScore))
			{
			}
		}
	}
}
