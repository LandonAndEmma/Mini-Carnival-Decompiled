using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COMA_Scene_Trade : MonoBehaviour
{
	public class TradeTexInfo
	{
		public string tid = string.Empty;

		public Texture2D[] tex = new Texture2D[3];

		public bool isSuit;

		public int kind = -1;

		public int price;

		public string[] serialName = new string[3]
		{
			string.Empty,
			string.Empty,
			string.Empty
		};

		public UIMarket_AvatarShopData data = new UIMarket_AvatarShopData();

		public void Reset()
		{
			tid = string.Empty;
			tex[0] = null;
			tex[1] = null;
			tex[2] = null;
			isSuit = false;
			kind = -1;
			price = 0;
			serialName[0] = string.Empty;
			serialName[1] = string.Empty;
			serialName[2] = string.Empty;
			data = new UIMarket_AvatarShopData();
		}

		public void Clone(TradeTexInfo info)
		{
			tid = info.tid;
			tex[0] = info.tex[0];
			tex[1] = info.tex[1];
			tex[2] = info.tex[2];
			isSuit = info.isSuit;
			kind = info.kind;
			price = info.price;
			serialName[0] = info.serialName[0];
			serialName[1] = info.serialName[1];
			serialName[2] = info.serialName[2];
			data.OfficalIcon = info.data.OfficalIcon;
			data.AvatarPrice = info.data.AvatarPrice;
			data.Suited = info.data.Suited;
		}
	}

	private static COMA_Scene_Trade _instance;

	public int forDifferentParam;

	public int texCountToLoad_Official;

	public int texCountToLoad_Suit;

	public int texCountToLoad_Part;

	public TradeTexInfo officialTex = new TradeTexInfo();

	public List<TradeTexInfo> suitTex = new List<TradeTexInfo>();

	public List<TradeTexInfo> partTex = new List<TradeTexInfo>();

	public static readonly int suitCount = 11;

	public static readonly int partCount = 8;

	public static COMA_Scene_Trade Instance
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

	private void Start()
	{
		if (COMA_Sys.Instance.bMemFirstGame)
		{
			_instance = null;
			Object.DestroyObject(base.gameObject);
			return;
		}
		for (int i = 0; i < suitCount; i++)
		{
			suitTex.Add(new TradeTexInfo());
		}
		for (int j = 0; j < partCount; j++)
		{
			partTex.Add(new TradeTexInfo());
		}
		if (COMA_Server_Texture.Instance != null)
		{
			RequestItemsOnBuyShell();
		}
	}

	public void RequestItemsOnBuyShell()
	{
		forDifferentParam++;
		COMA_Scene_Shop.Instance.framesToLate = 0;
		texCountToLoad_Official = -1;
		texCountToLoad_Suit = -1;
		texCountToLoad_Part = -1;
		COMA_Server_Texture.Instance.TextureOfficial_GetTextureFromServer(forDifferentParam);
		for (int i = 0; i < suitCount; i++)
		{
			suitTex[i].Reset();
		}
		COMA_Server_Texture.Instance.TextureSuit_GetTexListFromServer(suitCount, forDifferentParam);
		for (int j = 0; j < partCount; j++)
		{
			partTex[j].Reset();
		}
		COMA_Server_Texture.Instance.TextureSell_GetTexListFromServer(partCount, forDifferentParam);
	}

	public TradeTexInfo GetTexInfoBySlotID(int slotID)
	{
		if (slotID < 1)
		{
			return officialTex;
		}
		if (slotID < 1 + suitCount && suitTex[slotID - 1].tid != string.Empty)
		{
			return suitTex[slotID - 1];
		}
		if (slotID < 1 + suitCount + partCount && partTex[slotID - 1 - suitCount].tid != string.Empty)
		{
			return partTex[slotID - 1 - suitCount];
		}
		return null;
	}

	public void SetOfficial(Texture2D tex, int kind, int gold, int num, float leftTime, bool isOfficial, int param)
	{
		if (forDifferentParam == param)
		{
			if (kind < 0)
			{
				Debug.LogError("kind : " + kind);
			}
			officialTex.tex[kind] = tex;
			officialTex.tid = string.Empty;
			officialTex.isSuit = true;
			officialTex.kind = -1;
			officialTex.price = gold;
			officialTex.serialName[kind] = COMA_PackageItem.KindToName(kind);
			officialTex.data.OfficalIcon = true;
			officialTex.data.AvatarPrice = gold;
			officialTex.data.Suited = officialTex.isSuit;
			if (officialTex.tex[0] != null && officialTex.tex[1] != null && officialTex.tex[2] != null)
			{
				StartCoroutine(RenderOfficial());
			}
		}
	}

	public void SetSuit(string tid, Texture2D[] tex, int gold, int num, float leftTime, bool isOfficial, int param)
	{
		if (forDifferentParam != param)
		{
			return;
		}
		TradeTexInfo tradeTexInfo = new TradeTexInfo();
		tradeTexInfo.tid = tid;
		tradeTexInfo.tex = tex;
		tradeTexInfo.isSuit = true;
		tradeTexInfo.kind = -1;
		tradeTexInfo.price = gold;
		tradeTexInfo.serialName[0] = COMA_PackageItem.KindToName(0);
		tradeTexInfo.serialName[1] = COMA_PackageItem.KindToName(1);
		tradeTexInfo.serialName[2] = COMA_PackageItem.KindToName(2);
		tradeTexInfo.data.OfficalIcon = false;
		tradeTexInfo.data.AvatarPrice = gold;
		tradeTexInfo.data.Suited = tradeTexInfo.isSuit;
		int i;
		for (i = 0; i < suitTex.Count; i++)
		{
			if (suitTex[i].tid == string.Empty)
			{
				suitTex[i].Clone(tradeTexInfo);
				break;
			}
		}
		StartCoroutine(RenderSuit(i));
	}

	public void SetPart(string tid, Texture2D tex, int kind, int gold, int num, float leftTime, bool isOfficial, int param)
	{
		if (forDifferentParam != param)
		{
			return;
		}
		if (kind < 0)
		{
			Debug.LogError("kind : " + kind);
		}
		TradeTexInfo tradeTexInfo = new TradeTexInfo();
		tradeTexInfo.tid = tid;
		tradeTexInfo.tex[0] = tex;
		tradeTexInfo.isSuit = false;
		tradeTexInfo.kind = kind;
		tradeTexInfo.price = gold;
		tradeTexInfo.serialName[0] = COMA_PackageItem.KindToName(kind);
		tradeTexInfo.data.OfficalIcon = false;
		tradeTexInfo.data.AvatarPrice = gold;
		tradeTexInfo.data.Suited = tradeTexInfo.isSuit;
		int i;
		for (i = 0; i < partTex.Count; i++)
		{
			if (partTex[i].tid == string.Empty)
			{
				partTex[i].Clone(tradeTexInfo);
				break;
			}
		}
		StartCoroutine(RenderPart(i, partTex[i].serialName[0]));
	}

	public IEnumerator RenderOfficial()
	{
		for (int i = 0; i < COMA_Scene_Shop.Instance.framesToLate; i++)
		{
			yield return new WaitForEndOfFrame();
		}
		COMA_Scene_Shop.Instance.framesToLate++;
		GameObject tarObj = Object.Instantiate(Resources.Load("FBX/Player/Part/PFB/All")) as GameObject;
		tarObj.transform.FindChild("head").renderer.material.mainTexture = officialTex.tex[0];
		tarObj.transform.FindChild("body").renderer.material.mainTexture = officialTex.tex[1];
		tarObj.transform.FindChild("leg").renderer.material.mainTexture = officialTex.tex[2];
		officialTex.data.AvatarIcon = IconShot.Instance.GetIconPic(tarObj);
		Object.DestroyObject(tarObj);
		CheckRefreshFinish();
		yield return 0;
	}

	public IEnumerator RenderSuit(int id)
	{
		for (int i = 0; i < COMA_Scene_Shop.Instance.framesToLate; i++)
		{
			yield return new WaitForEndOfFrame();
		}
		COMA_Scene_Shop.Instance.framesToLate++;
		GameObject tarObj = Object.Instantiate(Resources.Load("FBX/Player/Part/PFB/All")) as GameObject;
		tarObj.transform.FindChild("head").renderer.material.mainTexture = suitTex[id].tex[0];
		tarObj.transform.FindChild("body").renderer.material.mainTexture = suitTex[id].tex[1];
		tarObj.transform.FindChild("leg").renderer.material.mainTexture = suitTex[id].tex[2];
		suitTex[id].data.AvatarIcon = IconShot.Instance.GetIconPic(tarObj);
		Object.DestroyObject(tarObj);
		CheckRefreshFinish();
		yield return 0;
	}

	public IEnumerator RenderPart(int id, string fileName)
	{
		for (int k = 0; k < COMA_Scene_Shop.Instance.framesToLate; k++)
		{
			yield return new WaitForEndOfFrame();
		}
		COMA_Scene_Shop.Instance.framesToLate++;
		GameObject tarObj = Object.Instantiate(Resources.Load("FBX/Player/Part/PFB/" + fileName)) as GameObject;
		tarObj.transform.GetChild(0).renderer.material.mainTexture = partTex[id].tex[0];
		partTex[id].data.AvatarIcon = IconShot.Instance.GetIconPic(tarObj);
		Object.DestroyObject(tarObj);
		CheckRefreshFinish();
		yield return 0;
	}

	private void CheckRefreshFinish()
	{
		if (texCountToLoad_Official == 0 && texCountToLoad_Suit == 0 && texCountToLoad_Part == 0 && UIMarket.Instance != null)
		{
			UIMarket.Instance.RefreshFinish();
		}
	}
}
