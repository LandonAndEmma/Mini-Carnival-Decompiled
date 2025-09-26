using System.Collections.Generic;
using Protocol;
using Protocol.RPG.S2C;
using UnityEngine;

public class UIRPG_DataBufferCenter
{
	private class Nested
	{
		internal static readonly UIRPG_DataBufferCenter _instance;

		static Nested()
		{
			_instance = new UIRPG_DataBufferCenter();
		}
	}

	private Vector3[] _posVector3 = new Vector3[100]
	{
		new Vector3(40f, 30f, -2f),
		new Vector3(52f, 102f, -2f),
		new Vector3(94f, 68f, -2f),
		new Vector3(126f, 26f, -2f),
		new Vector3(134f, 130f, -2f),
		new Vector3(172f, 76f, -2f),
		new Vector3(58f, 170f, -2f),
		new Vector3(210f, 26f, -2f),
		new Vector3(122f, 208f, -2f),
		new Vector3(186f, 170f, -2f),
		new Vector3(236f, 114f, -2f),
		new Vector3(280f, 28f, -2f),
		new Vector3(78f, 264f, -2f),
		new Vector3(162f, 262f, -2f),
		new Vector3(308f, 128f, -2f),
		new Vector3(320f, 80f, -2f),
		new Vector3(122f, 340f, -2f),
		new Vector3(362f, 172f, -2f),
		new Vector3(208f, 332f, -2f),
		new Vector3(368f, 30f, -2f),
		new Vector3(458f, 32f, -2f),
		new Vector3(390f, 230f, -2f),
		new Vector3(404f, 86f, -2f),
		new Vector3(488f, 94f, -2f),
		new Vector3(442f, 164f, -2f),
		new Vector3(566f, 112f, -2f),
		new Vector3(552f, 34f, -2f),
		new Vector3(90f, 406f, -2f),
		new Vector3(516f, 190f, -2f),
		new Vector3(642f, 58f, -2f),
		new Vector3(668f, 128f, -2f),
		new Vector3(158f, 452f, -2f),
		new Vector3(92f, 508f, -2f),
		new Vector3(724f, 34f, -2f),
		new Vector3(748f, 106f, -2f),
		new Vector3(178f, 540f, -2f),
		new Vector3(816f, 48f, -2f),
		new Vector3(216f, 404f, -2f),
		new Vector3(776f, 178f, -2f),
		new Vector3(220f, 248f, -2f),
		new Vector3(260f, 188f, -2f),
		new Vector3(98f, 572f, -2f),
		new Vector3(846f, 124f, -2f),
		new Vector3(694f, 206f, -2f),
		new Vector3(290f, 262f, -2f),
		new Vector3(876f, 46f, -2f),
		new Vector3(750f, 260f, -2f),
		new Vector3(288f, 336f, -2f),
		new Vector3(166f, 612f, -2f),
		new Vector3(610f, 190f, -2f),
		new Vector3(302f, 408f, -2f),
		new Vector3(268f, 476f, -2f),
		new Vector3(910f, 142f, -2f),
		new Vector3(850f, 204f, -2f),
		new Vector3(250f, 600f, -2f),
		new Vector3(372f, 458f, -2f),
		new Vector3(826f, 270f, -2f),
		new Vector3(940f, 86f, -2f),
		new Vector3(288f, 542f, -2f),
		new Vector3(968f, 146f, -2f),
		new Vector3(964f, 210f, -2f),
		new Vector3(378f, 534f, -2f),
		new Vector3(898f, 262f, -2f),
		new Vector3(966f, 310f, -2f),
		new Vector3(338f, 626f, -2f),
		new Vector3(888f, 328f, -2f),
		new Vector3(964f, 390f, -2f),
		new Vector3(428f, 618f, -2f),
		new Vector3(456f, 534f, -2f),
		new Vector3(824f, 354f, -2f),
		new Vector3(896f, 402f, -2f),
		new Vector3(528f, 572f, -2f),
		new Vector3(962f, 480f, -2f),
		new Vector3(808f, 436f, -2f),
		new Vector3(624f, 558f, -2f),
		new Vector3(876f, 498f, -2f),
		new Vector3(688f, 602f, -2f),
		new Vector3(592f, 618f, -2f),
		new Vector3(960f, 548f, -2f),
		new Vector3(702f, 516f, -2f),
		new Vector3(744f, 564f, -2f),
		new Vector3(714f, 428f, -2f),
		new Vector3(752f, 360f, -2f),
		new Vector3(672f, 356f, -2f),
		new Vector3(766f, 508f, -2f),
		new Vector3(688f, 282f, -2f),
		new Vector3(932f, 618f, -2f),
		new Vector3(612f, 286f, -2f),
		new Vector3(542f, 246f, -2f),
		new Vector3(846f, 594f, -2f),
		new Vector3(462f, 264f, -2f),
		new Vector3(382f, 310f, -2f),
		new Vector3(426f, 374f, -2f),
		new Vector3(442f, 454f, -2f),
		new Vector3(524f, 490f, -2f),
		new Vector3(604f, 468f, -2f),
		new Vector3(606f, 366f, -2f),
		new Vector3(528f, 314f, -2f),
		new Vector3(490f, 382f, -2f),
		new Vector3(556f, 434f, -2f)
	};

