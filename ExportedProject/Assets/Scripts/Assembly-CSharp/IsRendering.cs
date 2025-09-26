using UnityEngine;

public class IsRendering : MonoBehaviour
{
	public OnRenderCallBack Func;

	private void OnWillRenderObject()
	{
		if (Func != null)
		{
			Func();
		}
	}
}
