using UnityEngine;

public class UISubPanelPosition : MonoBehaviour
{
	public enum ScreenDirection
	{
		horizontal = 0,
		vertical = 1
	}

	public ScreenDirection screenDirection;

	private Transform parent;

	private Transform child;

	private float ScaleSize;

	private float rateX;

	private float rateY;

	private UIPanel PanelScript;

	private void Start()
	{
		parent = base.transform.parent;
		child = base.transform.GetChild(0);
		PanelScript = base.transform.GetComponent<UIPanel>();
	}

	private void SetPanel()
	{
		base.transform.parent = null;
		child.parent = null;
		if (screenDirection == ScreenDirection.vertical)
		{
			ScaleSize = base.transform.localScale.y;
			rateX = ScaleSize / base.transform.localScale.x;
			rateY = 1f;
		}
		else if (screenDirection == ScreenDirection.horizontal)
		{
			ScaleSize = base.transform.localScale.x;
			rateX = 1f;
			rateY = ScaleSize / base.transform.localScale.y;
		}
		base.transform.localScale = new Vector4(ScaleSize, ScaleSize, ScaleSize, ScaleSize);
		base.transform.parent = parent;
		child.parent = base.transform;
		PanelScript.clipRange = new Vector4(PanelScript.clipRange.x, PanelScript.clipRange.y, PanelScript.clipRange.z * rateX, PanelScript.clipRange.w * rateY);
	}

	private void Update()
	{
	}
}
