using System.Collections;
using UnityEngine;

public class COMA_PackageIconInit : MonoBehaviour
{
	private IEnumerator Start()
	{
		for (int i = 0; i < COMA_Pref.Instance.package.pack.Length; i++)
		{
			if (COMA_Pref.Instance.package.pack[i] != null)
			{
				COMA_Pref.Instance.package.pack[i].CreateIconTexture();
				yield return 0;
			}
		}
	}
}
