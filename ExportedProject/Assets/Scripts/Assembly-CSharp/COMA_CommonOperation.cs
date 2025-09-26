using System.Collections.Generic;
using MessageID;
using Protocol.Role.C2S;
using UnityEngine;

public class COMA_CommonOperation
{
	private static COMA_CommonOperation _instance;

	private bool bInitPlayerTexture;

	public bool bSelectModeLock;

	public int seletectGameModeIndex;

	public int selectedWeaponIndex;

	public bool selectedWeaponValidity = true;

	public int selectedWeaponPrice;

	public bool isCreateRoom;

	public bool isEnterWithG;

	public bool bNeedFBFeed;

	public List<int> otherRankScores = new List<int>();

	public string defaultInput = string.Empty;

	public InputBoardCategory inputKind;

	public bool bfriendRequireInverval;

	public string ValueLock_CrazyRocket_Rocket = "CR_R";

	public string ValueLock_Maze_Pumpkin = "Mz_P";

	public string ValueLock_Flag_Flag = "Fg_F";

	public string ValueLock_Hunger_Item = string.Empty;

	public string ValueLock_Blood_Cannon = "Bd_C";

	public string ValueLock_Fishing_Boat1 = "F_B1";

	public string ValueLock_Fishing_Boat2 = "F_B2";

