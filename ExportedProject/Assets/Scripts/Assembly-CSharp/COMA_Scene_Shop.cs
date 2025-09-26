using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class COMA_Scene_Shop : MonoBehaviour
{
	public static readonly string iconPrename = "deco_";

	private static COMA_Scene_Shop _instance;

	public int framesToLate;

	private XmlNode shopAtlas;

	public List<UIMarket_AvatarShopData> shopData_Molds = new List<UIMarket_AvatarShopData>();

	public List<UIMarket_AvatarShopData> shopData_Accessories = new List<UIMarket_AvatarShopData>();

	private int[] modelPrices = new int[3];

	public static COMA_Scene_Shop Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		if (_instance != null)
		{
			Object.DestroyObject(base.gameObject);
			return;
		}
		_instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public static void ResetInstance()
	{
		_instance = null;
	}

	private IEnumerator Start()
	{
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(COMA_Version.Instance.shopConfig);
		shopAtlas = xmlDoc.DocumentElement;
		List<XmlElement> elms = new List<XmlElement>();
		for (int i = 0; i < shopAtlas.ChildNodes.Count; i++)
		{
			if (!(shopAtlas.ChildNodes[i].Name == "#comment"))
			{
				XmlElement itemElm = shopAtlas.ChildNodes[i] as XmlElement;
				elms.Add(itemElm);
			}
		}
		for (int j = 0; j < elms.Count; j++)
		{
			UIMarket_AvatarShopData data = new UIMarket_AvatarShopData
			{
				AvatarPrice = int.Parse(elms[j].GetAttribute("crystal"))
			};
			if (data.AvatarPrice <= 0)
			{
				data.AvatarPrice = int.Parse(elms[j].GetAttribute("gold"));
			}
			else
			{
				data.AvatarPrice = -data.AvatarPrice;
			}
			data.OfficalIcon = false;
			data.Suited = false;
			data.PartType = elms[j].GetAttribute("serial");
			data.itemName = elms[j].GetAttribute("name");
			if (elms[j].Name.StartsWith("D"))
			{
				GameObject tarObj = Object.Instantiate(Resources.Load("FBX/Player/Part/PFB/" + data.PartType)) as GameObject;
				Object.DontDestroyOnLoad(tarObj);
				for (int k = 0; k < framesToLate; k++)
				{
					yield return new WaitForEndOfFrame();
				}
				framesToLate++;
				data.AvatarIcon = IconShot.Instance.GetIconPic(tarObj, true);
			}
			else
			{
				data.AvatarIconName = iconPrename + data.PartType;
			}
			if (elms[j].Name.StartsWith("D"))
			{
				if (data.PartType == "Head01")
				{
					if (!COMA_Pref.Instance.HasBought(0))
					{
						data.AvatarPrice /= 4;
					}
					modelPrices[0] = data.AvatarPrice;
				}
				else if (data.PartType == "Body01")
				{
					if (!COMA_Pref.Instance.HasBought(1))
					{
						data.AvatarPrice /= 4;
					}
					modelPrices[1] = data.AvatarPrice;
				}
				else if (data.PartType == "Leg01")
				{
					if (!COMA_Pref.Instance.HasBought(2))
					{
						data.AvatarPrice /= 4;
					}
					modelPrices[2] = data.AvatarPrice;
				}
				else if (data.PartType == "HBL01")
				{
					data.AvatarPrice = (int)((float)((modelPrices[0] + modelPrices[1] + modelPrices[2]) * COMA_Sys.Instance.modelSuitSaleRate) / 100f);
				}
				shopData_Molds.Add(data);
			}
			else
			{
				shopData_Accessories.Add(data);
			}
		}
		Debug.Log("Molds : " + shopData_Molds.Count + "  Accessories : " + shopData_Accessories.Count);
		yield return 0;
	}
}
