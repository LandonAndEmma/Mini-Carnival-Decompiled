using UnityEngine;

public class UITrade_EquipedBoxSlot : UI_BoxSlot
{
	[SerializeField]
	private GameObject bkNormal;

	[SerializeField]
	private GameObject bkSelected;

	[SerializeField]
	private GameObject doodlePic;

	private bool bCurSelected;

	public bool CurSelected
	{
		get
		{
			return bCurSelected;
		}
		set
		{
			bCurSelected = value;
			NotifyDataUpdate();
		}
	}

	public void SetIconPic(Texture2D pic)
	{
		TUIMeshSprite component = doodlePic.GetComponent<TUIMeshSprite>();
		component.UseCustomize = true;
		component.CustomizeTexture = pic;
		component.CustomizeRect = new Rect(0f, 0f, pic.width, pic.height);
	}

	public void DeleteIconPic()
	{
		TUIMeshSprite component = doodlePic.GetComponent<TUIMeshSprite>();
		Object.DestroyObject(component.CustomizeTexture);
		component.CustomizeTexture = null;
		component.UseCustomize = false;
	}

	private void Awake()
	{
		ProcessSelected(false);
	}

	private void Start()
	{
	}

	private new void Update()
	{
	}

	protected override void ProcessNullData()
	{
		base.ProcessNullData();
	}

	public override int NotifyDataUpdate()
	{
		return 0;
	}

	public void HandleEventButton_Doodle(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("Button_Doodle---CommandClick");
			ProcessSelected(true);
		}
	}

	public UI_BoxSlot ProcessSelected(bool bSel)
	{
		CurSelected = bSel;
		bkSelected.active = bSel;
		if (!CurSelected)
		{
			return null;
		}
		UITrade uITrade = (UITrade)GetTUIMessageHandler();
		if (uITrade != null)
		{
			uITrade.ProcessSelChange(this);
		}
		return this;
	}

	protected override UIMessageHandler GetTUIMessageHandler()
	{
		Transform parent = base.gameObject.transform.parent;
		if (parent != null)
		{
			Transform parent2 = parent.parent;
			if (parent2 != null)
			{
				Transform parent3 = parent2.parent;
				if (parent3 != null)
				{
					Transform parent4 = parent3.parent;
					if (parent4 != null)
					{
						Transform parent5 = parent4.parent;
						if (parent5 != null)
						{
							Transform parent6 = parent5.parent;
							if (parent6 != null)
							{
								return parent6.GetComponent<UIMessageHandler>();
							}
						}
					}
				}
			}
		}
		return null;
	}
}
