using UnityEngine;

public class COMA_PlayerSync_Maze : COMA_PlayerSync
{
	private GameObject evilObj;

	public bool bEvil;

	private COMA_Maze_Font goldGetCom;

	private GameObject cameraObj;

	private int markID = -1;

	private bool isRendering;

	private COMA_PlayerSelf_Maze selfCom;

	protected new void OnEnable()
	{
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.EVIL, ReceiveSetEvil);
		base.OnEnable();
		if (UIInGame_DirTagMgr.Instance != null)
		{
			markID = UIInGame_DirTagMgr.Instance.AddTagInfo(base.transform);
		}
	}

	protected new void OnDisable()
	{
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.EVIL, ReceiveSetEvil);
		base.OnDisable();
		if (UIInGame_DirTagMgr.Instance != null && markID >= 0)
		{
			UIInGame_DirTagMgr.Instance.DelTagInfo(markID);
			markID = -1;
		}
	}

	protected void ReceiveSetEvil(COMA_CommandDatas commandDatas)
	{
		if (!(commandDatas.dataSender.Id.ToString() != base.gameObject.name))
		{
			COMA_CD_BeEvil cOMA_CD_BeEvil = commandDatas as COMA_CD_BeEvil;
			SetEvil(cOMA_CD_BeEvil.bEvil);
		}
	}

	protected new void Start()
	{
		base.Start();
		if (goldGetCom == null)
		{
			GameObject gameObject = Object.Instantiate(Resources.Load("FBX/SceneAddition/Maze/Golds/PFB_GoldGet")) as GameObject;
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = new Vector3(0f, 2.3f, 0f);
			gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			gameObject.transform.parent = characterCom.boneTrs_Waist;
			goldGetCom = gameObject.GetComponent<COMA_Maze_Font>();
		}
		cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
		IsRendering isRendering = characterCom.bodyObjs[0].AddComponent<IsRendering>();
		isRendering.Func = IsHeadRendering;
	}

	protected new void Update()
	{
		UpdateShadow();
		if (COMA_PlayerSelf.Instance == null)
		{
			return;
		}
		base.Update();
		goldGetCom.SetNumber(base.score);
		goldGetCom.transform.forward = cameraObj.transform.forward;
		if (selfCom == null)
		{
			selfCom = COMA_PlayerSelf.Instance as COMA_PlayerSelf_Maze;
			UIInGame_DirTagMgr.Instance.HideTagInfo(markID);
		}
		if (selfCom.bEvil)
		{
			if (isRendering)
			{
				if ((base.transform.position - COMA_PlayerSelf.Instance.transform.position).magnitude < 3f)
				{
					UIInGame_DirTagMgr.Instance.HideTagInfo(markID);
				}
				else
				{
					UIInGame_DirTagMgr.Instance.ShowTagInfo(markID);
				}
			}
			else
			{
				UIInGame_DirTagMgr.Instance.ShowTagInfo(markID);
			}
		}
		else
		{
			UIInGame_DirTagMgr.Instance.HideTagInfo(markID);
		}
		isRendering = false;
	}

	public void SetEvil(bool evil)
	{
		bEvil = evil;
		if (evil)
		{
			evilObj = Object.Instantiate(Resources.Load("FBX/SceneAddition/Maze/PumpkinHead")) as GameObject;
			evilObj.transform.parent = characterCom.head_top.parent;
			evilObj.transform.localPosition = new Vector3(0.8f, 0f, 0f);
			evilObj.transform.localEulerAngles = new Vector3(0f, 270f, 180f);
			characterCom.bodyObjs[0].SetActive(false);
			characterCom.bodyObjs[1].renderer.material.mainTexture = Object.Instantiate(Resources.Load("FBX/SceneAddition/Maze/Tex_Body")) as Texture2D;
			characterCom.bodyObjs[2].renderer.material.mainTexture = Object.Instantiate(Resources.Load("FBX/SceneAddition/Maze/Tex_Leg")) as Texture2D;
			return;
		}
		if (evilObj != null)
		{
			Object.DestroyObject(evilObj);
		}
		characterCom.bodyObjs[0].SetActive(true);
		string key = base.gameObject.name + "_1";
		Texture2D value = null;
		if (COMA_TexLib.Instance.currentRoomPlayerTextures.TryGetValue(key, out value))
		{
			characterCom.bodyObjs[1].renderer.material.mainTexture = value;
		}
		else
		{
			characterCom.bodyObjs[1].renderer.material.mainTexture = COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[1]].texture;
		}
		string key2 = base.gameObject.name + "_2";
		Texture2D value2 = null;
		if (COMA_TexLib.Instance.currentRoomPlayerTextures.TryGetValue(key2, out value2))
		{
			characterCom.bodyObjs[2].renderer.material.mainTexture = value2;
		}
		else
		{
			characterCom.bodyObjs[2].renderer.material.mainTexture = COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[2]].texture;
		}
	}

	public void IsHeadRendering()
	{
		isRendering = true;
	}

	public override void CharacterCall_Fire()
	{
		GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Swoop/Swoop")) as GameObject;
		gameObject.transform.position = base.transform.position + base.transform.forward * 0.8f;
		gameObject.transform.rotation = base.transform.rotation;
		Object.DestroyObject(gameObject, 1f);
	}
}
