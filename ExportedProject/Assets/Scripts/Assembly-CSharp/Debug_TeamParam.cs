using System;
using System.Collections.Generic;

[Serializable]
public class Debug_TeamParam
{
	public int lv;

	public List<int> _careerLst = new List<int>();

	public List<PlayerTransParam> _transParamLst = new List<PlayerTransParam>();

	public List<RPGEquipmentUnit> _equipmentLst = new List<RPGEquipmentUnit>();
}
