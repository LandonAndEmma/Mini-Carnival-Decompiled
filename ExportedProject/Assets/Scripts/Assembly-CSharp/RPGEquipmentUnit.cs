using System;
using UnityEngine;

[Serializable]
public class RPGEquipmentUnit
{
	[SerializeField]
	public EquipAttrPromote _boutLimit_ATT;

	[SerializeField]
	public EquipAttrPromote _boutLimit_DEF;

	[SerializeField]
	public EquipAttrPromote _ATT;

	[SerializeField]
	public EquipAttrPromote _DEF;

	[SerializeField]
	public EquipAttrPromote _DodgeRate;

	[SerializeField]
	public EquipAttrPromote _CriticalRate;

	[SerializeField]
	public EquipAttrPromote _ATIndex;

	[SerializeField]
	public EquipAttrPromote _HP;

	[SerializeField]
	public EquipAttrPromote _CriticalMultiplier;

	[SerializeField]
	public EquipAttrPromote _ITDValue;

	[SerializeField]
	public EquipAttrPromote _FrozenResistRate;

	[SerializeField]
	public EquipAttrPromote _StunResistRate;

	[SerializeField]
	public EquipAttrPromote _SuckHP;

	[SerializeField]
	public EquipAttrPromote _LimitRHP;
}
