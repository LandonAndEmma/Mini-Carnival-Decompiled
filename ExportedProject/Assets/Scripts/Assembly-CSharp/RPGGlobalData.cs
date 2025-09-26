using System.Collections.Generic;

public class RPGGlobalData
{
	public static readonly RPGGlobalData Instance = new RPGGlobalData();

	private RPGCareerUnitPool _careerUnitPool = new RPGCareerUnitPool();

	private RPGTacticUnitPool _tacticUnitPool = new RPGTacticUnitPool();

	private RPGSkillUnitPool _skillUnitPool = new RPGSkillUnitPool();

	private RPGCareerAnimationUnitPool _careerAniPool = new RPGCareerAnimationUnitPool();

	private RPGAttackEffectUnitPool _attackEffectPool = new RPGAttackEffectUnitPool();

	private RPGBeAttackEffectUnitPool _beAttackEffectPool = new RPGBeAttackEffectUnitPool();

	private RPGBuffEffectUnitPool _buffEffectPool = new RPGBuffEffectUnitPool();

	private RPGGemDefineUnitPool _gemDefineUnitPool = new RPGGemDefineUnitPool();

	private RPGCompoundFeeUnit _compoundFeePool = new RPGCompoundFeeUnit();

	private RPGCompoundTableUnitPool _compoundTableUnitPool = new RPGCompoundTableUnitPool();

	private RPGMiscUnit _rpgMiscUnit = new RPGMiscUnit();

	private RPGGemShopUnitPool _rpgGemShop = new RPGGemShopUnitPool();

	private RPGMapLayoutUnitPool _mapLayout = new RPGMapLayoutUnitPool();

	private RPGSpeciaLevelDropPool _specLevelDrop = new RPGSpeciaLevelDropPool();

	private RPGLevel_SceneUnitPool _level_Scene = new RPGLevel_SceneUnitPool();

	private RPGGemDropUnitPool _gem_drop = new RPGGemDropUnitPool();

	private List<OccupyPointIncome> _lstOccupyPointIncome = new List<OccupyPointIncome>();

	private List<LevelIncome> _lstLevelIncome = new List<LevelIncome>();

	private List<RPGMaxExp> _lstRPGMaxExp = new List<RPGMaxExp>();

	public RPGCareerUnitPool CareerUnitPool
	{
		get
		{
			return _careerUnitPool;
		}
		set
		{
			_careerUnitPool = value;
		}
	}

	public RPGTacticUnitPool TacticUnitPool
	{
		get
		{
			return _tacticUnitPool;
		}
		set
		{
			_tacticUnitPool = value;
		}
	}

	public RPGSkillUnitPool SkillUnitPool
	{
		get
		{
			return _skillUnitPool;
		}
		set
		{
			_skillUnitPool = value;
		}
	}

	public RPGCareerAnimationUnitPool CareerAniPool
	{
		get
		{
			return _careerAniPool;
		}
		set
		{
			_careerAniPool = value;
		}
	}

	public RPGAttackEffectUnitPool AttackEffectPool
	{
		get
		{
			return _attackEffectPool;
		}
		set
		{
			_attackEffectPool = value;
		}
	}

	public RPGBeAttackEffectUnitPool BeAttackEffectPool
	{
		get
		{
			return _beAttackEffectPool;
		}
		set
		{
			_beAttackEffectPool = value;
		}
	}

	public RPGBuffEffectUnitPool BuffEffectPool
	{
		get
		{
			return _buffEffectPool;
		}
		set
		{
			_buffEffectPool = value;
		}
	}

	public RPGGemDefineUnitPool GemDefineUnitPool
	{
		get
		{
			return _gemDefineUnitPool;
		}
		set
		{
			_gemDefineUnitPool = value;
		}
	}

	public RPGCompoundFeeUnit CompoundFeePool
	{
		get
		{
			return _compoundFeePool;
		}
		set
		{
			_compoundFeePool = value;
		}
	}

	public RPGCompoundTableUnitPool CompoundTableUnitPool
	{
		get
		{
			return _compoundTableUnitPool;
		}
		set
		{
			_compoundTableUnitPool = value;
		}
	}

	public RPGMiscUnit RpgMiscUnit
	{
		get
		{
			return _rpgMiscUnit;
		}
		set
		{
			_rpgMiscUnit = value;
		}
	}

	public RPGGemShopUnitPool RpgGemShop
	{
		get
		{
			return _rpgGemShop;
		}
		set
		{
			_rpgGemShop = value;
		}
	}

	public RPGMapLayoutUnitPool MapLayout
	{
		get
		{
			return _mapLayout;
		}
		set
		{
			_mapLayout = value;
		}
	}

	public RPGSpeciaLevelDropPool SpecLevelDrop
	{
		get
		{
			return _specLevelDrop;
		}
		set
		{
			_specLevelDrop = value;
		}
	}

	public RPGLevel_SceneUnitPool Level_Scene
	{
		get
		{
			return _level_Scene;
		}
		set
		{
			_level_Scene = value;
		}
	}

	public RPGGemDropUnitPool GemDropPool
	{
		get
		{
			return _gem_drop;
		}
		set
		{
			_gem_drop = value;
		}
	}

	public List<OccupyPointIncome> LstOccupyPointIncome
	{
		get
		{
			return _lstOccupyPointIncome;
		}
		set
		{
			_lstOccupyPointIncome = value;
		}
	}

	public List<LevelIncome> LstLevelIncome
	{
		get
		{
			return _lstLevelIncome;
		}
		set
		{
			_lstLevelIncome = value;
		}
	}

	public List<RPGMaxExp> LstRPGMaxExp
	{
		get
		{
			return _lstRPGMaxExp;
		}
		set
		{
			_lstRPGMaxExp = value;
		}
	}

	private RPGGlobalData()
	{
	}
}
