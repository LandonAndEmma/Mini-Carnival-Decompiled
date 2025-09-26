using System;

public class COMA_Fishing_FishPoleData : TResData
{
	[Serializable]
	public class Season_fishList
	{
		public int[] _fishList;
	}

	[Serializable]
	public class Deco_content
	{
		public float _fp;

		public string[] _decoList;
	}

	[Serializable]
	public class Gold
	{
		public int _nMaxNum;

		public int _nMinNum;
	}

	[Serializable]
	public class Crystal
	{
		public int _nMaxNum;

		public int _nMinNum;
	}

	[Serializable]
	public class Chest_content
	{
		public float _fp_gold;

		public float _fp_crystal;

		public float _fp_deco;
	}

	public float _fp_inseason_fish;

	public float _fp_un_inseason_fish;

	public float _fp_treasure_chest;

	public float _fp_garbage;

	public Season_fishList[] _conf_season_fish;

	public Deco_content[] _deco;

	public Gold _gold_conf;

	public Crystal _crystal_conf;

	public Chest_content _fp_chest_content;
}
