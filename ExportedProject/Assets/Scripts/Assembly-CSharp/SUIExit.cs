using System;
using UnityEngine;

[Serializable]
public class SUIExit
{
	[SerializeField]
	public AnimationClip _clip;

	[SerializeField]
	public GameObject _obj;

	public SUIExit()
	{
		_clip = null;
		_obj = null;
	}
}
