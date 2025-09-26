using System;
using UnityEngine;

public class TAudioAnimEvt : ScriptableObject
{
	[Serializable]
	public class AnimEvt
	{
		public string prefab;

		public float time;
	}

	[Serializable]
	public class Anim
	{
		public string name;

		public AnimEvt[] evts;

		public WrapMode wrapMode;
	}

	public Anim[] anims;
}
