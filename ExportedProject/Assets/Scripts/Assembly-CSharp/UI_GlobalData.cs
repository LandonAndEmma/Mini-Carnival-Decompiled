public class UI_GlobalData
{
	public static readonly UI_GlobalData Instance = new UI_GlobalData();

	public static string[] _strACCaption = new string[35]
	{
		"biaoti1_chengjiujiemian", "biaoti2_chengjiujiemian", "biaoti3_chengjiujiemian", "biaoti4_chengjiujiemian", "biaoti5_chengjiujiemian", "biaoti6_chengjiujiemian", "biaoti7_chengjiujiemian", "biaoti8_chengjiujiemian", "biaoti9_chengjiujiemian", "biaoti10_chengjiujiemian",
		"biaoti11_chengjiujiemian", "biaoti12_chengjiujiemian", "biaoti13_chengjiujiemian", "biaoti14_chengjiujiemian", "biaoti15_chengjiujiemian", "biaoti16_chengjiujiemian", "biaoti17_chengjiujiemian", "biaoti18_chengjiujiemian", "biaoti19_chengjiujiemian", "biaoti20_chengjiujiemian",
		"biaoti21_chengjiujiemian", "biaoti22_chengjiujiemian", "biaoti23_chengjiujiemian", "biaoti24_chengjiujiemian", "biaoti25_chengjiujiemian", "biaoti26_chengjiujiemian", "biaoti27_chengjiujiemian", "biaoti28_chengjiujiemian", "biaoti29_chengjiujiemian", "biaoti30_chengjiujiemian",
		"biaoti31_chengjiujiemian", "biaoti32_chengjiujiemian", "biaoti33_chengjiujiemian", "biaoti34_chengjiujiemian", "biaoti35_chengjiujiemian"
	};

	public static string[] _strACCaptionDetail = new string[35]
	{
		"chengjiumiaoshu1_chengjiujiemian", "chengjiumiaoshu2_chengjiujiemian", "chengjiumiaoshu3_chengjiujiemian", "chengjiumiaoshu4_chengjiujiemian", "chengjiumiaoshu5_chengjiujiemian", "chengjiumiaoshu6_chengjiujiemian", "chengjiumiaoshu7_chengjiujiemian", "chengjiumiaoshu8_chengjiujiemian", "chengjiumiaoshu9_chengjiujiemian", "chengjiumiaoshu10_chengjiujiemian",
		"chengjiumiaoshu11_chengjiujiemian", "chengjiumiaoshu12_chengjiujiemian", "chengjiumiaoshu13_chengjiujiemian", "chengjiumiaoshu14_chengjiujiemian", "chengjiumiaoshu15_chengjiujiemian", "chengjiumiaoshu16_chengjiujiemian", "chengjiumiaoshu17_chengjiujiemian", "chengjiumiaoshu18_chengjiujiemian", "chengjiumiaoshu19_chengjiujiemian", "chengjiumiaoshu20_chengjiujiemian",
		"chengjiumiaoshu21_chengjiujiemian", "chengjiumiaoshu22_chengjiujiemian", "chengjiumiaoshu23_chengjiujiemian", "chengjiumiaoshu24_chengjiujiemian", "chengjiumiaoshu25_chengjiujiemian", "chengjiumiaoshu26_chengjiujiemian", "chengjiumiaoshu27_chengjiujiemian", "chengjiumiaoshu28_chengjiujiemian", "chengjiumiaoshu29_chengjiujiemian", "chengjiumiaoshu30_chengjiujiemian",
		"chengjiumiaoshu31_chengjiujiemian", "chengjiumiaoshu32_chengjiujiemian", "chengjiumiaoshu33_chengjiujiemian", "chengjiumiaoshu34_chengjiujiemian", "chengjiumiaoshu35_chengjiujiemian"
	};

	public static string[] _strACIcons = new string[36]
	{
		"AC_0", "AC_1", "AC_2", "AC_3", "AC_4", "AC_5", "AC_6", "AC_7", "AC_8", "AC_9",
		"AC_10", "AC_11", "AC_12", "AC_13", "AC_14", "AC_15", "AC_16", "AC_17", "AC_18", "AC_19",
		"AC_20", "AC_21", "AC_22", "AC_23", "AC_24", "AC_25", "AC_26", "AC_27", "AC_28", "AC_29",
		"AC_30", "AC_31", "AC_32", "AC_33", "AC_34", "AC_35"
	};

	public static string[] _strACIconsBk = new string[36]
	{
		string.Empty,
		"ac_gem",
		"ac_crown",
		"ac_gold",
		"ac_white_wing",
		"ac_straw_hat",
		"ac_devil_horns",
		"ac_gem",
		"ac_gem",
		"ac_gold",
		"ac_gold",
		"ac_gold",
		"ac_gold",
		"ac_gold",
		"ac_gold",
		"ac_hat",
		"ac_pen",
		"ac_lamp",
		"ac_gold",
		"ac_fisherman_sunhat",
		"ac_fisherman_pack",
		"ac_gem",
		"ac_fisherman_goldpole",
		"ac_gold",
		"ac_corolla",
		"ac_p90",
		"ac_m413",
		"ac_jujiqiang",
		"ac_huojianbeibao",
		"ac_huojian",
		"ac_gold",
		"ac_gem",
		"ac_gem",
		"ac_gold",
		"ac_gem",
		"ac_gem"
	};

	private int _nCanGetNum;

	public string[] _strModeID = new string[11]
	{
		"paoku_zhujiemian", "chuansongdaimoshi_zhujiemian", "duoqimoshi_zhujiemian", "migong_zhujiemian", "jiemoshi_zhujiemian", "jiangshiweichengmoshi_zhujiemian", "diaoyumoshi_zhujiemian", "qiangzhanmoshi_zhujiemian", "tanke_zhujiemian", "feiyangdehuojian_zhujiemian",
		"feiyangdehuojian_zhujiemian"
	};

	public int CanGetACNum
	{
		get
		{
			return _nCanGetNum;
		}
		set
		{
			_nCanGetNum = value;
		}
	}

	private UI_GlobalData()
	{
		CanGetACNum = 0;
	}
}
