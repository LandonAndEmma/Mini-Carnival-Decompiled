using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class UIArmory_WeaponBox : UIArmory_Box
{
	[Serializable]
	public class WeaponBoxData : ISerializable
	{
		public enum EPurchaseType
		{
			Gold = 0,
			Gem = 1,
			MaxCount = 2
		}

		public enum ELockType
		{
			Locked = 0,
			UnLocked = 1,
			MaxCount = 2
		}

		public int level;

		public int cost;

		public char damage;

		public char accureacy;

		public string weaponName;

		public EPurchaseType purchaseType;

		public ELockType lockType;

		public WeaponBoxData()
		{
			level = 11;
			cost = 100;
			damage = 'D';
			accureacy = 'S';
			weaponName = "Shotgun";
			purchaseType = EPurchaseType.Gem;
			lockType = ELockType.UnLocked;
		}

		private WeaponBoxData(SerializationInfo info, StreamingContext ctxt)
		{
			level = (int)info.GetValue("level", typeof(int));
			cost = (int)info.GetValue("cost", typeof(int));
			damage = (char)info.GetValue("damage", typeof(char));
			accureacy = (char)info.GetValue("accureacy", typeof(char));
			weaponName = (string)info.GetValue("weaponName", typeof(string));
			purchaseType = (EPurchaseType)(int)info.GetValue("purchaseType", typeof(EPurchaseType));
			lockType = (ELockType)(int)info.GetValue("lockType", typeof(ELockType));
		}

		public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
		{
			info.AddValue("level", level);
			info.AddValue("cost", cost);
			info.AddValue("damage", damage);
			info.AddValue("accureacy", accureacy);
			info.AddValue("weaponName", weaponName);
			info.AddValue("purchaseType", purchaseType);
			info.AddValue("lockType", lockType);
		}
	}

	private WeaponBoxData weaponBoxData = new WeaponBoxData();

	private void Awake()
	{
		CheckScript();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private int CheckScript()
	{
		return 0;
	}

	public int LoadWeaponInfo()
	{
		TextAsset textAsset = Resources.Load("UI/Armory/weapon_test") as TextAsset;
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.Write(textAsset.bytes, 0, textAsset.bytes.Length);
		memoryStream.Seek(0L, SeekOrigin.Begin);
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		weaponBoxData = (WeaponBoxData)binaryFormatter.Deserialize(memoryStream);
		memoryStream.Close();
		return 0;
	}
}
