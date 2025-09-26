using System;
using MC_UIToolKit;
using MessageID;
using Protocol.Role.C2S;
using UIGlobal;
using UnityEngine;

public class UIDoodle1 : UIMessageHandler
{
	[SerializeField]
	private GameObject _pallet;

	[SerializeField]
	private UIDoodle_SelOperate _selOperate;

	[SerializeField]
	private UI_BrushSliderMgr _burshSize;

	[SerializeField]
	private TUIMeshSprite _curUseColor;

	[SerializeField]
	private UI_DoodleLock _curLockState;

	[SerializeField]
	private UI_ColorSelMgr _colorSelMgr;

	[SerializeField]
	private UI_BrushSizeIcon _brushSizeIconCom;

	[SerializeField]
	private UI_EditZoomSliderMgr _editZoomSliderCom;

	public BodyPaint bodyPaint;

	[NonSerialized]
	public Collider paintTarget;

	private Vector3 paintPoint = Vector3.zero;

	private bool bPainting;

	private bool bColored;

	private int paintState;

	private int prePaintSize;

	private bool bLocked;

	public Transform cmrTrs;

	private float scaleWithZ0 = -1.5f;

	private float scaleWithZ1 = -0.5f;

	private float rotY;

	private float rotX;

	private float posZ;

	private float distance;

	[SerializeField]
	private GameObject[] _wizardMask;

	private float startDistance;

	public void ChangePrePaintSize(int size)
	{
		prePaintSize = size;
	}

	private void ProcessStartNW()
	{
		GameObject[] wizardMask = _wizardMask;
		foreach (GameObject gameObject in wizardMask)
		{
			gameObject.SetActive(false);
		}
	}