	public static COMA_CommonOperation Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new COMA_CommonOperation();
			}
			return _instance;
		}
	}

	public static void ResetInstance()
	{
		_instance = null;
	}

	public int GetRankScoreAverage()
	{
		if (otherRankScores.Count <= 0)
		{
			return 0;
		}
		int num = 0;
		foreach (int otherRankScore in otherRankScores)
		{
			num += otherRankScore;
		}
		return num / otherRankScores.Count;
	}

	public void EnterMode(string sceneName)
	{
		Debug.LogWarning("GameModeEnter : " + sceneName);
		int num = SceneNameToID(sceneName);
		if (num >= 0)
		{
			ModifyModelNumCmd modifyModelNumCmd = new ModifyModelNumCmd();
			modifyModelNumCmd.m_mode_name = num.ToString();
			modifyModelNumCmd.m_op = 0;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, modifyModelNumCmd);
		}
	}

	public void ExitMode(string sceneName)
	{
		Debug.LogWarning("GameModeExit : " + sceneName);
		int num = SceneNameToID(sceneName);
		if (num >= 0)
		{
			ModifyModelNumCmd modifyModelNumCmd = new ModifyModelNumCmd();
			modifyModelNumCmd.m_mode_name = num.ToString();
			modifyModelNumCmd.m_op = 1;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, modifyModelNumCmd);
		}
	}

	public bool IsWaittingRoom(string sceneName)
	{
		if (sceneName == "COMA_Scene_WaitingRoom")
		{
			return true;
		}
		return false;
	}

	public bool IsMode_Run(string sceneName)
	{
		if (sceneName == "COMA_Scene_Run")
		{
			return true;
		}
		return false;
	}

	public bool IsMode_Rocket(string sceneName)
	{
		if (sceneName == "COMA_Scene_Rocket")
		{
			return true;
		}
		return false;
	}

	public bool IsMode_Flag(string sceneName)
	{
		if (sceneName == "COMA_Scene_Flag")
		{
			return true;
		}
		return false;
	}

	public bool IsMode_Maze(string sceneName)
	{
		if (sceneName == "COMA_Scene_Maze")
		{
			return true;
		}
		return false;
	}

	public bool IsMode_Hunger(string sceneName)
	{
		if (sceneName == "COMA_Scene_Hunger")
		{
			return true;
		}
		return false;
	}

	public bool IsMode_Castle(string sceneName)
	{
		if (sceneName == "COMA_Scene_Castle")
		{
			return true;
		}
		return false;
	}

	public bool IsMode_Fishing(string sceneName)
	{
		if (sceneName == "COMA_Scene_Fishing")
		{
			return true;
		}
		return false;
	}

	public bool IsMode_Blood(string sceneName)
	{
		if (sceneName == "COMA_Scene_Blood")
		{
			return true;
		}
		return false;
	}

	public bool IsMode_Tank(string scenename)
	{
		return scenename == "COMA_Scene_Tank";
	}

	public bool IsMode_BlackHouse(string sceneName)
	{
		if (sceneName == "COMA_Scene_BlackHouse")
		{
			return true;
		}
		return false;
	}

	public bool IsMode_Flappy(string sceneName)
	{
		if (sceneName == "COMA_Scene_Flappy")
		{
			return true;
		}
		return false;
	}

	public bool IsMode_Network(string sceneName)
	{
		if (IsMode_Run(sceneName) || IsMode_Rocket(sceneName) || IsMode_Flag(sceneName) || IsMode_Maze(sceneName) || IsMode_Hunger(sceneName) || IsMode_Fishing(sceneName) || IsMode_Blood(sceneName) || IsMode_Tank(sceneName) || IsMode_BlackHouse(sceneName))
		{
			return true;
		}
		return false;
	}

	public bool CanBeInvited(string sceneName)
	{
		if (IsMode_Run(sceneName) || IsMode_Rocket(sceneName) || IsMode_Flag(sceneName) || IsMode_Maze(sceneName) || IsMode_Hunger(sceneName) || IsMode_Castle(sceneName) || IsMode_Fishing(sceneName) || IsMode_Blood(sceneName) || IsMode_Tank(sceneName) || IsMode_BlackHouse(sceneName) || IsMode_Flappy(sceneName))
		{
			return false;
		}
		if (IsWaittingRoom(sceneName))
		{
			return false;
		}
		return true;
	}

	public int SceneNameToID(string sceneName)
	{
		switch (sceneName)
		{
		case "COMA_Scene_Run":
			return 0;
		case "COMA_Scene_Rocket":
			return 1;
		case "COMA_Scene_Flag":
			return 2;
		case "COMA_Scene_Maze":
			return 3;
		case "COMA_Scene_Hunger":
			return 4;
		case "COMA_Scene_Castle":
			return 5;
		case "COMA_Scene_Fishing":
			return 6;
		case "COMA_Scene_Blood":
			return 7;
		case "COMA_Scene_Tank":
			return 8;
		case "COMA_Scene_BlackHouse":
			return 9;
		case "COMA_Scene_Flappy":
			return 10;
		default:
			return -1;
		}
	}

	public string SceneIDToName(int sceneID)
	{
		switch (sceneID)
		{
		case 0:
			return "COMA_Scene_Run";
		case 1:
			return "COMA_Scene_Rocket";
		case 2:
			return "COMA_Scene_Flag";
		case 3:
			return "COMA_Scene_Maze";
		case 4:
			return "COMA_Scene_Hunger";
		case 5:
			return "COMA_Scene_Castle";
		case 6:
			return "COMA_Scene_Fishing";
		case 7:
			return "COMA_Scene_Blood";
		case 8:
			return "COMA_Scene_Tank";
		case 9:
			return "COMA_Scene_BlackHouse";
		case 10:
			return "COMA_Scene_Flappy";
		default:
			return string.Empty;
		}
	}

	public int SceneNameToPlayerCount(string sceneName)
	{
		switch (sceneName)
		{
		case "COMA_Scene_Fishing":
			return 12;
		case "COMA_Scene_Hunger":
			return 8;
		case "COMA_Scene_Blood":
			return 8;
		case "COMA_Scene_BlackHouse":
			return 6;
		default:
			return 4;
		}
	}

	public string SceneNameToRankID()
	{
		switch (Application.loadedLevelName)
		{
		case "COMA_Scene_Run":
			return "ComAvatar001";
		case "COMA_Scene_Rocket":
			return "ComAvatar002";
		case "COMA_Scene_Flag":
			return "ComAvatar003";
		case "COMA_Scene_Maze":
			return "ComAvatar004";
		case "COMA_Scene_Hunger":
			return "ComAvatar005";
		case "COMA_Scene_Castle":
			return "ComAvatar006";
		case "COMA_Scene_Fishing":
			return "ComAvatar007";
		case "COMA_Scene_Blood":
			return "ComAvatar008";
		case "COMA_Scene_Tank":
			return "ComAvatar009";
		case "COMA_Scene_BlackHouse":
			return "ComAvatar010";
		case "COMA_Scene_Flappy":
			return "ComAvatar011";
		default:
			return string.Empty;
		}
	}

	public string SceneIDToRankID(int sceneID)
	{
		switch (sceneID)
		{
		case 0:
			return "ComAvatar001";
		case 1:
			return "ComAvatar002";
		case 2:
			return "ComAvatar003";
		case 3:
			return "ComAvatar004";
		case 4:
			return "ComAvatar005";
		case 5:
			return "ComAvatar006";
		case 6:
			return "ComAvatar007";
		case 7:
			return "ComAvatar008";
		case 8:
			return "ComAvatar009";
		case 9:
			return "ComAvatar010";
		case 10:
			return "ComAvatar011";
		case 901:
			return "ComAvatar901";
		default:
			Debug.LogError(sceneID);
			return string.Empty;
		}
	}

	public int RankIDToSceneID(string rankID)
	{
		switch (rankID)
		{
		case "ComAvatar001":
			return 0;
		case "ComAvatar002":
			return 1;
		case "ComAvatar003":
			return 2;
		case "ComAvatar004":
			return 3;
		case "ComAvatar005":
			return 4;
		case "ComAvatar006":
			return 5;
		case "ComAvatar007":
			return 6;
		case "ComAvatar008":
			return 7;
		case "ComAvatar009":
			return 8;
		case "ComAvatar010":
			return 9;
		case "ComAvatar011":
			return 10;
		case "ComAvatar901":
			return 901;
		default:
			Debug.LogError(rankID);
			return -1;
		}
	}

	public int GetGroupID(string sceneName)
	{
		switch (sceneName)
		{
		case "COMA_Scene_Castle":
			return 7004;
		case "COMA_Scene_BlackHouse":
			return 7025;
		case "COMA_Scene_Flag":
			return 7068;
		case "COMA_Scene_Blood":
			return 7069;
		case "COMA_Scene_Hunger":
			return 7070;
		case "COMA_Scene_Fishing":
			return 7071;
		case "COMA_Scene_Maze":
			return 7072;
		case "COMA_Scene_Rocket":
			return 7073;
		case "COMA_Scene_Run":
			return 7074;
		case "COMA_Scene_Flappy":
			return 7075;
		case "COMA_Scene_Tank":
			return 7076;
		default:
			Debug.LogError("No ID!!!");
			return -1;
		}
	}

	public int CalExp(string sceneName, int rank, int score, int lv)
	{
		int num = 0;
		switch (sceneName)
		{
		case "COMA_Scene_Castle":
			num = ((score > 300) ? ((score > 500) ? ((score > 700) ? ((score > 1000) ? Mathf.Min(1500, 500 + (score - 1000) / 50 * 20) : 500) : 350) : 250) : 150);
			break;
		case "COMA_Scene_Hunger":
		{
			int[] array5 = new int[8] { 1500, 1000, 700, 500, 200, 200, 200, 200 };
			if (rank >= 0 && rank < 8)
			{
				num = array5[rank] + score * 55 + lv * 5;
			}
			break;
		}
		case "COMA_Scene_Rocket":
		{
			int[] array4 = new int[4] { 600, 400, 250, 200 };
			num = array4[rank] + lv * 5;
			break;
		}
		case "COMA_Scene_Flag":
		{
			int[] array3 = new int[4] { 500, 300, 200, 150 };
			num = array3[rank] + score * 33 + lv * 5;
			break;
		}
		case "COMA_Scene_Maze":
		{
			int[] array2 = new int[4] { 500, 300, 200, 150 };
			num = array2[rank] + lv * 5;
			break;
		}
		case "COMA_Scene_Run":
		{
			int[] array = new int[4] { 550, 300, 200, 150 };
			num = array[rank] + lv * 5;
			break;
		}
		}
		if (Instance.isCreateRoom)
		{
			num = Mathf.FloorToInt((float)num * 0.2f);
		}
		return num;
	}

	public int CalGold(string sceneName, int rank, int score, int goldGet, int lv)
	{
		int num = 0;
		switch (sceneName)
		{
		case "COMA_Scene_Castle":
			num = ((score > 300) ? ((score > 500) ? ((score > 700) ? ((score > 1000) ? Mathf.Min(150, 100 + (score - 1000) / 50 * 10) : 100) : 75) : 50) : 25);
			break;
		case "COMA_Scene_Hunger":
		{
			int[] array5 = new int[8] { 600, 400, 300, 200, 100, 100, 100, 100 };
			num = array5[rank] + score * 13 + lv * 3;
			break;
		}
		case "COMA_Scene_Rocket":
		{
			int[] array4 = new int[4] { 200, 120, 80, 50 };
			num = array4[rank] + goldGet * 5 + lv * 3;
			break;
		}
		case "COMA_Scene_Flag":
		{
			int[] array3 = new int[4] { 200, 120, 80, 50 };
			num = array3[rank] + score * 7 + lv * 3;
			break;
		}
		case "COMA_Scene_Maze":
		{
			int[] array2 = new int[4] { 180, 100, 80, 50 };
			num = array2[rank] + score + lv * 3;
			break;
		}
		case "COMA_Scene_Run":
		{
			int[] array = new int[4] { 180, 120, 70, 50 };
			num = array[rank] + goldGet + lv * 3;
			break;
		}
		}
		if (Instance.isCreateRoom)
		{
			num = Mathf.FloorToInt((float)num * 0.2f);
		}
		return num;
	}

	public int CalScore(string sceneName, int rank, int n, int m)
	{
		if (Instance.isCreateRoom)
		{
			return 0;
		}
		int result = 0;
		if (m < 2000 && ((sceneName == "COMA_Scene_Hunger" && rank > 3) || rank > 1))
		{
			return -(int)((float)m / 20f);
		}
		float num = 0f;
		switch (sceneName)
		{
		case "COMA_Scene_Castle":
			return result;
		case "COMA_Scene_Hunger":
		{
			float[] array5 = new float[8] { 1f, 0.75f, 0.75f, 0.5f, 0.5f, 0.25f, 0.25f, 0f };
			num = array5[rank];
			break;
		}
		case "COMA_Scene_Rocket":
		{
			float[] array4 = new float[4] { 1f, 0.9f, 0.5f, 0f };
			num = array4[rank];
			break;
		}
		case "COMA_Scene_Flag":
		{
			float[] array3 = new float[4] { 1f, 0.9f, 0.5f, 0f };
			num = array3[rank];
			break;
		}
		case "COMA_Scene_Maze":
		{
			float[] array2 = new float[4] { 1f, 0.9f, 0.5f, 0f };
			num = array2[rank];
			break;
		}
		case "COMA_Scene_Run":
		{
			float[] array = new float[4] { 1f, 0.9f, 0.5f, 0f };
			num = array[rank];
			break;
		}
		}
		float p = (float)(n - m) / 400f;
		p = Mathf.Pow(10f, p) + 1f;
		p = 1f / p;
		int num2 = 210;
		if (m >= 10000)
		{
			num2 = 10;
		}
		else if (m >= 2000)
		{
			num2 = 260 - m / 40;
		}
		result = Mathf.FloorToInt((float)num2 * (num - p));
		Debug.Log("R : " + result);
		if (result == 0)
		{
			result = 1;
		}
		if (result > 203)
		{
			result = 203;
		}
		return result;
	}

	public bool IsPlayingMultiMode()
	{
		if (Application.loadedLevelName == "COMA_Scene_Run" || Application.loadedLevelName == "COMA_Scene_Rocket" || Application.loadedLevelName == "COMA_Scene_Maze" || Application.loadedLevelName == "COMA_Scene_Flag" || Application.loadedLevelName == "COMA_Scene_Hunger" || Application.loadedLevelName == "COMA_Scene_Blood" || Application.loadedLevelName == "COMA_Scene_Tank" || Application.loadedLevelName == "COMA_Scene_BlackHouse")
		{
			return true;
		}
		return false;
	}

	public void GamePause(bool pause)
	{
		if (pause)
		{
			Time.timeScale = 0.001f;
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	public string GIDForShow(string GID)
	{
		return GID.Trim();
	}

	public string GIDFormat(string shortGID)
	{
		return shortGID.PadLeft(15);
	}
}
