using UnityEngine;

public class RPGBuff_IconOnly : RPGTBuff
{
	protected string _strEffectPath = string.Empty;

	public string EffectPath_Icon
	{
		get
		{
			return _strEffectPath;
		}
		set
		{
			_strEffectPath = value;
		}
	}

	protected new void Awake()
	{
		base.Awake();
		base.ConfId = 50;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		return true;
	}

	protected override void InitBufferEffect()
	{
		if (EffectPath_Icon != string.Empty)
		{
			Object obj = Resources.Load(EffectPath_Icon);
			if (obj != null)
			{
				Debug.Log(EffectPath_Icon);
				GameObject gameObject = Object.Instantiate(obj) as GameObject;
				gameObject.transform.parent = base.RPGEntityOwner.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
				AddSpecBuffEffectObj(gameObject);
			}
		}
	}
}
