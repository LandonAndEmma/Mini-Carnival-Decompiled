using UnityEngine;

public class COMA_Loading : MonoBehaviour
{
	private static string targetLevel = string.Empty;

	private int tmr;

	public static void LoadLevel(string tar)
	{
		targetLevel = tar;
		Debug.Log("Loading...     " + targetLevel);
		Application.LoadLevelAdditive("COMA_Loading");
	}

	private void Update()
	{
		if (tmr != 4)
		{
			if (tmr == 8)
			{
				Resources.UnloadUnusedAssets();
			}
			else if (tmr == 16)
			{
				if (targetLevel == string.Empty)
				{
					return;
				}
				Application.LoadLevel(targetLevel);
				targetLevel = string.Empty;
			}
		}
		tmr++;
	}
}