	public static bool isPreSceneMap = false;

	public static bool _isPreSceneBattle = false;

	public static uint _challangePrePersonLv = UIDataBufferCenter.Instance.RPGData.m_rpg_level;

	public static int _cardMemberNumPre = GetAvailableMemberSlot();

	public static UIRPG_DataBufferCenter Instance
	{
		get
		{
			return Nested._instance;
		}
	}

	public Vector3[] PosVector3
	{
		get
		{
			return _posVector3;
		}
	}

	private UIRPG_DataBufferCenter()
	{
	}

	public static byte GetCardGradeByCardId(int cardId)
	{
		return RPGGlobalData.Instance.CareerUnitPool._dict[cardId].StarGrade;
	}

	public static Color GetAureoleColorByCardGrade(byte grade)
	{
		Color result = Color.red;
		switch (grade)
		{
		case 1:
			result = new Color(0.5019608f, 0.5019608f, 0.5019608f);
			break;
		case 2:
			result = new Color(0f, 0.99215686f, 0.09019608f);
			break;
		case 3:
			result = new Color(0f, 0.4627451f, 1f);
			break;
		case 4:
			result = new Color(0.34901962f, 0f, 0.5019608f);
			break;
		case 5:
			result = new Color(1f, 0.4392157f, 0f);
			break;
		case 6:
			result = new Color(1f, 0f, 0f);
			break;
		}
		return result;
	}

