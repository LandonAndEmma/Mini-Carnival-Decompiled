using System;
using UnityEngine;

[Serializable]
public class TagObjectData
{
	public static float _thresholdAngree;

	public static Vector2 _area;

	[SerializeField]
	protected int _nId;

	[SerializeField]
	protected Transform _trans;

	[SerializeField]
	protected GameObject _tagObj;

	[SerializeField]
	protected int _nDis;

	[SerializeField]
	protected float _fDegrees;

	public Vector3 Pos
	{
		get
		{
			return _trans.position;
		}
	}

	public int Dis
	{
		get
		{
			return _nDis;
		}
		set
		{
			_nDis = value;
		}
	}

	public float DegreesWithRole
	{
		get
		{
			return _fDegrees;
		}
		set
		{
			_fDegrees = value;
		}
	}

	public GameObject TagObj
	{
		get
		{
			return _tagObj;
		}
		set
		{
			_tagObj = value;
		}
	}

	public int Id
	{
		get
		{
			return _nId;
		}
		set
		{
			_nId = value;
		}
	}

	public TagObjectData(Transform trans)
	{
		_trans = trans;
		_tagObj = null;
	}

	public void RefreshUI()
	{
		TagObj.GetComponent<UIInGame_DirTag>().RefreshDis(Dis);
		TagObj.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, DegreesWithRole));
		DegreesWithRole %= 360f;
		if (DegreesWithRole < 0f)
		{
			DegreesWithRole = 360f + DegreesWithRole;
		}
		if ((DegreesWithRole >= 0f && DegreesWithRole < _thresholdAngree) || (DegreesWithRole >= 360f - _thresholdAngree && DegreesWithRole <= 360f))
		{
			Vector3 localPosition = new Vector3(0f, _area.y, 0f);
			localPosition.x = Mathf.Tan(DegreesWithRole * ((float)Math.PI / 180f)) * -1f * _area.y;
			TagObj.transform.localPosition = localPosition;
		}
		else if (DegreesWithRole >= _thresholdAngree && DegreesWithRole < _thresholdAngree + 2f * (90f - _thresholdAngree))
		{
			Vector3 localPosition2 = new Vector3(0f - _area.x, 0f, 0f);
			localPosition2.y = _area.x / Mathf.Tan(DegreesWithRole * ((float)Math.PI / 180f));
			TagObj.transform.localPosition = localPosition2;
		}
		else if (DegreesWithRole >= _thresholdAngree + 2f * (90f - _thresholdAngree) && DegreesWithRole < 180f + _thresholdAngree)
		{
			Vector3 localPosition3 = new Vector3(0f, 0f - _area.y, 0f);
			localPosition3.x = Mathf.Tan(DegreesWithRole * ((float)Math.PI / 180f)) * _area.y;
			TagObj.transform.localPosition = localPosition3;
		}
		else
		{
			Vector3 localPosition4 = new Vector3(_area.x, 0f, 0f);
			localPosition4.y = _area.x / Mathf.Tan(DegreesWithRole * ((float)Math.PI / 180f)) * -1f;
			TagObj.transform.localPosition = localPosition4;
		}
	}
}
