using System.Collections.Generic;
using UnityEngine;

public class UIInGame_PropsBox : MonoBehaviour
{
	[SerializeField]
	private UI_3DModeToTexture _3dModeToTex;

	[SerializeField]
	private TUIMeshSprite _propsIcon;

	[SerializeField]
	private Animation _ani;

	[SerializeField]
	private TUILabel _propNum;

	[SerializeField]
	private int _propsCount = 5;

	private int _curPropsIndex = -1;

	private int _curUseCount;

	[SerializeField]
	private List<PropsLib> _lstPropsTex = new List<PropsLib>();

	private static UIInGame_PropsBox _instance;

	public bool _bTest;

	private bool _bEnableTest;

	public bool _bGet = true;

	public int propsID;

	public int CurUseCount
	{
		get
		{
			return _curUseCount;
		}
		set
		{
			if (value > 1)
			{
				_propNum.Text = value.ToString();
				_propNum.gameObject.SetActive(true);
			}
			else
			{
				_propNum.gameObject.SetActive(false);
			}
			_curUseCount = value;
		}
	}

	public static UIInGame_PropsBox Instance
	{
		get
		{
			return _instance;
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

	private void Awake()
	{
		CurUseCount = 0;
	}

	private void Start()
	{
		base.gameObject.GetComponent<TUIMeshSprite>().color = new Color(1f, 1f, 1f, 0.5f);
		for (int i = 0; i < _propsCount; i++)
		{
			PropsLib propsLib = new PropsLib();
			propsLib._tex = _3dModeToTex.GetTexById(i, propsLib.DelayAssignment);
			_lstPropsTex.Add(propsLib);
		}
	}

	public void Init(Texture2D[] texs)
	{
		for (int i = 0; i < texs.Length; i++)
		{
			PropsLib propsLib = new PropsLib();
			propsLib._tex = texs[i];
			_lstPropsTex.Add(propsLib);
		}
	}

	public void HandleEventButton_props(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (COMA_Sys.Instance.bCoverUIInput)
		{
			return;
		}
		switch (eventType)
		{
		case 3:
			if (!(_propsIcon != null))
			{
				break;
			}
			Debug.Log("----------------------------->Use props!! " + CurUseCount);
			if (COMA_PlayerSelf.Instance.UseItem(_curPropsIndex) == 0)
			{
				CurUseCount--;
				if (CurUseCount <= 0)
				{
					DelProps();
				}
			}
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			base.gameObject.GetComponent<TUIMeshSprite>().color = new Color(1f, 1f, 1f, 1f);
			break;
		case 2:
			base.gameObject.GetComponent<TUIMeshSprite>().color = new Color(1f, 1f, 1f, 0.5f);
			break;
		}
	}

	private void Update()
	{
		if (_bEnableTest && _bTest)
		{
			if (_bGet)
			{
				GetProps(propsID);
			}
			else
			{
				DelProps();
			}
			_bTest = false;
		}
	}

	public void GetProps(int index)
	{
		GetProps(index, 1);
	}

	public void GetProps(int index, int useCount)
	{
		DelProps();
		_curPropsIndex = index;
		CurUseCount = useCount;
		if (!(_propsIcon != null))
		{
			return;
		}
		_propsIcon.UseCustomize = true;
		if (index >= 0 && index < _lstPropsTex.Count)
		{
			_propsIcon.CustomizeTexture = _lstPropsTex[index]._tex;
			if (_ani != null)
			{
				_ani.Play();
			}
		}
	}

	protected void DelProps()
	{
		_curPropsIndex = -1;
		if (_propsIcon != null)
		{
			_propsIcon.UseCustomize = true;
			_propsIcon.CustomizeTexture = null;
		}
	}
}
