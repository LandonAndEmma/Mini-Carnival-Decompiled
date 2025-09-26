using UnityEngine;

public class RPGMiscUnit
{
	[SerializeField]
	public int _energyRenewTimePerUnit = 5;

	[SerializeField]
	public int _energyValue_Max = 5;

	[SerializeField]
	public int _energyRenewPricePerMinute = 1;

	[SerializeField]
	public int _victoryAward_NPC = 1;

	[SerializeField]
	public int _failureAward_NPC = -1;

	[SerializeField]
	public int _victoryAward_BOSS = 1;

	[SerializeField]
	public int _failureAward_BOSS = -1;

	[SerializeField]
	public int _victoryAward_Player = 1;

	[SerializeField]
	public int _failureAward_Player = -1;

	[SerializeField]
	public int _couponCount_extractCard = 5;

	[SerializeField]
	public int _cardNum_extractCard = 5;

	[SerializeField]
	public int _cardNum_donate = 5;

	[SerializeField]
	public int _couponCount_maxPerDay = 10;

	[SerializeField]
	public int _couponPrice_five = 50;

	[SerializeField]
	public int _couponPrice_fifty = 400;

	[SerializeField]
	public int _originalCapacity_CardBag = 20;

	[SerializeField]
	public int _maxCapacity_CardBag = 100;

	[SerializeField]
	public int _unitCardBagPrice = 10;

	[SerializeField]
	public int _lvLimit_TeamPos4 = 15;

	[SerializeField]
	public int _lvLimit_TeamPos5 = 100;

	[SerializeField]
	public int _lvLimit_TeamPos6 = 700;

	[SerializeField]
	public int _gemCompoundConsumeCount = 4;

	[SerializeField]
	public int _originalCapacity_GemBag = 20;

	[SerializeField]
	public int _maxCapacity_GemBag = 100;

	[SerializeField]
	public int _extendGemBagCount_Once = 10;

	[SerializeField]
	public int _extendGemBagPrice_Once = 50;

	[SerializeField]
	public int _originalCapacity_RPGAvatarBag = 20;

	[SerializeField]
	public int _maxCapacity_RPGAvatarBag = 100;

	[SerializeField]
	public int _unitRPGAvatarBagPrice = 10;

	[SerializeField]
	public int _cancelReinforcePrice_FirstTime = 1000;

	[SerializeField]
	public int _cancelReinforcePrice_Mul = 2;

	[SerializeField]
	public int _dropGemCount_PerDay = 10;

	[SerializeField]
	public int _intervalTime_DonateEnergy = 24;

	[SerializeField]
	public int _intervalTime_DonateCoupon = 24;

	[SerializeField]
	public int _useTime_FriendCaptain = 10;

	[SerializeField]
	public int _useIntervalTime_FriendCaptain = 24;

	[SerializeField]
	public int _worldRaningCount = 50;

	[SerializeField]
	public int _EnergyMailNum_PerDay = 5;

	[SerializeField]
	public int _couponMailNum_PerDay = 10;

	[SerializeField]
	public int _intervalTime_ChangePlayerLevel = 48;

	[SerializeField]
	public int _levelOccupyByPlayer = 1;

	[SerializeField]
	public int _levelDefeatNPC = 1;

	[SerializeField]
	public int _levelDefeatPlayer = 1;

	[SerializeField]
	public int _levelExceedPCT = 10;

	[SerializeField]
	public int _levelDefeatPlayer_ExceedPCT = 2;

	[SerializeField]
	public float _occupyParam1 = 0.7f;

	[SerializeField]
	public float _occupyParam2 = 0.25f;

	[SerializeField]
	public float _occupyParam3 = 0.5f;

	[SerializeField]
	public float _occupyParam4 = 0.75f;

	[SerializeField]
	public float _occupyParam5 = 1f;

	[SerializeField]
	public float _occupyParam2_1 = 1f;

	[SerializeField]
	public float _occupyParam3_1 = 2f;

	[SerializeField]
	public float _occupyParam4_1 = 4f;

	[SerializeField]
	public float _occupyParam5_1 = 8f;

	[SerializeField]
	public int _occupyParam6 = 48;

	[SerializeField]
	public float _produceGoldInterTime = 2f;
}
