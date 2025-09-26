using UnityEngine;

public class NGUIAndroidUIRoot : MonoBehaviour
{
	private void Awake()
	{
		UIRoot[] componentsInChildren = GetComponentsInChildren<UIRoot>(true);
		UIRoot[] array = componentsInChildren;
		foreach (UIRoot uIRoot in array)
		{
			uIRoot.scalingStyle = UIRoot.Scaling.FixedSizeOnMobiles;
			float num = Mathf.Max(Screen.width, Screen.height);
			float num2 = Mathf.Min(Screen.width, Screen.height);
			if (num / num2 > 1.5f)
			{
				uIRoot.manualHeight = 640;
			}
			else
			{
				uIRoot.manualHeight = (int)(960f * num2 / num);
			}
			uIRoot.minimumHeight = 320;
			uIRoot.maximumHeight = 768;
		}
	}
}
