using MessageID;
using Protocol.Rank.C2S;
using UnityEngine;

public class COMA_Flappy_SceneController : TBaseEntity
{
	private static COMA_Flappy_SceneController _instance;

	public Transform blockNodeTrs;

	public GameObject blockObj;

	public GameObject startUIObj;

	public GameObject endUIObj;

	public TUIMeshSprite[] uiScore;

	public Animation foregroundAnim;

	public Animation screenFlashAnim;

	public Animation cameraShakeAnim;

	public TAudioEffectRandom hitAudio;

	public static COMA_Flappy_SceneController Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		GetScoreCmd getScoreCmd = new GetScoreCmd();
		getScoreCmd.m_rank_name = COMA_CommonOperation.Instance.SceneIDToRankID(10);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, getScoreCmd);
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

	private new void OnEnable()
	{
		_instance = this;
	}

	private new void OnDisable()
	{
		_instance = null;
	}

	private void Start()
	{
		COMA_AudioManager.Instance.MusicPlay(AudioCategory.BGM_Scene_Castle);
		COMA_Sys.Instance.bCoverUpdate = true;
		Resources.UnloadUnusedAssets();
	}

	private void OnDestroy()
	{
		if (TPCInputMgr.Instance != null)
		{
			TPCInputMgr.Instance.UnregisterPCInput(COMA_MsgSec.PCInput, this);
		}
	}

	public void ReadyToStart()
	{
		endUIObj.SetActive(false);
		startUIObj.SetActive(true);
		for (int i = 0; i < uiScore.Length; i++)
		{
			uiScore[i].gameObject.SetActive(true);
		}
		for (int num = blockNodeTrs.childCount - 1; num >= 0; num--)
		{
			Object.DestroyObject(blockNodeTrs.GetChild(num).gameObject);
		}
		COMA_PlayerSelf.Instance.OnRelive();
		COMA_PlayerSelf.Instance.score = 0;
		UpdateCurrentScore(COMA_PlayerSelf.Instance.score);
		foregroundAnim.Play();
		COMA_PlayerSelf.Instance.characterCom.animation.Play();
	}

	public bool StartIndeed()
	{
		startUIObj.SetActive(false);
		SceneTimerInstance.Instance.Add(1.6f, CreateBlock);
		COMA_PlayerSelf.Instance.UI_Jump();
		COMA_Sys.Instance.bCoverUpdate = false;
		return false;
	}

	public bool CreateBlock()
	{
		GameObject gameObject = Object.Instantiate(blockObj) as GameObject;
		gameObject.transform.parent = blockNodeTrs;
		gameObject.transform.localPosition = new Vector3(Random.Range(-2.5f, 1f), 0f, 0f);
		return true;
	}

	private new void Update()
	{
		for (int num = blockNodeTrs.childCount - 1; num >= 0; num--)
		{
			if (blockNodeTrs.GetChild(num).GetChild(0).localPosition.y < -11f)
			{
				Object.DestroyObject(blockNodeTrs.GetChild(num).gameObject);
			}
		}
	}

	public void Stop()
	{
		screenFlashAnim.Play();
		cameraShakeAnim.Play();
		if (COMA_AudioManager.Instance.bSound)
		{
			hitAudio.Trigger();
		}
		SceneTimerInstance.Instance.Remove(CreateBlock);
		Animation[] componentsInChildren = blockNodeTrs.GetComponentsInChildren<Animation>();
		Animation[] array = componentsInChildren;
		foreach (Animation animation in array)
		{
			animation.Stop();
		}
		foregroundAnim.Stop();
	}

	public void Ground()
	{
		COMA_Sys.Instance.bCoverUpdate = true;
		endUIObj.SetActive(true);
		for (int i = 0; i < uiScore.Length; i++)
		{
			uiScore[i].gameObject.SetActive(false);
		}
		COMA_PlayerSelf.Instance.characterCom.animation.Stop();
	}

	public void UpdateCurrentScore(int score)
	{
		if (score > 9999)
		{
			score = 9999;
		}
		for (int i = 0; i < uiScore.Length; i++)
		{
			int num = score % 10;
			uiScore[i].texture = "number" + num;
			score /= 10;
		}
	}
}
