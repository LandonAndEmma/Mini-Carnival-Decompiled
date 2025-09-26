using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class COMA_Castle_SceneController : MonoBehaviour
{
	private static COMA_Castle_SceneController _instance;

	public UIInGame_CrystalVolumeMgr targetCrystalCom;

	public UI_HungerRedMgr redScreenCom;

	public TextAsset waves;

	private int[] enemy_score = new int[4];

	private float[] enemy_hp = new float[4];

	private float[] enemy_hpMax = new float[4];

	private float[] enemy_ap = new float[4];

	private float[] enemy_apMax = new float[4];

	private float[] enemy_speed = new float[4];

	private float[] enemy_speedMax = new float[4];

	private float[] enemy_view = new float[4];

	public GameObject[] enemyPrefabs;

	private List<int>[] enemyToInit = new List<int>[4]
	{
		new List<int>(),
		new List<int>(),
		new List<int>(),
		new List<int>()
	};

	private int waveCur;

	private float waveStronger = 1f;

	private float waveStrongerCur = 1f;

	private int waveNumber;

	private int waveNumberMax = 30;

	private int waveNumberAdd;

	private int enemyIDToName = 1;

	public Transform[] regionTrs;

	private Vector3[] rpMin;

	private Vector3[] rpMax;

	private bool isSync;

	public Transform playerNodeTrs;

	public Transform targetTrs;

	private COMA_PlayerSelf_Castle playerCastleCom;

	public static COMA_Castle_SceneController Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		if (COMA_Platform.Instance != null)
		{
			COMA_Platform.Instance.DestroyPlatform();
		}
		if (COMA_CommonOperation.Instance.selectedWeaponPrice > 0)
		{
			COMA_Pref.Instance.AddGold(-COMA_CommonOperation.Instance.selectedWeaponPrice);
			COMA_Pref.Instance.Save(true);
		}
		else if (COMA_CommonOperation.Instance.selectedWeaponPrice < 0)
		{
			COMA_Pref.Instance.AddCrystal(COMA_CommonOperation.Instance.selectedWeaponPrice);
			COMA_Pref.Instance.Save(true);
		}
	}

	private void OnEnable()
	{
		_instance = this;
	}

	private void OnDisable()
	{
		_instance = null;
	}

	private void Start()
	{
		COMA_AudioManager.Instance.MusicPlay(AudioCategory.BGM_Scene_Castle);
		float lastTime = COMA_Camera.Instance.SceneStart_CameraAnim(false);
		SceneTimerInstance.Instance.Add(lastTime, CountToStart);
		COMA_Sys.Instance.bCoverUIInput = true;
		playerCastleCom = playerNodeTrs.GetComponentInChildren<COMA_PlayerSelf_Castle>();
		if (playerCastleCom == null)
		{
			Debug.Log("playerCastleCom is null!!");
		}
		rpMin = new Vector3[regionTrs.Length];
		rpMax = new Vector3[regionTrs.Length];
		for (int i = 0; i < regionTrs.Length; i++)
		{
			rpMin[i] = regionTrs[i].position - regionTrs[i].localScale * 0.5f;
			rpMax[i] = regionTrs[i].position + regionTrs[i].localScale * 0.5f;
			Object.DestroyObject(regionTrs[i].gameObject);
		}
		XmlNode xmlNode = COMA_Sys.Instance.ParseXml(waves);
		XmlNode xmlNode2 = xmlNode["Wave"];
		for (int j = 0; j < xmlNode2.ChildNodes.Count; j++)
		{
			XmlElement xmlElement = xmlNode2.ChildNodes[j] as XmlElement;
			enemy_score[j] = int.Parse(xmlElement.GetAttribute("score"));
			enemy_hp[j] = float.Parse(xmlElement.GetAttribute("hp"));
			enemy_hpMax[j] = float.Parse(xmlElement.GetAttribute("hpMax"));
			enemy_ap[j] = float.Parse(xmlElement.GetAttribute("ap"));
			enemy_apMax[j] = float.Parse(xmlElement.GetAttribute("apMax"));
			enemy_speed[j] = float.Parse(xmlElement.GetAttribute("speed"));
			enemy_speedMax[j] = float.Parse(xmlElement.GetAttribute("speedMax"));
			enemy_view[j] = float.Parse(xmlElement.GetAttribute("view"));
		}
		XmlElement xmlElement2 = xmlNode["Tough"];
		waveStronger = float.Parse(xmlElement2.GetAttribute("stronger"));
		waveNumber = int.Parse(xmlElement2.GetAttribute("number"));
		waveNumberAdd = int.Parse(xmlElement2.GetAttribute("more"));
		Resources.UnloadUnusedAssets();
	}

	public bool CountToStart()
	{
		COMA_Scene.Instance.CountToStart(3);
		SceneTimerInstance.Instance.Add(5f, StartIndeed);
		return false;
	}

	public bool StartIndeed()
	{
		CreateWave();
		SceneTimerInstance.Instance.Add(5f, CreateWave);
		SceneTimerInstance.Instance.Add(2f, CreateEnemy);
		return false;
	}

	public bool CreateWave()
	{
		if (playerNodeTrs.childCount > 1)
		{
			return true;
		}
		for (int i = 0; i < enemyToInit.Length; i++)
		{
			if (enemyToInit[i].Count > 0)
			{
				return true;
			}
		}
		waveCur++;
		waveStrongerCur *= waveStronger;
		int num = Random.Range(1, 4);
		List<int> list = new List<int>();
		list.Add(0);
		list.Add(1);
		list.Add(2);
		list.Add(3);
		while (num > 0)
		{
			list.RemoveAt(Random.Range(0, list.Count));
			num--;
		}
		waveNumber += waveNumberAdd;
		if (waveNumber > waveNumberMax)
		{
			waveNumber = waveNumberMax;
		}
		for (int j = 0; j < waveNumber; j++)
		{
			int num2 = list[Random.Range(0, list.Count)];
			int item = 0;
			if (waveCur == 3)
			{
				float num3 = Random.Range(0f, 1f);
				if (num3 > 0.6f)
				{
					item = 1;
				}
			}
			else if (waveCur == 4)
			{
				float num4 = Random.Range(0f, 1f);
				if (num4 > 0.9f)
				{
					item = 2;
				}
				else if (num4 > 0.5f)
				{
					item = 1;
				}
			}
			else if (waveCur >= 5)
			{
				float num5 = Random.Range(0f, 1f);
				if (num5 > 0.8f)
				{
					item = 3;
				}
				else if (num5 > 0.7f)
				{
					item = 2;
				}
				else if (num5 > 0.4f)
				{
					item = 1;
				}
			}
			enemyToInit[num2].Add(item);
		}
		return true;
	}

	public bool CreateEnemy()
	{
		for (int i = 0; i < enemyToInit.Length; i++)
		{
			if (enemyToInit[i].Count > 0)
			{
				int num = enemyToInit[i][0];
				GameObject gameObject = Object.Instantiate(position: new Vector3(Random.Range(rpMin[i].x, rpMax[i].x), Random.Range(rpMin[i].y, rpMax[i].y), Random.Range(rpMin[i].z, rpMax[i].z)), original: enemyPrefabs[num], rotation: Quaternion.identity) as GameObject;
				gameObject.name = enemyIDToName.ToString();
				enemyIDToName++;
				gameObject.transform.parent = playerNodeTrs;
				COMA_Enemy_Zombie component = gameObject.GetComponent<COMA_Enemy_Zombie>();
				component.HP = enemy_hp[num] * waveStrongerCur;
				if (component.HP > enemy_hpMax[num])
				{
					component.HP = enemy_hpMax[num];
				}
				component.AP = enemy_ap[num] * waveStrongerCur;
				if (component.AP > enemy_apMax[num])
				{
					component.AP = enemy_apMax[num];
				}
				component.speedRun = Random.Range(enemy_speed[num], enemy_speedMax[num]);
				component.score = enemy_score[num];
				component.view = enemy_view[num];
				component.targetToAttack = targetTrs;
				component.SetSync(isSync);
				component.playerCastleCom = playerCastleCom;
				enemyToInit[i].RemoveAt(0);
			}
		}
		return true;
	}

	public void StartRedScreen()
	{
		redScreenCom.gameObject.SetActive(true);
		SceneTimerInstance.Instance.Add(0.5f, EndRedScreen);
	}

	public bool EndRedScreen()
	{
		redScreenCom.gameObject.SetActive(false);
		return false;
	}
}