	public static string GetHexColorStringByGrade(int grade)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		switch (grade)
		{
		case 1:
			num = 128;
			num2 = 128;
			num3 = 128;
			break;
		case 2:
			num = 0;
			num2 = 253;
			num3 = 23;
			break;
		case 3:
			num = 0;
			num2 = 118;
			num3 = 255;
			break;
		case 4:
			num = 89;
			num2 = 0;
			num3 = 128;
			break;
		case 5:
			num = 255;
			num2 = 112;
			num3 = 0;
			break;
		case 6:
			num = 255;
			num2 = 0;
			num3 = 0;
			break;
		}
		string text = ((num >= 16) ? num.ToString("X") : ("0" + num.ToString("X")));
		string text2 = ((num2 >= 16) ? num2.ToString("X") : ("0" + num2.ToString("X")));
		string text3 = ((num3 >= 16) ? num3.ToString("X") : ("0" + num3.ToString("X")));
		return text + text2 + text3;
	}

	public static Color GetCardColorByGrade(byte grade)
	{
		Color result = Color.red;
		switch (grade)
		{
		case 1:
			result = new Color(1f, 1f, 1f);
			break;
		case 2:
			result = new Color(40f / 51f, 1f, 0f);
			break;
		case 3:
			result = new Color(0f, 72f / 85f, 1f);
			break;
		case 4:
			result = new Color(59f / 85f, 0.14901961f, 0.8745098f);
			break;
		case 5:
			result = new Color(1f, 0.45490196f, 0f);
			break;
		case 6:
			result = new Color(1f, 6f / 85f, 0f);
			break;
		}
		return result;
	}

	public static int GetGemCompoundValue(int type, int level)
	{
		return RPGGlobalData.Instance.CompoundTableUnitPool._dict[type]._apList[level - 1];
	}

	public static string GetCardCareerNameByCardId(int cardId)
	{
		RPGCareerUnit rPGCareerUnit = RPGGlobalData.Instance.CareerUnitPool._dict[cardId];
		return TUITool.StringFormat(Localization.instance.Get(rPGCareerUnit.CareerName));
	}

	public static string GetCardIconNameByCardId(int cardId)
	{
		string result = "RPG_Small_" + cardId;
		if (cardId >= 500)
		{
			return GetBossIconNameByCardId(cardId);
		}
		return result;
	}

	public static string GetBossIconNameByCardId(int cardId)
	{
		string result = null;
		switch (cardId)
		{
		case 500:
			result = "RPG_Small_" + 5;
			break;
		case 501:
			result = "RPG_Small_" + 3;
			break;
		case 502:
			result = "RPG_Small_" + 11;
			break;
		case 503:
			result = "RPG_Small_" + 13;
			break;
		case 504:
			result = "RPG_Small_" + 14;
			break;
		case 505:
			result = "RPG_Small_" + 18;
			break;
		case 506:
			result = "RPG_Small_" + 17;
			break;
		case 507:
			result = "RPG_Small_" + 25;
			break;
		case 508:
			result = "RPG_Small_" + 26;
			break;
		case 509:
			result = "RPG_Small_" + 16;
			break;
		case 510:
			result = "RPG_Small_" + 28;
			break;
		case 511:
			result = "RPG_Small_" + 36;
			break;
		case 512:
			result = "RPG_Small_" + 37;
			break;
		case 513:
			result = "RPG_Small_" + 39;
			break;
		case 514:
			result = "RPG_Small_" + 41;
			break;
		case 515:
			result = "RPG_Small_" + 42;
			break;
		case 516:
			result = "RPG_Small_" + 43;
			break;
		case 517:
			result = "RPG_Small_" + 44;
			break;
		case 518:
			result = "RPG_Small_" + 47;
			break;
		case 519:
			result = "RPG_Small_" + 48;
			break;
		}
		return result;
	}

	public static string GetCardCareerDesByCardId(int cardId)
	{
		Debug.Log("cardId = " + cardId);
		string result = null;
		int key = RPGGlobalData.Instance.CareerUnitPool._dict[cardId].SkillLst[0];
		string skillDes = RPGGlobalData.Instance.SkillUnitPool._dict[key].SkillDes;
		if (RPGGlobalData.Instance.SkillUnitPool._dict[key].SkillEffectType == RPGSkillUnit.ESkillEffectType.Generate_Tactic)
		{
			int key2 = (int)RPGGlobalData.Instance.SkillUnitPool._dict[key].ParamLst[0];
			object[] args = RPGGlobalData.Instance.TacticUnitPool._dict[key2].ParamLst.ToArray();
			result = TUITool.StringFormat(Localization.instance.Get(skillDes), args);
		}
		else if (RPGGlobalData.Instance.SkillUnitPool._dict[key].ParamLst.Count == 0)
		{
			result = TUITool.StringFormat(Localization.instance.Get(skillDes));
		}
		else
		{
			switch (int.Parse(skillDes.Substring(16)))
			{
			case 3:
			case 4:
			case 5:
			case 6:
			case 8:
			case 10:
			case 12:
			case 15:
			case 18:
				result = TUITool.StringFormat(Localization.instance.Get(skillDes));
				break;
			case 7:
			{
				object obj4 = RPGGlobalData.Instance.SkillUnitPool._dict[key].ParamLst[2];
				result = TUITool.StringFormat(Localization.instance.Get(skillDes), obj4);
				break;
			}
			case 13:
			case 17:
			case 19:
			case 20:
			case 21:
			case 26:
			case 27:
			case 28:
			case 29:
			case 30:
			case 31:
			case 32:
			case 34:
			case 36:
			case 40:
			case 43:
			case 44:
			case 46:
			case 48:
			{
				object obj3 = RPGGlobalData.Instance.SkillUnitPool._dict[key].ParamLst[0];
				result = TUITool.StringFormat(Localization.instance.Get(skillDes), obj3);
				break;
			}
			case 14:
			case 42:
			{
				object obj2 = RPGGlobalData.Instance.SkillUnitPool._dict[key].ParamLst[1];
				result = TUITool.StringFormat(Localization.instance.Get(skillDes), obj2);
				break;
			}
			case 16:
			{
				object[] args2 = RPGGlobalData.Instance.SkillUnitPool._dict[key].ParamLst.ToArray();
				result = TUITool.StringFormat(Localization.instance.Get(skillDes), args2);
				break;
			}
			case 11:
			{
				object obj = RPGGlobalData.Instance.SkillUnitPool._dict[key].ParamLst[0];
				int num = (int)obj;
				num = -num;
				result = TUITool.StringFormat(Localization.instance.Get(skillDes), num);
				break;
			}
			}
		}
		return result;
	}

	public static string GetDesByGemTypeAndLevel(int type, int level)
	{
		int gemCompoundValue = GetGemCompoundValue(type, level);
		string apDes = RPGGlobalData.Instance.CompoundTableUnitPool._dict[type]._apDes;
		return TUITool.StringFormat(Localization.instance.Get(apDes), gemCompoundValue);
	}

	private static float[] GetMainAttr(int cardId, int lv)
	{
		float[] array = new float[3];
		RPGCareerUnit rPGCareerUnit = RPGGlobalData.Instance.CareerUnitPool._dict[cardId];
		array[0] = (float)(4 + lv) * rPGCareerUnit.AttrValue[0];
		array[1] = (float)(4 + lv) * rPGCareerUnit.AttrValue[1];
		array[2] = (float)(4 + lv) * rPGCareerUnit.AttrValue[2];
		return array;
	}

	public static int GetATKGradeByCardId(int cardId, int lv = 1)
	{
		RPGCareerUnit rPGCareerUnit = RPGGlobalData.Instance.CareerUnitPool._dict[cardId];
		float[] mainAttr = GetMainAttr(cardId, lv);
		float[] mainAttr2 = GetMainAttr(cardId, lv + 1);
		float num = 0f;
		for (int i = 0; i < mainAttr.Length; i++)
		{
			if (i != (int)rPGCareerUnit.MainAttr)
			{
				num += mainAttr[i];
			}
		}
		float num2 = 0f;
		for (int j = 0; j < mainAttr2.Length; j++)
		{
			if (j != (int)rPGCareerUnit.MainAttr)
			{
				num2 += mainAttr2[j];
			}
		}
		float num3 = mainAttr[(int)rPGCareerUnit.MainAttr] * 1.5f + num;
		float num4 = mainAttr2[(int)rPGCareerUnit.MainAttr] * 1.5f + num2;
		float num5 = num4 - num3;
		Debug.LogWarning("atk=" + num5);
		if (num5 < 7f)
		{
			return 1;
		}
		if (num5 < 12f)
		{
			return 2;
		}
		if (num5 < 17f)
		{
			return 3;
		}
		if (num5 < 22f)
		{
			return 4;
		}
		return 5;
	}

	public static int GetDEFGradeByCardId(int cardId, int lv = 1)
	{
		RPGCareerUnit rPGCareerUnit = RPGGlobalData.Instance.CareerUnitPool._dict[cardId];
		float[] mainAttr = GetMainAttr(cardId, lv);
		float[] mainAttr2 = GetMainAttr(cardId, lv + 1);
		float num = 35f - 40000f / (1150f + mainAttr[1]) + (35f - 40000f / (1150f + mainAttr[2]));
		float num2 = 35f - 40000f / (1150f + mainAttr2[1]) + (35f - 40000f / (1150f + mainAttr2[2]));
		float num3 = num2 - num;
		Debug.LogWarning("def=" + num3);
		if (num3 < 0.16f)
		{
			return 1;
		}
		if (num3 < 0.26f)
		{
			return 2;
		}
		if (num3 < 0.36f)
		{
			return 3;
		}
		if (num3 < 0.46f)
		{
			return 4;
		}
		return 5;
	}

	public static int GetHPGradeByCardId(int cardId, int lv = 1)
	{
		RPGCareerUnit rPGCareerUnit = RPGGlobalData.Instance.CareerUnitPool._dict[cardId];
		float[] mainAttr = GetMainAttr(cardId, lv);
		float[] mainAttr2 = GetMainAttr(cardId, lv + 1);
		float num = mainAttr[0];
		float num2 = mainAttr2[0];
		float num3 = num2 - num;
		Debug.LogWarning("hp=" + num3);
		if (num3 < 5f)
		{
			return 1;
		}
		if (num3 < 7f)
		{
			return 2;
		}
		if (num3 < 9f)
		{
			return 3;
		}
		if (num3 < 11f)
		{
			return 4;
		}
		return 5;
	}

	public static int GetApValueByLevelAndTeam(int lv, List<int> lst, Dictionary<ulong, Equip> equipBag)
	{
		int num = 0;
		for (int i = 0; i < lst.Count; i++)
		{
			if (lst[i] == 0)
			{
				continue;
			}
			RPGCareerUnit rPGCareerUnit = RPGGlobalData.Instance.CareerUnitPool._dict[lst[i]];
			float[] mainAttr = GetMainAttr(lst[i], lv);
			float hPByRoleAttr = RPGEntity.GetHPByRoleAttr(mainAttr[0], mainAttr[1], mainAttr[2]);
			float num2 = 0f;
			for (int j = 0; j < mainAttr.Length; j++)
			{
				if (j != (int)rPGCareerUnit.MainAttr)
				{
					num2 += mainAttr[j];
				}
			}
			float num3 = mainAttr[(int)rPGCareerUnit.MainAttr] * 1.5f + num2;
			float num4 = 35f - 40000f / (1150f + mainAttr[1]) + (35f - 40000f / (1150f + mainAttr[2]));
			float num5 = rPGCareerUnit.AttrValue[0] + rPGCareerUnit.AttrValue[1] + rPGCareerUnit.AttrValue[2];
			int num6 = Mathf.CeilToInt(num5 * (float)lv + num3 + hPByRoleAttr / num4);
			num += num6;
			Debug.Log(string.Format("i = {0}, ap = {1}", i, num6));
		}
		return num;
	}

	public static int GetApValueByLevelAndTeam(int lv, MemberSlot[] memberSlot)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < memberSlot.Length; i++)
		{
			list.Add((int)memberSlot[i].m_member);
		}
		return GetApValueByLevelAndTeam(lv, list, null);
	}

	public static int GetApValueByLevelAndTeam(int lv, List<int> lst)
	{
		return GetApValueByLevelAndTeam(lv, lst, null);
	}

	public static string GetSmallGemSpriteNameByTypeAndLevel(int type, int level)
	{
		string result = null;
		switch (type)
		{
		case 1:
			result = "gem_red_s" + level;
			break;
		case 2:
			result = "gem_yellow_s" + level;
			break;
		case 3:
			result = "gem_blue_s" + level;
			break;
		case 4:
			result = "gem_p_s" + level;
			break;
		}
		return result;
	}

	public static string GetBigGemSpriteNameByTypeAndLevel(int type, int level)
	{
		string result = null;
		switch (type)
		{
		case 1:
			result = "gem_red" + level;
			break;
		case 2:
			result = "gem_yellow" + level;
			break;
		case 3:
			result = "gem_blue" + level;
			break;
		case 4:
			result = "gem_purple" + level;
			break;
		}
		return result;
	}

	public static int GetCardMemberNum()
	{
		int num = 0;
		for (int i = 0; i < UIDataBufferCenter.Instance.RPGData.m_member_slot.Length; i++)
		{
			if (UIDataBufferCenter.Instance.RPGData.m_member_slot[i].m_member != 0)
			{
				num++;
			}
		}
		return num;
	}

	public static int GetAvailableMemberSlot()
	{
		int num = 3;
		uint lvLimit_TeamPos = (uint)RPGGlobalData.Instance.RpgMiscUnit._lvLimit_TeamPos4;
		uint lvLimit_TeamPos2 = (uint)RPGGlobalData.Instance.RpgMiscUnit._lvLimit_TeamPos5;
		uint lvLimit_TeamPos3 = (uint)RPGGlobalData.Instance.RpgMiscUnit._lvLimit_TeamPos6;
		uint rpg_level = UIDataBufferCenter.Instance.RPGData.m_rpg_level;
		if (rpg_level >= lvLimit_TeamPos)
		{
			num++;
		}
		if (rpg_level >= lvLimit_TeamPos2)
		{
			num++;
		}
		if (rpg_level >= lvLimit_TeamPos3)
		{
			num++;
		}
		return num;
	}

	public static string GetColorStringByRPGAndValue(string rpg, object obj)
	{
		return "[" + rpg + "]" + obj.ToString() + "[-]";
	}

	public static int GetTeamTotalAp(MemberSlot[] memberSlot, int lv, bool isEnemy, uint roleId = 0)
	{
		int num = 0;
		for (int i = 0; i < memberSlot.Length; i++)
		{
			if (memberSlot[i].m_member != 0)
			{
				int[] singleMemberAttr = GetSingleMemberAttr(memberSlot[i], lv, isEnemy, roleId);
				num += singleMemberAttr[10];
			}
		}
		return num;
	}

	public static int[] GetSingleMemberAttr(MemberSlot member, int lv, bool isEnemy, uint roleId = 0)
	{
		int member2 = (int)member.m_member;
		List<ulong> list = new List<ulong>();
		list.Add(member.m_head);
		list.Add(member.m_body);
		list.Add(member.m_leg);
		return GetProperty(member2, lv, list, isEnemy, roleId);
	}

	public static int[] GetProperty(int cardId, int lv, List<ulong> part, bool isEnemy, uint roleId = 0)
	{
		int[] array = new int[12];
		RPGCareerUnit rPGCareerUnit = RPGGlobalData.Instance.CareerUnitPool._dict[cardId];
		array[0] = Mathf.CeilToInt((float)(4 + lv) * rPGCareerUnit.AttrValue[0]);
		array[1] = Mathf.CeilToInt((float)(4 + lv) * rPGCareerUnit.AttrValue[1]);
		array[2] = Mathf.CeilToInt((float)(4 + lv) * rPGCareerUnit.AttrValue[2]);
		int num = 0;
		while (part != null && num < part.Count)
		{
			InitBaseProperty(part[num], array, isEnemy, roleId);
			num++;
		}
		array[3] = Mathf.CeilToInt(RPGEntity.GetHPByRoleAttr(array[0], array[1], array[2]));
		int num2 = 0;
		for (int i = 0; i <= 2; i++)
		{
			if (i != (int)rPGCareerUnit.MainAttr)
			{
				num2 += array[i];
			}
		}
		array[4] = Mathf.CeilToInt((float)array[(int)rPGCareerUnit.MainAttr] * 1.5f + (float)num2);
		array[5] = Mathf.CeilToInt(35f - 40000f / (1150f + (float)array[1]) + (35f - 40000f / (1150f + (float)array[2])));
		array[6] = Mathf.CeilToInt(20f - 40000f / (2000f + (float)array[1]));
		array[7] = 5;
		array[8] = 100;
		array[9] = Mathf.CeilToInt(100f - 40000f / (float)(array[2] + 400) + 150f);
		int num3 = Mathf.CeilToInt(rPGCareerUnit.AttrValue[0] + rPGCareerUnit.AttrValue[1] + rPGCareerUnit.AttrValue[2]);
		int num4 = 0;
		while (part != null && num4 < part.Count)
		{
			InitBaseProperty(part[num4], array, isEnemy, roleId);
			num4++;
		}
		array[10] = Mathf.CeilToInt((float)(num3 * lv + array[4]) + 1f * (float)array[3] * (float)array[5]);
		return array;
	}

	private static void InitBaseProperty(ulong id, int[] ret, bool isEnemy, uint roleId = 0)
	{
		Equip equip = null;
		if (!isEnemy)
		{
			if (UIDataBufferCenter.Instance.RPGData.m_equip_bag.ContainsKey(id))
			{
				equip = UIDataBufferCenter.Instance.RPGData.m_equip_bag[id];
				InitBasePropertyByEquip(equip, ret);
			}
			return;
		}
		UIDataBufferCenter.Instance.FetchPlayerRPGData(roleId, delegate(PlayerRpgDataCmd playData)
		{
			if (playData != null && playData.m_equip_bag.ContainsKey(id))
			{
				equip = playData.m_equip_bag[id];
				InitBasePropertyByEquip(equip, ret);
			}
		});
	}

	private static void InitBasePropertyByEquip(Equip equip, int[] ret)
	{
		RPGGemCompoundTableUnit.EAttrType attrType = RPGGlobalData.Instance.CompoundTableUnitPool._dict[equip.m_type]._attrType;
		int level = equip.m_level;
		int num = RPGGlobalData.Instance.CompoundTableUnitPool._dict[equip.m_type]._apList[level - 1];
		switch (attrType)
		{
		case RPGGemCompoundTableUnit.EAttrType._ATT:
			ret[4] = Mathf.CeilToInt((float)ret[4] * (1f + (float)num / 100f));
			break;
		case RPGGemCompoundTableUnit.EAttrType._DEF:
			ret[5] += num;
			break;
		case RPGGemCompoundTableUnit.EAttrType._DodgeRate:
			ret[7] += num;
			break;
		case RPGGemCompoundTableUnit.EAttrType._CriticalRate:
			ret[6] += num;
			break;
		case RPGGemCompoundTableUnit.EAttrType._ATIndex:
			ret[8] = Mathf.CeilToInt((float)ret[8] * (1f + (float)num / 100f));
			break;
		case RPGGemCompoundTableUnit.EAttrType._HP:
			ret[3] = Mathf.CeilToInt((float)ret[3] * (1f + (float)num / 100f));
			break;
		case RPGGemCompoundTableUnit.EAttrType._CriticalMultiplier:
			ret[9] += num;
			break;
		}
	}

	public static int GetNpcTeamTotalAp(Debug_TeamParam teamParam)
	{
		Debug.Log("public static int GetNpcTeamTotalAp(Debug_TeamParam teamParam)");
		int num = 0;
		int lv = teamParam.lv;
		int[] array = new int[12];
		for (int i = 0; i < teamParam._careerLst.Count; i++)
		{
			int key = teamParam._careerLst[i];
			if (!RPGGlobalData.Instance.CareerUnitPool._dict.ContainsKey(key))
			{
				continue;
			}
			RPGCareerUnit rPGCareerUnit = RPGGlobalData.Instance.CareerUnitPool._dict[key];
			array[0] = Mathf.CeilToInt((float)(4 + lv) * rPGCareerUnit.AttrValue[0]);
			array[1] = Mathf.CeilToInt((float)(4 + lv) * rPGCareerUnit.AttrValue[1]);
			array[2] = Mathf.CeilToInt((float)(4 + lv) * rPGCareerUnit.AttrValue[2]);
			array[3] = Mathf.CeilToInt(RPGEntity.GetHPByRoleAttr(array[0], array[1], array[2]));
			int num2 = 0;
			for (int j = 0; j <= 2; j++)
			{
				if (j != (int)rPGCareerUnit.MainAttr)
				{
					num2 += array[j];
				}
			}
			array[4] = Mathf.CeilToInt((float)array[(int)rPGCareerUnit.MainAttr] * 1.5f + (float)num2);
			array[5] = Mathf.CeilToInt(35f - 40000f / (1150f + (float)array[1]) + (35f - 40000f / (1150f + (float)array[2])));
			array[6] = Mathf.CeilToInt(20f - 40000f / (2000f + (float)array[1]));
			array[7] = 5;
			array[8] = 100;
			array[9] = Mathf.CeilToInt(100f - 40000f / (float)(array[2] + 400) + 150f);
			int num3 = Mathf.CeilToInt(rPGCareerUnit.AttrValue[0] + rPGCareerUnit.AttrValue[1] + rPGCareerUnit.AttrValue[2]);
			GetNpcTeamTotalApForEquip(array, i, teamParam);
			array[10] = Mathf.CeilToInt((float)(num3 * lv + array[4]) + 1f * (float)array[3] * (float)array[5]);
			num += array[10];
		}
		return num;
	}

	private static void GetNpcTeamTotalApForEquip(int[] ret, int i, Debug_TeamParam teamParam)
	{
		ret[4] += Mathf.CeilToInt((float)(ret[4] * Mathf.CeilToInt(teamParam._equipmentLst[i]._ATT._promoteValue)) / 100f);
		ret[5] += Mathf.CeilToInt(teamParam._equipmentLst[i]._DEF._promoteValue);
		ret[7] += Mathf.CeilToInt(teamParam._equipmentLst[i]._DodgeRate._promoteValue);
		ret[6] += Mathf.CeilToInt(teamParam._equipmentLst[i]._CriticalRate._promoteValue);
		ret[8] += Mathf.CeilToInt((float)ret[8] * teamParam._equipmentLst[i]._ATIndex._promoteValue / 100f);
		ret[3] += Mathf.CeilToInt((float)ret[3] * teamParam._equipmentLst[i]._HP._promoteValue / 100f);
		ret[9] += Mathf.CeilToInt(teamParam._equipmentLst[i]._CriticalMultiplier._promoteValue);
	}
}
