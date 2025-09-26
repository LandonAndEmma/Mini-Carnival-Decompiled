using UnityEngine;

public class COMA_Fishing_FishPole : TBaseEntity
{
	public enum EFishPoleType
	{
		BLACK = 0,
		SLIVER = 1,
		GOLD = 2
	}

	protected enum ESeason
	{
		Spring = 0,
		Summer = 1,
		Autumn = 2,
		Winter = 3
	}

	[SerializeField]
	protected ESeason _curSeason = ESeason.Winter;

	[SerializeField]
	protected int _nMaxUseCount;

	[SerializeField]
	protected int _nMinUseCount;

	[SerializeField]
	protected int _nCurUseCount;

	public float _fFishPoleLen = 5f;

	[SerializeField]
	protected Transform _transEndPos;

	[SerializeField]
	protected Transform _transItemPos;

	[SerializeField]
	public Animation _aniCmp;

	public int CurUseCount
	{
		get
		{
			return _nCurUseCount;
		}
		set
		{
			_nCurUseCount = value;
			if (_nCurUseCount <= 0)
			{
				Debug.Log("---------> " + base.name + "  has damaged!!");
			}
			int iDByName = TFishingAddressBook.Instance.GetIDByName(1);
			TMessageDispatcher.Instance.DispatchMsg(GetInstanceID(), iDByName, 1007, TTelegram.SEND_MSG_IMMEDIATELY, _nCurUseCount);
		}
	}

	public Vector3 GetFishingPoleEndPos()
	{
		return _transEndPos.position;
	}

	public Transform GetFishingPoleItemPos()
	{
		return _transItemPos;
	}

	public void SetCurMonth(int nMonth)
	{
		if (nMonth >= 1 && nMonth <= 3)
		{
			_curSeason = ESeason.Spring;
		}
		else if (nMonth >= 4 && nMonth <= 6)
		{
			_curSeason = ESeason.Summer;
		}
		else if (nMonth >= 7 && nMonth <= 9)
		{
			_curSeason = ESeason.Autumn;
		}
		else if (nMonth >= 10 && nMonth <= 12)
		{
			_curSeason = ESeason.Winter;
		}
	}

	protected void Awake()
	{
	}

	public void InitFishPole(int nCount)
	{
		if (nCount != -1)
		{
			CurUseCount = nCount;
		}
		else
		{
			CurUseCount = Random.Range(_nMinUseCount, _nMaxUseCount + 1);
		}
	}

	protected void Start()
	{
		Debug.Log("---------> " + base.name + "  CurUseCount=" + CurUseCount);
	}

	public virtual COMA_Fishing_FishableObj GeneratorFishingObj()
	{
		if (CurUseCount <= 0)
		{
			return null;
		}
		COMA_Fishing_FishPoleData cOMA_Fishing_FishPoleData = (COMA_Fishing_FishPoleData)_resData;
		float num = cOMA_Fishing_FishPoleData._fp_inseason_fish;
		float num2 = num + cOMA_Fishing_FishPoleData._fp_un_inseason_fish;
		float num3 = num2 + cOMA_Fishing_FishPoleData._fp_treasure_chest;
		float num4 = num3 + cOMA_Fishing_FishPoleData._fp_garbage;
		COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = COMA_PlayerSelf.Instance as COMA_PlayerSelf_Fishing;
		if (cOMA_PlayerSelf_Fishing.IsOnBoat())
		{
			num4 -= 0.06f;
			num += 0.03f;
			num3 += 0.03f;
		}
		float num5 = (float)Random.Range(0, 100) / 100f;
		if (num5 < num)
		{
			return GeneratorInseasonFish();
		}
		if (num5 < num2)
		{
			return GeneratorUnInseasonFish();
		}
		if (num5 < num3)
		{
			return GeneratorChest();
		}
		return GeneratorGarbage();
	}

	protected virtual COMA_Fishing_FishableObj GeneratorInseasonFish()
	{
		return SpawnFishEntityByCustomID(GetOneInFishlistBySeason(_curSeason));
	}

