using UnityEngine;

public class COMA_Achievement : MonoBehaviour
{
	private static COMA_Achievement _instance;

	private char sepSign = 'x';

	private int[] _achievement = new int[35];

	private int[] _count = new int[35]
	{
		10, 100, 20, 1, 1, 1, 1, 1, 1, 1,
		1, 10, 1, 1, 200, 300, 1000, 1, 4, 1,
		20, 40, 50, 500, 10, 500, 1000, 1, 1, 1,
		30, 100, 1, 20, 50
	};

	public static COMA_Achievement Instance
	{
		get
		{
			return _instance;
		}
	}

	public string content
	{
		get
		{
			string text = string.Empty;
			for (int i = 0; i < _achievement.Length; i++)
			{
				text = text + sepSign + _achievement[i].ToString();
			}
			return text.Substring(1);
		}
		set
		{
			if (value != null && !(value == string.Empty))
			{
				string[] array = value.Split(sepSign);
				int num = Mathf.Min(array.Length, _achievement.Length);
				for (int i = 0; i < num; i++)
				{
					_achievement[i] = int.Parse(array[i]);
				}
			}
		}
	}

	public int Drawer
	{
		get
		{
			return _achievement[0];
		}
		set
		{
			SetValue(0, value);
		}
	}

	public int Artist
	{
		get
		{
			return _achievement[1];
		}
		set
		{
			SetValue(1, value);
		}
	}

	public int Flagger
	{
		get
		{
			return _achievement[2];
		}
		set
		{
			SetValue(2, value);
		}
	}

	public int Flyer
	{
		get
		{
			return _achievement[3];
		}
		set
		{
			if (_achievement[3] >= 0 && _achievement[3] < _count[3])
			{
				_achievement[3] = value;
				if (_achievement[3] >= _count[3])
				{
					SceneTimerInstance.Instance.Add(5f, TimeToUnlock);
				}
				COMA_Pref.Instance.Save(true);
			}
		}
	}

	public int Lucky
	{
		get
		{
			return _achievement[4];
		}
		set
		{
			SetValue(4, value);
		}
	}

	public int Baddy
	{
		get
		{
			return _achievement[5];
		}
		set
		{
			SetValue(5, value);
		}
	}

	public int Digger
	{
		get
		{
			return _achievement[6];
		}
		set
		{
			SetValue(6, value);
		}
	}

	public int Robber
	{
		get
		{
			return _achievement[7];
		}
		set
		{
			SetValue(7, value);
		}
	}

	public int Escaper
	{
		get
		{
			return _achievement[8];
		}
		set
		{
			SetValue(8, value);
		}
	}

	public int Rocketer
	{
		get
		{
			return _achievement[9];
		}
		set
		{
			SetValue(9, value);
		}
	}

	public int Fighter
	{
		get
		{
			return _achievement[10];
		}
		set
		{
			SetValue(10, value);
		}
	}

	public int Rich
	{
		get
		{
			return _achievement[11];
		}
		set
		{
			SetValue(11, value);
		}
	}

	public int Apprentice
	{
		get
		{
			return _achievement[12];
		}
		set
		{
			SetValue(12, value);
		}
	}

	public int GetRich
	{
		get
		{
			return _achievement[13];
		}
		set
		{
			SetValue(13, value);
		}
	}

	public int DrawingExpert
	{
		get
		{
			return _achievement[14];
		}
		set
		{
			SetValue(14, value);
		}
	}

	public int DrawingMaster
	{
		get
		{
			return _achievement[15];
		}
		set
		{
			SetValue(15, value);
		}
	}

	public int DrawingGrandMaster
	{
		get
		{
			return _achievement[16];
		}
		set
		{
			SetValue(16, value);
		}
	}

	public int Fish1
	{
		get
		{
			return _achievement[17];
		}
		set
		{
			SetValue(17, value);
		}
	}

	public int Fish2
	{
		get
		{
			return _achievement[18];
		}
		set
		{
			SetValue(18, value);
		}
	}

	public int Fish3
	{
		get
		{
			return _achievement[19];
		}
		set
		{
			SetValue(19, value);
		}
	}

