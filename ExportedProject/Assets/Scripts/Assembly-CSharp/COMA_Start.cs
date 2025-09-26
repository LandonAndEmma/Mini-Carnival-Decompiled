using UnityEngine;

public class COMA_Start : MonoBehaviour
{
	private int tmr;

	private void Awake()
	{
		StopAllCoroutines();
		Debug.Log("========================================================Extract Config!");
		TextAsset[] array = Resources.LoadAll<TextAsset>("RPG_Levels");
		for (int i = 0; i < array.Length; i++)
		{
			if (!COMA_FileIO.IsExistFile("Levels", array[i].name + ".xml"))
			{
				COMA_FileIO.SaveFile("Levels", array[i].name + ".xml", array[i].text);
			}
		}
		TextAsset[] array2 = Resources.LoadAll<TextAsset>("RPG_Config");
		for (int j = 0; j < array2.Length; j++)
		{
			string empty = string.Empty;
			empty = ((!(array2[j].name == "LevelIncome") && !(array2[j].name == "MAXEXP") && !(array2[j].name == "pointincome")) ? (array2[j].name + ".xml") : (array2[j].name + ".csv"));
			if (!COMA_FileIO.IsExistFile("Configs", empty))
			{
				COMA_FileIO.SaveFile("Configs", empty, array2[j].text);
			}
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (tmr == 5)
		{
			Object[] array = Object.FindObjectsOfType(typeof(GameObject));
			for (int num = array.Length - 1; num >= 0; num--)
			{
				if (array[num].GetInstanceID() != base.gameObject.GetInstanceID())
				{
					Object.DestroyObject(array[num]);
				}
			}
			ChartboostPlugin.ResetShowNumber();
			TapjoyPlugin.ResetEventDict();
			TAudioController.bSound = true;
			TAudioManager.ResetInstance();
			COMA_AudioManager.ResetInstance();
			COMA_Buff.ResetInstance();
			COMA_TimeAtlas.ResetInstance();
			COMA_CommonOperation.ResetInstance();
			COMA_FileNameManager.ResetInstance();
			COMA_Pref.ResetInstance();
			COMA_IAPCheck.ResetInstance();
			COMA_Sys.ResetInstance();
			COMA_GC_TID.ResetInstance();
			COMA_TexBase.ResetInstance();
			COMA_PaintBase.ResetInstance();
			COMA_TexLib.ResetInstance();
			COMA_TexBuyBuffer.ResetInstance();
			COMA_TexOnSale.ResetInstance();
			COMA_Scene_Shop.ResetInstance();
			COMA_Scene_Trade.ResetInstance();
			COMA_CommandHandler.ResetInstance();
			COMA_HTTP_DataCollect.ResetInstance();
			COMA_Network.ResetInstance();
			COMA_NetworkConnect.ResetInstance();
			COMA_ServerManager.ResetInstance();
			CLoginManager.ResetInstance();
			iGameCenter.ResetInstance();
			iIAPManager.ResetInstance();
			iServerConfigData.ResetInstance();
			iServerDataManager.ResetInstance();
			iServerFile.ResetInstance();
			iServerIAPVerify.ResetInstance();
			iServerIAPVerifyBackground.ResetInstance();
			iServerSaveData.ResetInstance();
			iServerVerify.ResetInstance();
			COMA_InstanceManager.Instance.Reset();
			TEntityMgr.Instance.ResetInstance();
			TPCInputMgr.Instance.ResetInstance();
		}
		else if (tmr != 6)
		{
			if (tmr == 10)
			{
				Resources.UnloadUnusedAssets();
			}
			else if (tmr == 20)
			{
				Application.LoadLevel("COMA_Login");
			}
		}
		tmr++;
	}
}