	protected virtual COMA_Fishing_FishableObj GeneratorUnInseasonFish()
	{
		int[] inputArray = new int[3];
		int num = 0;
		for (int i = 0; i <= 3; i++)
		{
			if (i != (int)_curSeason)
			{
				inputArray[num] = i;
				num++;
			}
		}
		int[] outArray = new int[1];
		COMA_Tools.GetRandomArray_AppointArray(ref inputArray, ref outArray);
		return SpawnFishEntityByCustomID(GetOneInFishlistBySeason((ESeason)outArray[0]));
	}

	protected virtual COMA_Fishing_FishableObj GeneratorChest()
	{
		COMA_Fishing_FishPoleData cOMA_Fishing_FishPoleData = (COMA_Fishing_FishPoleData)_resData;
		GameObject gameObject = Object.Instantiate(Resources.Load("FBX/Scene/Fishing/Chest")) as GameObject;
		gameObject.name = "Chest";
		gameObject.transform.position = new Vector3(-1000f, -1000f, -10000f);
		COMA_Fishing_Chest component = gameObject.GetComponent<COMA_Fishing_Chest>();
		float fp_gold = cOMA_Fishing_FishPoleData._fp_chest_content._fp_gold;
		float num = fp_gold + cOMA_Fishing_FishPoleData._fp_chest_content._fp_crystal;
		float num2 = num + cOMA_Fishing_FishPoleData._fp_chest_content._fp_deco;
		float num3 = (float)Random.Range(0, 100) / 100f;
		if (num3 < fp_gold)
		{
			int goldNum = Random.Range(cOMA_Fishing_FishPoleData._gold_conf._nMinNum, cOMA_Fishing_FishPoleData._gold_conf._nMaxNum + 1);
			component.SetGoldNum(goldNum);
		}
		else if (num3 < num)
		{
			int crystalNum = Random.Range(cOMA_Fishing_FishPoleData._crystal_conf._nMinNum, cOMA_Fishing_FishPoleData._crystal_conf._nMaxNum + 1);
			component.SetCrystalNum(crystalNum);
		}
		else
		{
			float num4 = (float)Random.Range(0, 100) / 100f;
			int length = cOMA_Fishing_FishPoleData._deco.GetLength(0);
			float[] array = new float[length];
			float num5 = 0f;
			for (int i = 0; i < length; i++)
			{
				array[i] = num5 + cOMA_Fishing_FishPoleData._deco[i]._fp;
				num5 = array[i];
				if (num4 < num5)
				{
					int num6 = Random.Range(0, cOMA_Fishing_FishPoleData._deco[i]._decoList.GetLength(0));
					component.SetDecoName(cOMA_Fishing_FishPoleData._deco[i]._decoList[num6]);
					break;
				}
			}
		}
		return component;
	}

	protected virtual COMA_Fishing_FishableObj GeneratorGarbage()
	{
		GameObject gameObject = Object.Instantiate(Resources.Load("FBX/Scene/Fishing/Garbage")) as GameObject;
		gameObject.name = "Garbage";
		gameObject.transform.position = new Vector3(-1000f, -1000f, -10000f);
		return gameObject.GetComponent<COMA_Fishing_Garbage>();
	}

	private COMA_Fishing_Fish SpawnFishEntityByCustomID(int id)
	{
		GameObject gameObject = Object.Instantiate(Resources.Load("FBX/Scene/Fishing/Fish")) as GameObject;
		gameObject.name = "Fish_" + id;
		gameObject.transform.position = new Vector3(-1000f, -1000f, -10000f);
		COMA_Fishing_Fish component = gameObject.GetComponent<COMA_Fishing_Fish>();
		component.SetCustomID(id);
		Fish_Param fishParam = COMA_Fishing_FishPool.Instance.GetFishParam(id);
		int weight = Random.Range(fishParam._nMinWeight, fishParam._nMaxWeight + 1);
		component.SetWeight(weight);
		return component;
	}

	private int GetOneInFishlistBySeason(ESeason season)
	{
		COMA_Fishing_FishPoleData cOMA_Fishing_FishPoleData = (COMA_Fishing_FishPoleData)_resData;
		int[] fishList = cOMA_Fishing_FishPoleData._conf_season_fish[(int)season]._fishList;
		int num = Random.Range(0, fishList.GetLength(0));
		return fishList[num];
	}

	protected new void Update()
	{
	}
}
