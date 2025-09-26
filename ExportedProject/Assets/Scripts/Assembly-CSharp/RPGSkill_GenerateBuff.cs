using UnityEngine;

public class RPGSkill_GenerateBuff : RPGTSkill
{
	protected RPGTBuff GenerateBuff(GameObject obj, string buffName, int secondType, bool isOverlap)
	{
		RPGTBuff rPGTBuff = obj.AddComponent(buffName) as RPGTBuff;
		int num = 0;
		int num2 = (int)base.SkillUnit.ParamLst[1];
		while (num2 > 0)
		{
			if ((num2 & 1) == 1)
			{
				rPGTBuff.OffsetAttr.Attrs[num] = (int)base.SkillUnit.ParamLst[2];
			}
			num2 >>= 1;
			num++;
		}
		rPGTBuff.SecondType = secondType;
		rPGTBuff.IsOverlap = isOverlap;
		return rPGTBuff;
	}
}
