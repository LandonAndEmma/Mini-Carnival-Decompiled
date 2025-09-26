using UnityEngine;

public class UIFishing_GetFishItems : MonoBehaviour
{
	[SerializeField]
	private TUILabel _desLabel;

	[SerializeField]
	private TUILabel _btnLabel;

	[SerializeField]
	private TUIMeshSprite _icon;

	public int _nGoldNum = -1;

	public int _nCrystal = -1;

	public string _strDecoName = string.Empty;

	public bool _bNeedPopup;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetBtnLabel(string str)
	{
		_btnLabel.Text = str;
	}

	public void SetDesLabel(string str)
	{
		_desLabel.TextID = string.Empty;
		_desLabel.Text = str;
	}

	public void SetDesLabelID(string id)
	{
		_desLabel.TextID = id;
	}

	public void SetIcon(string str)
	{
		_icon.texture = str;
	}

	public void HandleEventButton_Yes(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			base.gameObject.SetActive(false);
			if (_bNeedPopup)
			{
				if (_nGoldNum != -1)
				{
					COMA_Pref.Instance.AddGold(_nGoldNum);
					TUI_MsgBox.Instance.TipBox(0, _nGoldNum, string.Empty, null);
				}
				else if (_nCrystal != -1)
				{
					COMA_Pref.Instance.AddCrystal(_nCrystal);
					TUI_MsgBox.Instance.TipBox(1, _nCrystal, string.Empty, null);
				}
				else if (COMA_Pref.Instance.PackageNullCount() < 1)
				{
					COMA_Pref.Instance.AddCrystal(2);
					TUI_MsgBox.Instance.TipBox(1, 2, string.Empty, null);
				}
				else
				{
					COMA_PackageItem cOMA_PackageItem = new COMA_PackageItem();
					cOMA_PackageItem.serialName = _strDecoName;
					cOMA_PackageItem.itemName = string.Empty;
					cOMA_PackageItem.part = COMA_PackageItem.NameToPart(cOMA_PackageItem.serialName);
					cOMA_PackageItem.CreateIconTexture();
					cOMA_PackageItem.state = COMA_PackageItem.PackageItemStatus.None;
					Debug.Log("Fishing Mode: Get :" + _strDecoName);
					COMA_Pref.Instance.GetAnItem(cOMA_PackageItem);
					TUI_MsgBox.Instance.TipBox(2, 1, string.Empty, null, _strDecoName);
				}
			}
			Debug.Log("Button_Yes-CommandClick");
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}
}
