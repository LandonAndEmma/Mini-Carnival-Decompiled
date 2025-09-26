using MessageID;
using Protocol.Binary;
using Protocol.Rank.S2C;
using UnityEngine;

public class UILobby_RankProtocolProcessor : UILobbyMessageHandler
{
	protected override void Load()
	{
		UILobbySession component = base.transform.root.GetComponent<UILobbySession>();
		if (component != null)
		{
			component.RegisterHanlder(5, this);
			OnMessage(1, OnUpdateScoreResult);
			OnMessage(3, OnGetMyScoreResult);
			OnMessage(5, OnGetRankListResult);
			OnMessage(7, OnGetFriendScoreListResult);
		}
	}

	protected override void UnLoad()
	{
		UILobbySession component = base.transform.root.GetComponent<UILobbySession>();
		if (component != null)
		{
			component.UnregisterHanlder(5, this);
		}
	}

	private bool OnUpdateScoreResult(UnPacker unpacker)
	{
		UpdateScoreResultCmd updateScoreResultCmd = new UpdateScoreResultCmd();
		if (!updateScoreResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		if (updateScoreResultCmd.m_result == 0)
		{
			Debug.Log("Upload score finish.");
		}
		else
		{
			Debug.LogError("Error!");
		}
		return true;
	}

	private bool OnGetMyScoreResult(UnPacker unpacker)
	{
		GetScoreResultCmd getScoreResultCmd = new GetScoreResultCmd();
		if (!getScoreResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		if (getScoreResultCmd.m_result != 0)
		{
			if (getScoreResultCmd.m_result == 2)
			{
				Debug.Log("自己分数：当前没有分数");
				getScoreResultCmd.m_pos = 1000u;
				getScoreResultCmd.m_score = 0u;
			}
			else if (getScoreResultCmd.m_result == 1)
			{
				Debug.Log("自己分数：有分数无名次");
				getScoreResultCmd.m_pos = 1000u;
			}
			else
			{
				Debug.LogError("Error!");
			}
		}
		string rank_name = getScoreResultCmd.m_rank_name;
		if (rank_name.StartsWith("ComAvatar011"))
		{
			COMA_Pref.Instance.mode_flappy_weekScore = (int)getScoreResultCmd.m_score;
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_SetData, this, getScoreResultCmd);
		return true;
	}

	private bool OnGetRankListResult(UnPacker unpacker)
	{
		GetRankListResultCmd getRankListResultCmd = new GetRankListResultCmd();
		if (!getRankListResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log(getRankListResultCmd.m_rank_name + " " + getRankListResultCmd.m_list.Count);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_SetData, this, getRankListResultCmd);
		return true;
	}

	private bool OnGetFriendScoreListResult(UnPacker unpacker)
	{
		GetFriendListScoreResultCmd getFriendListScoreResultCmd = new GetFriendListScoreResultCmd();
		if (!getFriendListScoreResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log(getFriendListScoreResultCmd.m_rank_name + " " + getFriendListScoreResultCmd.m_list_num + " " + getFriendListScoreResultCmd.m_list.Count);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_SetData, this, getFriendListScoreResultCmd);
		return true;
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}
}
