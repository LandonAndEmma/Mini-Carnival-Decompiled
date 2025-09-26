using System;
using UnityEngine;

public class RPGGlobalFun
{
	public static RPGTSkill EquipSkillById(int skillId, RPGEntity entity)
	{
		RPGTSkill rPGTSkill = null;
		string text = "RPGSkill_" + skillId;
		Debug.Log("EquipSkillById:" + text);
		Type type = Type.GetType(text, true, true);
		if (type == null)
		{
			return null;
		}
		return entity.gameObject.AddComponent(text) as RPGTSkill;
	}

	public static RPGTTactic EquipTacticById(int tacticId, RPGTeam team)
	{
		RPGTTactic rPGTTactic = null;
		string text = "RPGTactic_" + tacticId;
		Debug.Log("EquipTacticById:" + text);
		Type type = Type.GetType(text, true, true);
		if (type == null)
		{
			return null;
		}
		return team.gameObject.AddComponent(text) as RPGTTactic;
	}
}
