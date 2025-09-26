using UnityEngine;

public class GoldLine : MonoBehaviour
{
	public Transform pStr;

	public Transform ctl1;

	public Transform ctl2;

	public Transform pEnd;

	public GameObject item;

	public int count = 10;

	public Transform nodeTrs;

	private Bezier3 myBezier;

	private void OnDrawGizmos()
	{
		if (count < 5)
		{
			count = 5;
		}
		if (myBezier == null)
		{
			myBezier = new Bezier3(pStr.position, ctl1.position, pEnd.position);
		}
		if (nodeTrs == null)
		{
			return;
		}
		if (nodeTrs.childCount != count)
		{
			for (int num = nodeTrs.childCount - 1; num >= 0; num--)
			{
				Object.DestroyImmediate(nodeTrs.GetChild(num).gameObject);
			}
			for (int i = 0; i < count; i++)
			{
				GameObject gameObject = Object.Instantiate(item) as GameObject;
				gameObject.transform.parent = nodeTrs;
			}
		}
		myBezier.UpdatePoints(pStr.position, ctl1.position, pEnd.position);
		float num2 = 1f / (float)(count - 1);
		for (int j = 0; j < count; j++)
		{
			Vector3 pointAtTime = myBezier.GetPointAtTime((float)j * num2);
			nodeTrs.GetChild(j).position = pointAtTime;
		}
	}

	private void Start()
	{
	}
}
