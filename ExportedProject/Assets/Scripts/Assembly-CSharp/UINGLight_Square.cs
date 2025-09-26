using System.Collections.Generic;
using UnityEngine;

public class UINGLight_Square : UIEntity
{
	[SerializeField]
	protected UISprite _light;

	[SerializeField]
	protected UIWidget[] _needLightsObj;

	protected List<int> _oriDepthLst = new List<int>();

	protected override void Load()
	{
	}

	protected override void UnLoad()
	{
	}

	protected void Awake()
	{
		if (_light != null)
		{
			_light.enabled = false;
		}
	}

	protected override void Tick()
	{
	}

	protected void LightOn()
	{
		if (_light != null)
		{
			_light.enabled = true;
		}
		_oriDepthLst.Clear();
		for (int i = 0; i < _needLightsObj.Length; i++)
		{
			_oriDepthLst.Add(_needLightsObj[i].depth);
			_needLightsObj[i].depth += 610;
		}
	}

	protected void LightOff()
	{
		if (_light != null)
		{
			_light.enabled = false;
		}
		for (int i = 0; i < _needLightsObj.Length; i++)
		{
			_needLightsObj[i].depth = _oriDepthLst[i];
		}
	}
}
