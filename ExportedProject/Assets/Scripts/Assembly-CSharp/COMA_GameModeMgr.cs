using UnityEngine;

public class COMA_GameModeMgr : MonoBehaviour
{
	public UIGameRulesMsgBox ruleBox;

	public GameObject btn_Create;

	public GameObject btn_QuichMatch;

	public GameObject btn_Play;

	private string[] _strModeDesc = new string[11]
	{
		"moshitanchukuang_desc15",
		"moshitanchukuang_desc8",
		"moshitanchukuang_desc5",
		"moshitanchukuang_desc9",
		"moshitanchukuang_desc3",
		"moshitanchukuang_desc6",
		"tanchukuang_desc1",
		"yewaiqiangzhan_desc02",
		"tank_description",
		string.Empty,
		string.Empty
	};

	private int[] _nMoney = new int[33]
	{
		0, 300, -3, 0, 200, -3, 0, 200, -3, 0,
		200, -3, 0, 200, -3, 0, 100, 1000, 0, 2000,
		-25, 0, 500, -3, 0, 0, 0, 0, 0, 0,
		0, 0, 0
	};

	private string[] _strTex = new string[33]
	{
		"prop_gold",
		"prop_random",
		"prop_speed",
		"prop_gold",
		"prop_speed",
		"prop_spring",
		"3",
		"1",
		"prop_weapon3",
		"prop_gold",
		"prop_speed",
		"prop_perception",
		"prop_meat",
		"prop_hide",
		"prop_speed",
		"prop_weapon5",
		"prop_weapon4",
		"prop_weapon3",
		"prop_blackpole",
		"prop_silverpole",
		"prop_goldpole",
		"19",
		"21",
		"20",
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty
	};

	private string[] _strDes = new string[33]
	{
		"zhuangbeijiemian_weapon9",
		"zhuangbeijiemian_weapon16",
		"zhuangbeijiemian_weapon17",
		"zhuangbeijiemian_weapon9",
		"zhuangbeijiemian_weapon10",
		"zhuangbeijiemian_weapon15",
		"zhuangbeijiemian_weapon7",
		"zhuangbeijiemian_weapon2",
		"zhuangbeijiemian_weapon8",
		"zhuangbeijiemian_weapon9",
		"zhuangbeijiemian_weapon10",
		"zhuangbeijiemian_weapon11",
		"zhuangbeijiemian_weapon12",
		"zhuangbeijiemian_weapon13",
		"zhuangbeijiemian_weapon14",
		"zhuangbeijiemian_weapon6",
		"zhuangbeijiemian_weapon3",
		"zhuangbeijiemian_weapon8",
		"yuganjiemian1",
		"yuganjiemian2",
		"yuganjiemian3",
		"qiangzhanjiemian_desc05",
		"qiangzhanjiemian_desc07",
		"qiangzhanjiemian_desc06",
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty
	};

	private void Awake()
	{
		TUITextManager.Instance().Parser("UI/language.en", "UI/language.en");
	}

	private void Start()
	{
		int seletectGameModeIndex = COMA_CommonOperation.Instance.seletectGameModeIndex;
		Debug.Log(seletectGameModeIndex);
		if (seletectGameModeIndex == 5 || seletectGameModeIndex == 6)
		{
			btn_Create.SetActive(false);
			btn_QuichMatch.SetActive(false);
		}
		else
		{
			btn_Play.SetActive(false);
		}
		if (seletectGameModeIndex == 6)
		{
			ruleBox.InitTimeLimit(COMA_Fishing.Instance.GetPoleTime(0), COMA_Fishing.Instance.nTime0, 0);
			ruleBox.InitTimeLimit(COMA_Fishing.Instance.GetPoleTime(1), COMA_Fishing.Instance.nTime1, 1);
		}
		string strTheme = TUITextManager.Instance().GetString(_strModeDesc[seletectGameModeIndex]);
		string strParam = COMA_Pref.Instance.GetGold() + "," + COMA_Pref.Instance.GetCrystal();
		ruleBox.MsgBox(strTheme, string.Empty, seletectGameModeIndex, strParam);
		if (seletectGameModeIndex == 8)
		{
			int[] money = new int[5] { 0, 1000, 1000, -3, -3 };
			string[] tex2D = new string[5] { "22", "24", "25", "23", "26" };
			string[] des = new string[5] { "tank_tank01_description", "tank_tank02_description", "tank_tank03_description", "tank_tank04_description", "tank_tank05_description" };
			ruleBox.SetRuleBoxWeaponInfo(seletectGameModeIndex, money, tex2D, des);
		}
		else
		{
			int[] money2 = new int[3]
			{
				_nMoney[seletectGameModeIndex * 3],
				_nMoney[seletectGameModeIndex * 3 + 1],
				_nMoney[seletectGameModeIndex * 3 + 2]
			};
			string[] tex2D2 = new string[3]
			{
				_strTex[seletectGameModeIndex * 3],
				_strTex[seletectGameModeIndex * 3 + 1],
				_strTex[seletectGameModeIndex * 3 + 2]
			};
			string[] des2 = new string[3]
			{
				_strDes[seletectGameModeIndex * 3],
				_strDes[seletectGameModeIndex * 3 + 1],
				_strDes[seletectGameModeIndex * 3 + 2]
			};
			ruleBox.SetRuleBoxWeaponInfo(seletectGameModeIndex, money2, tex2D2, des2);
		}
	}
}
