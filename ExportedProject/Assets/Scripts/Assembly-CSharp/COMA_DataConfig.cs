using System.Collections.Generic;
using System.Xml;
using Protocol.Role.S2C;
using UIGlobal;
using UnityEngine;

public class COMA_DataConfig : MonoBehaviour
{
	private static COMA_DataConfig _instance;

	public TextAsset equipmentAsset;

	private XmlNode gunAtlas;

	private List<XmlElement> elms = new List<XmlElement>();

	private int count_model;

	private int count_accessories;

	public List<UIMarket_BoxData> sysShop_Accessories = new List<UIMarket_BoxData>();

	public Config _sysConfig = new Config();

	public CHelpTips _helpTips = new CHelpTips();

	public static COMA_DataConfig Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
	}

	private void OnEnable()
	{
		_instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
		Init();
	}

	private void OnDisable()
	{
		_instance = null;
	}

	private void Init()
	{
		XmlNode xmlNode = COMA_Sys.Instance.ParseXml(equipmentAsset);
		gunAtlas = xmlNode["GunAtlas"];
	}

	public void GetDataConfig(string serialName, ref COMA_DataConfig_Gun config)
	{
		if (gunAtlas == null)
		{
			Debug.LogWarning("gunAtlas is Null!!");
			return;
		}
		string text = serialName.Substring(0, 1);
		XmlElement xmlElement = gunAtlas[text][serialName];
		Debug.Log("Get Attribute : " + serialName);
		config.name = xmlElement.GetAttribute("name");
		config.gold = int.Parse(xmlElement.GetAttribute("gold"));
		config.lv = int.Parse(xmlElement.GetAttribute("lv"));
		config.ap = float.Parse(xmlElement.GetAttribute("ap"));
		config.apr = float.Parse(xmlElement.GetAttribute("apr"));
		config.radius = float.Parse(xmlElement.GetAttribute("radius"));
		config.push = float.Parse(xmlElement.GetAttribute("push"));
		float num = float.Parse(xmlElement.GetAttribute("aimdistance"));
		float num2 = float.Parse(xmlElement.GetAttribute("aimradius"));
		config.precision = num2 / num;
		config.aimradiusmax = float.Parse(xmlElement.GetAttribute("aimradiusmax"));
		config.aimmagnify = float.Parse(xmlElement.GetAttribute("aimmagnify"));
		config.shotcount = int.Parse(xmlElement.GetAttribute("shotcount"));
		config.bulletspeed = float.Parse(xmlElement.GetAttribute("bulletspeed"));
		config.bulletgravity = float.Parse(xmlElement.GetAttribute("bulletgravity"));
		config.bulletrange = float.Parse(xmlElement.GetAttribute("bulletrange"));
		config.frequency = float.Parse(xmlElement.GetAttribute("frequency"));
		config.clipcount = int.Parse(xmlElement.GetAttribute("clipcount"));
		config.reloadtime = float.Parse(xmlElement.GetAttribute("reloadtime"));
		config.preshoottime = float.Parse(xmlElement.GetAttribute("preshoottime"));
		config.movespeedadd = float.Parse(xmlElement.GetAttribute("movespeedadd"));
		config.lineattackdistance = xmlElement.GetAttribute("lineattackdistance");
		config.penetrate = xmlElement.GetAttribute("penetrate");
	}

	public void ParseSysShop(string config)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(config);
		XmlNode documentElement = xmlDocument.DocumentElement;
		elms = new List<XmlElement>();
		for (int i = 0; i < documentElement.ChildNodes.Count; i++)
		{
			if (!(documentElement.ChildNodes[i].Name == "#comment"))
			{
				XmlElement xmlElement = documentElement.ChildNodes[i] as XmlElement;
				elms.Add(xmlElement);
				if (xmlElement.Name.StartsWith("D"))
				{
					count_model++;
				}
				else
				{
					count_accessories++;
				}
			}
		}
	}

	public int GetSysShopCount_Accessories()
	{
		return count_accessories;
	}

	public int GetSysShopCount_Model()
	{
		return count_model;
	}

	public void GetSysShopData_Accessories(int i, ref UIMarket_BoxData data)
	{
		GetSysShopData(i + 4, ref data);
	}

	public void GetSysShopData_Model(int i, ref UIMarket_BoxData data)
	{
		if (i < 4)
		{
			GetSysShopData(i, ref data);
			NotifyRoleDataCmd notifyRoleDataCmd = UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role) as NotifyRoleDataCmd;
			if (i == 0 && notifyRoleDataCmd.m_bag_data.m_first_buy_head != 0)
			{
				data.Price = (int)((float)data.Price * ((float)_sysConfig.Shop.first_buy / 100f));
			}
			else if (i == 1 && notifyRoleDataCmd.m_bag_data.m_first_buy_body != 0)
			{
				data.Price = (int)((float)data.Price * ((float)_sysConfig.Shop.first_buy / 100f));
			}
			else if (i == 2 && notifyRoleDataCmd.m_bag_data.m_first_buy_leg != 0)
			{
				data.Price = (int)((float)data.Price * ((float)_sysConfig.Shop.first_buy / 100f));
			}
			else if (i == 3)
			{
				UIMarket_BoxData data2 = new UIMarket_BoxData();
				GetSysShopData_Model(0, ref data2);
				UIMarket_BoxData data3 = new UIMarket_BoxData();
				GetSysShopData_Model(1, ref data3);
				UIMarket_BoxData data4 = new UIMarket_BoxData();
				GetSysShopData_Model(2, ref data4);
				data.Price = (data2.Price + data3.Price + data4.Price) * _sysConfig.Shop.suit_discount / 100;
			}
		}
	}

	private void GetSysShopData(int i, ref UIMarket_BoxData data)
	{
		data.CurrencyType = ECurrencyType.Gold;
		data.Price = int.Parse(elms[i].GetAttribute("crystal"));
		if (data.Price <= 0)
		{
			data.Price = int.Parse(elms[i].GetAttribute("gold"));
		}
		else
		{
			data.CurrencyType = ECurrencyType.Crystal;
		}
		data.Unit = elms[i].GetAttribute("serial");
		data.DataType = 2;
	}

	public void ParseSysConfig(string config)
	{
		_sysConfig = COMA_Tools.DeserializeObject<Config>(config) as Config;
		Debug.Log("sysConfig Parse Finish!! " + _sysConfig.Bag.unlock_cost);
	}
}
