using UnityEngine;

public class UI_Pallet : UICOM
{
	public delegate void BtnOKCallHandler(Color c);

	[SerializeField]
	private UI_Pallet_CurColorMgr _curColorMgr;

	[SerializeField]
	private UI_Pallet_HSMgr _hsMgr;

	[SerializeField]
	private UI_Pallet_LMgr _lMgr;

	public event BtnOKCallHandler ProceYesHandler;

	private void Start()
	{
	}

	private new void Update()
	{
	}

	public void InitPalletColor(Color c)
	{
		_hsMgr.InitHS(c);
		_lMgr.InitL(c);
	}

	public void HandleEventButton_ok(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_ok-CommandClick");
			Debug.Log("----------Sel Color:" + _curColorMgr.CurSelColor);
			COMA_PaintBase.Instance.curPaint = _curColorMgr.CurSelColor;
			COMA_Pref.Instance.Save(true);
			if (this.ProceYesHandler != null)
			{
				this.ProceYesHandler(_curColorMgr.CurSelColor);
			}
			base.gameObject.SetActive(false);
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
}
