using UnityEngine;

public class Bezier3
{
	public Vector3 p0;

	public Vector3 p1;

	public Vector3 p2;

	public float ti;

	private Vector3 b0 = Vector3.zero;

	private Vector3 b1 = Vector3.zero;

	private Vector3 b2 = Vector3.zero;

	private float Ax;

	private float Ay;

	private float Az;

	private float Bx;

	private float By;

	private float Bz;

	private float Cx;

	private float Cy;

	private float Cz;

	public Bezier3(Vector3 v0, Vector3 v1, Vector3 v2)
	{
		UpdatePoints(v0, v1, v2);
	}

	public void UpdatePoints(Vector3 v0, Vector3 v1, Vector3 v2)
	{
		p0 = v0;
		p1 = v1;
		p2 = v2;
	}

	public Vector3 GetPointAtTime(float t)
	{
		CheckConstant();
		float num = t * t;
		float x = Ax * num + Bx * t + Cx;
		float y = Ay * num + By * t + Cy;
		float z = Az * num + Bz * t + Cz;
		return new Vector3(x, y, z);
	}

	private void CheckConstant()
	{
		if (p0 != b0 || p1 != b1 || p2 != b2)
		{
			SetConstant();
			b0 = p0;
			b1 = p1;
			b2 = p2;
		}
	}

	private void SetConstant()
	{
		Cx = p0.x;
		Bx = 2f * (p1.x - p0.x);
		Ax = p2.x - 2f * p1.x + p0.x;
		Cy = p0.y;
		By = 2f * (p1.y - p0.y);
		Ay = p2.y - 2f * p1.y + p0.y;
		Cz = p0.z;
		Bz = 2f * (p1.z - p0.z);
		Az = p2.z - 2f * p1.z + p0.z;
	}
}