	public int Fish4
	{
		get
		{
			return _achievement[20];
		}
		set
		{
			SetValue(20, value);
		}
	}

	public int Fish5
	{
		get
		{
			return _achievement[21];
		}
		set
		{
			SetValue(21, value);
		}
	}

	public int Grata
	{
		get
		{
			return _achievement[22];
		}
		set
		{
			SetValue(22, value);
		}
	}

	public int VeryGrata
	{
		get
		{
			return _achievement[23];
		}
		set
		{
			SetValue(23, value);
		}
	}

	public int BloodWin
	{
		get
		{
			return _achievement[24];
		}
		set
		{
			SetValue(24, value);
		}
	}

	public int BloodKill50
	{
		get
		{
			return _achievement[25];
		}
		set
		{
			SetValue(25, value);
		}
	}

	public int BloodKill100
	{
		get
		{
			return _achievement[26];
		}
		set
		{
			SetValue(26, value);
		}
	}

	public int FlappyFly100
	{
		get
		{
			return _achievement[27];
		}
		set
		{
			SetValue(27, value);
		}
	}

	public int FlappyFly200
	{
		get
		{
			return _achievement[28];
		}
		set
		{
			SetValue(28, value);
		}
	}

	public int Friends1
	{
		get
		{
			return _achievement[29];
		}
		set
		{
			SetValue(29, value);
		}
	}

	public int Friends30
	{
		get
		{
			return _achievement[30];
		}
		set
		{
			SetValue(30, value);
		}
	}

	public int Friends100
	{
		get
		{
			return _achievement[31];
		}
		set
		{
			SetValue(31, value);
		}
	}

	public int FacebookShare1
	{
		get
		{
			return _achievement[32];
		}
		set
		{
			SetValue(32, value);
		}
	}

	public int FacebookShare20
	{
		get
		{
			return _achievement[33];
		}
		set
		{
			SetValue(33, value);
		}
	}

	public int FacebookShare50
	{
		get
		{
			return _achievement[34];
		}
		set
		{
			SetValue(34, value);
		}
	}

