using System;
using UnityEngine;

[Serializable]
public class PropsLib
{
	public Texture2D _tex;

	public int _id;

	public void DelayAssignment(Texture2D tex)
	{
		_tex = tex;
	}
}
