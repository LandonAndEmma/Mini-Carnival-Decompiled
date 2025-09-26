using UnityEngine;

public class UI_GetItemUIMgr : MonoBehaviour
{
	[SerializeField]
	private GameObject[] _objTypes;

	private int _nItemType;

	[SerializeField]
	private TUILabel _numLabel;

	[SerializeField]
	private GameObject _objGem;

	[SerializeField]
	private GameObject _objGold;

	private int _nNum;

	[SerializeField]
	private TUILabel _itemCaption;

	private string _strItemName;

	[SerializeField]
	private TUIMeshSprite _itemTexture;

	public int ItemType
	{
		get
		{
			return _nItemType;
		}
		set
		{
			_nItemType = value;
			int num = _objTypes.Length;
			for (int i = 0; i < num; i++)
			{
				if (i == ItemType)
				{
					_objTypes[i].SetActive(true);
				}
				else
				{
					_objTypes[i].SetActive(false);
				}
			}
		}
	}

	public int ItemNum
	{
		get
		{
			return _nNum;
		}
		set
		{
			_nNum = value;
			_numLabel.Text = Mathf.Abs(_nNum).ToString();
			if (_nNum >= 0)
			{
				_objGold.SetActive(true);
				_objGem.SetActive(false);
			}
			else
			{
				_objGold.SetActive(false);
				_objGem.SetActive(true);
			}
		}
	}

	public string ItemName
	{
		get
		{
			return _strItemName;
		}
		set
		{
			_strItemName = value;
			string text = TUITool.StringFormat(TUITextManager.Instance().GetString("jiaoyijiemian_desc16"), _strItemName);
			_itemCaption.Text = text;
		}
	}

	public string ItemName2
	{
		get
		{
			return _strItemName;
		}
		set
		{
			_strItemName = value;
			string text = TUITool.StringFormat(TUITextManager.Instance().GetString("jiaoyijiemian_desc19"), _strItemName);
			_itemCaption.Text = text;
		}
	}

	public Texture2D ItemTexture
	{
		set
		{
			if (value != null)
			{
				_itemTexture.UseCustomize = true;
				_itemTexture.CustomizeTexture = value;
				_itemTexture.CustomizeRect = new Rect(0f, 0f, _itemTexture.CustomizeTexture.width, _itemTexture.CustomizeTexture.height);
			}
		}
	}

	public void SetItemTex(string str)
	{
		_itemTexture.UseCustomize = false;
		_itemTexture.texture = str;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void HandleEventButtonOk(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Object.Destroy(base.gameObject);
			if (UIFriends.Instance != null)
			{
				UIFriends.Instance.ShowRewardRemove();
			}
		}
	}
}