	private void OnEnable()
	{
		_instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnDisable()
	{
		_instance = null;
	}

	public void CheckFriendsAchievement()
	{
		int friends = (Friends30 = (Friends1 = UIDataBufferCenter.Instance.GetFriendsCount()));
		Friends100 = friends;
	}

	public void ResetData()
	{
	}

	private void SetValue(int id, int value)
	{
		if (_achievement[id] >= 0 && _achievement[id] < _count[id] && _achievement[id] != value)
		{
			_achievement[id] = value;
			Debug.Log(_achievement[id] + "  " + _count[id]);
			if (_achievement[id] >= _count[id])
			{
				_achievement[id] = _count[id];
				Unlock(id);
			}
			COMA_Pref.Instance.Save(true);
		}
	}

	public void FulFill(int id)
	{
		if (id >= 0 && id < _achievement.Length && id < _count.Length)
		{
			_achievement[id] = 0;
			SetValue(id, _count[id]);
		}
	}

	public bool TimeToUnlock()
	{
		Unlock(3);
		return false;
	}

	private void Unlock(int index)
	{
		Debug.Log(" unlock..................................... ");
		UIMessageBoxMgr.Instance.UIMessageBox(new UIACPopupBoxData(index + 1));
	}

	public int GetAcceptableCount()
	{
		int num = 0;
		for (int i = 0; i < _achievement.Length; i++)
		{
			if (_achievement[i] >= _count[i])
			{
				num++;
			}
		}
		return num;
	}

	public int GetAchievementState(int id)
	{
		if (_achievement[id] < 0)
		{
			return 2;
		}
		if (_achievement[id] >= _count[id])
		{
			return 1;
		}
		return 0;
	}

	public int GetAchievementNumCur(int id)
	{
		return _achievement[id];
	}

	public int GetAchievementNumMax(int id)
	{
		return _count[id];
	}

	public bool AcceptAchievement(int id)
	{
		switch (id)
		{
		case 0:
			COMA_Pref.Instance.AddCrystal(10);
			TUI_MsgBox.Instance.TipBox(1, 10, string.Empty, null);
			break;
		case 1:
		{
			if (COMA_Pref.Instance.PackageNullCount() < 1)
			{
				TUI_MsgBox.Instance.MessageBox(107);
				return false;
			}
			COMA_PackageItem cOMA_PackageItem8 = new COMA_PackageItem();
			cOMA_PackageItem8.serialName = "HT04";
			cOMA_PackageItem8.itemName = "Crown";
			cOMA_PackageItem8.part = 1;
			cOMA_PackageItem8.CreateIconTexture();
			cOMA_PackageItem8.state = COMA_PackageItem.PackageItemStatus.None;
			COMA_Pref.Instance.GetAnItem(cOMA_PackageItem8);
			TUI_MsgBox.Instance.TipBox(2, 1, "Crown", null, cOMA_PackageItem8.serialName);
			break;
		}
		case 2:
			COMA_Pref.Instance.AddGold(2000);
			TUI_MsgBox.Instance.TipBox(0, 2000, string.Empty, null);
			break;
		case 3:
		{
			if (COMA_Pref.Instance.PackageNullCount() < 1)
			{
				TUI_MsgBox.Instance.MessageBox(107);
				return false;
			}
			COMA_PackageItem cOMA_PackageItem5 = new COMA_PackageItem();
			cOMA_PackageItem5.serialName = "CB02";
			cOMA_PackageItem5.itemName = "Angel's Wings";
			cOMA_PackageItem5.part = 7;
			cOMA_PackageItem5.CreateIconTexture();
			cOMA_PackageItem5.state = COMA_PackageItem.PackageItemStatus.None;
			COMA_Pref.Instance.GetAnItem(cOMA_PackageItem5);
			TUI_MsgBox.Instance.TipBox(2, 1, "Angel's Wings", null, cOMA_PackageItem5.serialName);
			break;
		}
		case 4:
		{
			if (COMA_Pref.Instance.PackageNullCount() < 1)
			{
				TUI_MsgBox.Instance.MessageBox(107);
				return false;
			}
			COMA_PackageItem cOMA_PackageItem10 = new COMA_PackageItem();
			cOMA_PackageItem10.serialName = "HT05";
			cOMA_PackageItem10.itemName = "Straw Hat";
			cOMA_PackageItem10.part = 1;
			cOMA_PackageItem10.CreateIconTexture();
			cOMA_PackageItem10.state = COMA_PackageItem.PackageItemStatus.None;
			COMA_Pref.Instance.GetAnItem(cOMA_PackageItem10);
			TUI_MsgBox.Instance.TipBox(2, 1, "Straw Hat", null, cOMA_PackageItem10.serialName);
			break;
		}
		case 5:
		{
			if (COMA_Pref.Instance.PackageNullCount() < 1)
			{
				TUI_MsgBox.Instance.MessageBox(107);
				return false;
			}
			COMA_PackageItem cOMA_PackageItem4 = new COMA_PackageItem();
			cOMA_PackageItem4.serialName = "HT08";
			cOMA_PackageItem4.itemName = "Evil Horn";
			cOMA_PackageItem4.part = 1;
			cOMA_PackageItem4.CreateIconTexture();
			cOMA_PackageItem4.state = COMA_PackageItem.PackageItemStatus.None;
			COMA_Pref.Instance.GetAnItem(cOMA_PackageItem4);
			TUI_MsgBox.Instance.TipBox(2, 1, "Evil Horn", null, cOMA_PackageItem4.serialName);
			break;
		}
		case 6:
			COMA_Pref.Instance.AddCrystal(10);
			TUI_MsgBox.Instance.TipBox(1, 10, string.Empty, null);
			break;
		case 7:
			COMA_Pref.Instance.AddCrystal(15);
			TUI_MsgBox.Instance.TipBox(1, 15, string.Empty, null);
			break;
		case 8:
			COMA_Pref.Instance.AddGold(2000);
			TUI_MsgBox.Instance.TipBox(0, 2000, string.Empty, null);
			break;
		case 9:
			COMA_Pref.Instance.AddGold(2000);
			TUI_MsgBox.Instance.TipBox(0, 2000, string.Empty, null);
			break;
		case 10:
			COMA_Pref.Instance.AddGold(1000);
			TUI_MsgBox.Instance.TipBox(0, 1000, string.Empty, null);
			break;
		case 11:
			COMA_Pref.Instance.AddGold(10000);
			TUI_MsgBox.Instance.TipBox(0, 10000, string.Empty, null);
			break;
		case 12:
			COMA_Pref.Instance.AddGold(500);
			TUI_MsgBox.Instance.TipBox(0, 500, string.Empty, null);
			break;
		case 13:
			COMA_Pref.Instance.AddGold(300);
			TUI_MsgBox.Instance.TipBox(0, 300, string.Empty, null);
			break;
		case 14:
		{
			if (COMA_Pref.Instance.PackageNullCount() < 1)
			{
				TUI_MsgBox.Instance.MessageBox(107);
				return false;
			}
			COMA_PackageItem cOMA_PackageItem11 = new COMA_PackageItem();
			cOMA_PackageItem11.serialName = "HT64";
			cOMA_PackageItem11.itemName = "Evil Horn";
			cOMA_PackageItem11.part = 1;
			cOMA_PackageItem11.CreateIconTexture();
			cOMA_PackageItem11.state = COMA_PackageItem.PackageItemStatus.None;
			COMA_Pref.Instance.GetAnItem(cOMA_PackageItem11);
			TUI_MsgBox.Instance.TipBox(2, 1, "Evil Horn", null, cOMA_PackageItem11.serialName);
			break;
		}
		case 15:
		{
			if (COMA_Pref.Instance.PackageNullCount() < 1)
			{
				TUI_MsgBox.Instance.MessageBox(107);
				return false;
			}
			COMA_PackageItem cOMA_PackageItem7 = new COMA_PackageItem();
			cOMA_PackageItem7.serialName = "CB12";
			cOMA_PackageItem7.itemName = "Evil Horn";
			cOMA_PackageItem7.part = 1;
			cOMA_PackageItem7.CreateIconTexture();
			cOMA_PackageItem7.state = COMA_PackageItem.PackageItemStatus.None;
			COMA_Pref.Instance.GetAnItem(cOMA_PackageItem7);
			TUI_MsgBox.Instance.TipBox(2, 1, "Evil Horn", null, cOMA_PackageItem7.serialName);
			break;
		}
		case 16:
		{
			if (COMA_Pref.Instance.PackageNullCount() < 1)
			{
				TUI_MsgBox.Instance.MessageBox(107);
				return false;
			}
			COMA_PackageItem cOMA_PackageItem2 = new COMA_PackageItem();
			cOMA_PackageItem2.serialName = "HT69";
			cOMA_PackageItem2.itemName = "Evil Horn";
			cOMA_PackageItem2.part = 1;
			cOMA_PackageItem2.CreateIconTexture();
			cOMA_PackageItem2.state = COMA_PackageItem.PackageItemStatus.None;
			COMA_Pref.Instance.GetAnItem(cOMA_PackageItem2);
			TUI_MsgBox.Instance.TipBox(2, 1, "Evil Horn", null, cOMA_PackageItem2.serialName);
			break;
		}
		case 17:
			COMA_Pref.Instance.AddGold(2000);
			TUI_MsgBox.Instance.TipBox(0, 2000, string.Empty, null);
			break;
		case 18:
		{
			if (COMA_Pref.Instance.PackageNullCount() < 1)
			{
				TUI_MsgBox.Instance.MessageBox(107);
				return false;
			}
			COMA_PackageItem cOMA_PackageItem16 = new COMA_PackageItem();
			cOMA_PackageItem16.serialName = "HT71";
			cOMA_PackageItem16.itemName = "Evil Horn";
			cOMA_PackageItem16.part = 1;
			cOMA_PackageItem16.CreateIconTexture();
			cOMA_PackageItem16.state = COMA_PackageItem.PackageItemStatus.None;
			COMA_Pref.Instance.GetAnItem(cOMA_PackageItem16);
			TUI_MsgBox.Instance.TipBox(2, 1, "Evil Horn", null, cOMA_PackageItem16.serialName);
			break;
		}
		case 19:
		{
			if (COMA_Pref.Instance.PackageNullCount() < 1)
			{
				TUI_MsgBox.Instance.MessageBox(107);
				return false;
			}
			COMA_PackageItem cOMA_PackageItem15 = new COMA_PackageItem();
			cOMA_PackageItem15.serialName = "HT70";
			cOMA_PackageItem15.itemName = "Evil Horn";
			cOMA_PackageItem15.part = 1;
			cOMA_PackageItem15.CreateIconTexture();
			cOMA_PackageItem15.state = COMA_PackageItem.PackageItemStatus.None;
			COMA_Pref.Instance.GetAnItem(cOMA_PackageItem15);
			TUI_MsgBox.Instance.TipBox(2, 1, "Evil Horn", null, cOMA_PackageItem15.serialName);
			break;
		}
		case 20:
			COMA_Pref.Instance.AddCrystal(10);
			TUI_MsgBox.Instance.TipBox(1, 10, string.Empty, null);
			break;
		case 21:
		{
			if (COMA_Pref.Instance.PackageNullCount() < 1)
			{
				TUI_MsgBox.Instance.MessageBox(107);
				return false;
			}
			COMA_PackageItem cOMA_PackageItem14 = new COMA_PackageItem();
			cOMA_PackageItem14.serialName = "CB10";
			cOMA_PackageItem14.itemName = "Evil Horn";
			cOMA_PackageItem14.part = 1;
			cOMA_PackageItem14.CreateIconTexture();
			cOMA_PackageItem14.state = COMA_PackageItem.PackageItemStatus.None;
			COMA_Pref.Instance.GetAnItem(cOMA_PackageItem14);
			TUI_MsgBox.Instance.TipBox(2, 1, "Evil Horn", null, cOMA_PackageItem14.serialName);
			break;
		}
		case 22:
			COMA_Pref.Instance.AddGold(2000);
			TUI_MsgBox.Instance.TipBox(0, 2000, string.Empty, null);
			break;
		case 23:
		{
			if (COMA_Pref.Instance.PackageNullCount() < 1)
			{
				TUI_MsgBox.Instance.MessageBox(107);
				return false;
			}
			COMA_PackageItem cOMA_PackageItem13 = new COMA_PackageItem();
			cOMA_PackageItem13.serialName = "HT72";
			cOMA_PackageItem13.itemName = "Flowers";
			cOMA_PackageItem13.part = 1;
			cOMA_PackageItem13.CreateIconTexture();
			cOMA_PackageItem13.state = COMA_PackageItem.PackageItemStatus.None;
			COMA_Pref.Instance.GetAnItem(cOMA_PackageItem13);
			TUI_MsgBox.Instance.TipBox(2, 1, "Flowers", null, cOMA_PackageItem13.serialName);
			break;
		}
		case 24:
		{
			if (COMA_Pref.Instance.PackageNullCount() < 1)
			{
				TUI_MsgBox.Instance.MessageBox(107);
				return false;
			}
			COMA_PackageItem cOMA_PackageItem12 = new COMA_PackageItem();
			cOMA_PackageItem12.serialName = "CB13";
			cOMA_PackageItem12.itemName = "P90";
			cOMA_PackageItem12.part = 7;
			cOMA_PackageItem12.CreateIconTexture();
			cOMA_PackageItem12.state = COMA_PackageItem.PackageItemStatus.None;
			COMA_Pref.Instance.GetAnItem(cOMA_PackageItem12);
			TUI_MsgBox.Instance.TipBox(2, 1, "P90", null, cOMA_PackageItem12.serialName);
			break;
		}
		case 25:
		{
			if (COMA_Pref.Instance.PackageNullCount() < 1)
			{
				TUI_MsgBox.Instance.MessageBox(107);
				return false;
			}
			COMA_PackageItem cOMA_PackageItem9 = new COMA_PackageItem();
			cOMA_PackageItem9.serialName = "CB14";
			cOMA_PackageItem9.itemName = "M16";
			cOMA_PackageItem9.part = 7;
			cOMA_PackageItem9.CreateIconTexture();
			cOMA_PackageItem9.state = COMA_PackageItem.PackageItemStatus.None;
			COMA_Pref.Instance.GetAnItem(cOMA_PackageItem9);
			TUI_MsgBox.Instance.TipBox(2, 1, "M16", null, cOMA_PackageItem9.serialName);
			break;
		}
		case 26:
		{
			if (COMA_Pref.Instance.PackageNullCount() < 1)
			{
				TUI_MsgBox.Instance.MessageBox(107);
				return false;
			}
			COMA_PackageItem cOMA_PackageItem6 = new COMA_PackageItem();
			cOMA_PackageItem6.serialName = "CB15";
			cOMA_PackageItem6.itemName = "Sniper";
			cOMA_PackageItem6.part = 7;
			cOMA_PackageItem6.CreateIconTexture();
			cOMA_PackageItem6.state = COMA_PackageItem.PackageItemStatus.None;
			COMA_Pref.Instance.GetAnItem(cOMA_PackageItem6);
			TUI_MsgBox.Instance.TipBox(2, 1, "Sniper", null, cOMA_PackageItem6.serialName);
			break;
		}
		case 27:
		{
			if (COMA_Pref.Instance.PackageNullCount() < 1)
			{
				TUI_MsgBox.Instance.MessageBox(107);
				return false;
			}
			COMA_PackageItem cOMA_PackageItem3 = new COMA_PackageItem();
			cOMA_PackageItem3.serialName = "CB19";
			cOMA_PackageItem3.itemName = "Rockets";
			cOMA_PackageItem3.part = 7;
			cOMA_PackageItem3.CreateIconTexture();
			cOMA_PackageItem3.state = COMA_PackageItem.PackageItemStatus.None;
			COMA_Pref.Instance.GetAnItem(cOMA_PackageItem3);
			TUI_MsgBox.Instance.TipBox(2, 1, "Rockets", null, cOMA_PackageItem3.serialName);
			break;
		}
		case 28:
		{
			if (COMA_Pref.Instance.PackageNullCount() < 1)
			{
				TUI_MsgBox.Instance.MessageBox(107);
				return false;
			}
			COMA_PackageItem cOMA_PackageItem = new COMA_PackageItem();
			cOMA_PackageItem.serialName = "CB20";
			cOMA_PackageItem.itemName = "Rocket";
			cOMA_PackageItem.part = 7;
			cOMA_PackageItem.CreateIconTexture();
			cOMA_PackageItem.state = COMA_PackageItem.PackageItemStatus.None;
			COMA_Pref.Instance.GetAnItem(cOMA_PackageItem);
			TUI_MsgBox.Instance.TipBox(2, 1, "Rocket", null, cOMA_PackageItem.serialName);
			break;
		}
		case 29:
			COMA_Pref.Instance.AddGold(1000);
			TUI_MsgBox.Instance.TipBox(0, 1000, string.Empty, null);
			break;
		case 30:
			COMA_Pref.Instance.AddCrystal(5);
			TUI_MsgBox.Instance.TipBox(1, 5, string.Empty, null);
			break;
		case 31:
			COMA_Pref.Instance.AddCrystal(15);
			TUI_MsgBox.Instance.TipBox(1, 15, string.Empty, null);
			break;
		case 32:
			COMA_Pref.Instance.AddGold(1000);
			TUI_MsgBox.Instance.TipBox(0, 1000, string.Empty, null);
			break;
		case 33:
			COMA_Pref.Instance.AddCrystal(5);
			TUI_MsgBox.Instance.TipBox(1, 5, string.Empty, null);
			break;
		case 34:
			COMA_Pref.Instance.AddCrystal(15);
			TUI_MsgBox.Instance.TipBox(1, 15, string.Empty, null);
			break;
		default:
			Debug.LogError(id);
			break;
		}
		_achievement[id] = -1;
		COMA_Pref.Instance.Save(true);
		return true;
	}
}