	private void Awake()
	{
		UIGolbalStaticFun.CloseIAPBlockMessageBox();
		if (_wizardMask[0] != null)
		{
			if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 9)
			{
				_wizardMask[0].GetComponent<UI_NewcomersWizardMask>().ProceFullScreenBtn += ProcessStartNW;
				GameObject[] wizardMask = _wizardMask;
				foreach (GameObject gameObject in wizardMask)
				{
					gameObject.SetActive(true);
				}
			}
			else
			{
				GameObject[] wizardMask2 = _wizardMask;
				foreach (GameObject gameObject2 in wizardMask2)
				{
					gameObject2.SetActive(false);
				}
			}
		}
		TUITextManager.Instance().Parser("UI/language.en", "UI/language.en");
		UIBackpack_BoxData.EDataType dataType = (UIBackpack_BoxData.EDataType)UIDataBufferCenter.Instance.SelectBoxDataForDesign.DataType;
		EAvatarPart part = UIGolbalStaticFun.UIBackpackAvatarTypeToAvatarPart(dataType);
		GameObject gameObject3 = UnityEngine.Object.Instantiate(Resources.Load(UIGolbalStaticFun.GetPaintResPathByAvatarPart(part))) as GameObject;
		gameObject3.transform.parent = bodyPaint.transform;
		gameObject3.transform.localPosition = Vector3.zero;
		gameObject3.transform.localEulerAngles = Vector3.zero;
		MeshRenderer componentInChildren = gameObject3.transform.FindChild("mesh").GetComponentInChildren<MeshRenderer>();
		paintTarget = componentInChildren.gameObject.AddComponent<MeshCollider>();
		Debug.Log("MD5:" + UIDataBufferCenter.Instance.SelectBoxDataForDesign.Unit);
		if (UIDataBufferCenter.Instance.SelectBoxDataForDesign.Unit == string.Empty)
		{
			bodyPaint.tex2D = UIGolbalStaticFun.CreateWhiteTexture();
			bodyPaint.tex2D.filterMode = FilterMode.Point;
			paintTarget.renderer.material.mainTexture = bodyPaint.tex2D;
			bodyPaint.StartInit();
		}
		else
		{
			UIDataBufferCenter.Instance.FetchFileByMD5(UIDataBufferCenter.Instance.SelectBoxDataForDesign.Unit, delegate(byte[] buffer)
			{
				Debug.Log(buffer.Length);
				Texture2D texture2D = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
				texture2D.LoadImage(buffer);
				texture2D.Apply(false);
				Debug.Log(texture2D.width);
				bodyPaint.tex2D = texture2D;
				bodyPaint.tex2D.filterMode = FilterMode.Point;
				paintTarget.renderer.material.mainTexture = bodyPaint.tex2D;
				bodyPaint.StartInit();
			});
		}
		ChangePrePaintSize(GetCurBurshSize() - 1);
		rotY = cmrTrs.parent.parent.localEulerAngles.y;
		rotX = cmrTrs.parent.localEulerAngles.x;
		posZ = cmrTrs.localPosition.z;
		distance = posZ;
	}

	public int GetCurOperate()
	{
		return (int)_selOperate._curSel;
	}

	public int GetCurBurshSize()
	{
		return _burshSize._nSize;
	}

	public Color GetCurUseColor()
	{
		return _curUseColor.color;
	}

	public bool IsLocked()
	{
		return _curLockState.IsLocked();
	}

	public void NotifyDraw()
	{
		_colorSelMgr.NotifyDraw();
	}

	public void NotifyZoomMode(float f)
	{
		float cameraScale = Mathf.Lerp(scaleWithZ0, scaleWithZ1, f);
		SetCameraScale(cameraScale);
	}

	private void Start()
	{
		if (!COMA_Sys.Instance.bMemFirstGame)
		{
			COMA_Achievement.Instance.Apprentice++;
		}
		float f = Mathf.InverseLerp(scaleWithZ0, scaleWithZ1, cmrTrs.localPosition.z);
		_editZoomSliderCom.InitPointerPos(f);
	}

	private new void Update()
	{
	}

	public void HandleEventButton_back(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Back);
			if (bodyPaint.tex2D != null)
			{
				UIDataBufferCenter.Instance.UploadFile(UIDataBufferCenter.Instance.SelectBoxDataForDesign.ItemId, bodyPaint.tex2D.EncodeToPNG(), delegate(string texMD5)
				{
					UIDataBufferCenter.Instance.SelectBoxDataForDesign.Unit = texMD5;
					ModifyBagItemCmd extraInfo = new ModifyBagItemCmd
					{
						m_unique_id = UIDataBufferCenter.Instance.SelectBoxDataForDesign.ItemId,
						m_unit = texMD5
					};
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, extraInfo);
					UIGolbalStaticFun.CloseBlockOnlyMessageBox();
				});
				UnityEngine.Object.DestroyObject(base.transform.root.gameObject);
			}
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_openPallet(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			_pallet.SetActive(true);
			_pallet.GetComponent<UI_Pallet>().InitPalletColor(_colorSelMgr.GetCurrentColor());
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}

	public void HandleEventButton_undo(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			bodyPaint.Undo();
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void NotifyLockStateChanged()
	{
		Debug.Log("Lock or NotLock");
		bLocked = IsLocked();
	}

	public void NotifyFrontState()
	{
		Debug.Log("Front View");
		cmrTrs.parent.parent.localEulerAngles = new Vector3(0f, rotY, 0f);
		cmrTrs.parent.localEulerAngles = new Vector3(rotX, 0f, 0f);
		cmrTrs.localPosition = new Vector3(0f, 0f, posZ);
	}

	public void NotifyOperatorChanged()
	{
		paintState = GetCurOperate();
		if (paintState == 2)
		{
			_burshSize.ChangePointerPos(5);
		}
		else
		{
			_burshSize.ChangePointerPos(prePaintSize);
		}
	}

	private void ResetCameraToFront()
	{
		cmrTrs.parent.parent.localEulerAngles = new Vector3(0f, rotY, 0f);
		cmrTrs.parent.localEulerAngles = new Vector3(rotX, 0f, 0f);
		cmrTrs.localPosition = new Vector3(0f, 0f, posZ);
	}

	private void SetCameraScale(float z)
	{
		cmrTrs.localPosition = new Vector3(0f, 0f, z);
	}

	private void CalcPaintPoint(float fW, float fL)
	{
		paintPoint.x = fW * (float)Screen.width;
		paintPoint.y = fL * (float)Screen.height;
	}

	public void HandleEventButton_RotateCamera(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (bodyPaint.tex2D == null)
		{
			Debug.LogWarning("贴图尚未载入，禁止绘画");
			return;
		}
		switch (eventType)
		{
		case 1:
			bodyPaint.isNoise = false;
			if (paintState == 0 || paintState == 2)
			{
				if (paintState == 2)
				{
					bodyPaint.isNoise = true;
				}
				bPainting = false;
				bodyPaint.paintRadius = GetCurBurshSize();
				bodyPaint.paintColor = GetCurUseColor();
				CalcPaintPoint(wparam, lparam);
				Ray ray = cmrTrs.camera.ScreenPointToRay(paintPoint);
				RaycastHit hitInfo;
				if (paintTarget.Raycast(ray, out hitInfo, 10f))
				{
					bPainting = true;
					bColored = true;
					bodyPaint.PaintStart(hitInfo.textureCoord);
				}
			}
			else if (paintState == 1)
			{
				CalcPaintPoint(wparam, lparam);
				Ray ray2 = cmrTrs.camera.ScreenPointToRay(paintPoint);
				RaycastHit hitInfo2;
				if (paintTarget.Raycast(ray2, out hitInfo2, 10f))
				{
					Color pixColor = bodyPaint.GetPixColor(hitInfo2.textureCoord);
					Debug.Log(pixColor);
					_brushSizeIconCom.RefreshColor(pixColor);
					COMA_PaintBase.Instance.curPaint = pixColor;
					COMA_Pref.Instance.Save(true);
				}
			}
			else if (paintState == 3)
			{
				CalcPaintPoint(wparam, lparam);
				Ray ray3 = cmrTrs.camera.ScreenPointToRay(paintPoint);
				RaycastHit hitInfo3;
				if (paintTarget.Raycast(ray3, out hitInfo3, 10f))
				{
					bodyPaint.paintColor = GetCurUseColor();
					bodyPaint.Full();
				}
				_selOperate.AutoSelectBrushFromBucket();
			}
			break;
		case 2:
			if (bLocked || bPainting)
			{
				paintPoint.x += wparam * (float)Screen.width;
				paintPoint.y += lparam * (float)Screen.height;
				Ray ray4 = cmrTrs.camera.ScreenPointToRay(paintPoint);
				RaycastHit hitInfo4;
				if (paintTarget.Raycast(ray4, out hitInfo4, 10f))
				{
					if (!bColored)
					{
						bColored = true;
						bodyPaint.PaintStart(hitInfo4.textureCoord);
					}
					else
					{
						bodyPaint.Paint(hitInfo4.textureCoord);
					}
				}
			}
			else
			{
				Quaternion quaternion = Quaternion.Euler(0f, wparam * (float)Screen.width, 0f);
				cmrTrs.parent.parent.rotation *= quaternion;
				Quaternion quaternion2 = Quaternion.Euler((0f - lparam) * (float)Screen.height, 0f, 0f);
				cmrTrs.parent.localRotation *= quaternion2;
			}
			break;
		case 3:
			if (bColored)
			{
				bodyPaint.PaintEnd();
			}
			bPainting = false;
			bColored = false;
			break;
		}
	}
}
