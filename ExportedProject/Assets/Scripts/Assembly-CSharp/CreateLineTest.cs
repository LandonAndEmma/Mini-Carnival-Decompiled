using UnityEngine;

public class CreateLineTest : MonoBehaviour
{
	private class PointObject
	{
		public Mesh mesh;

		public Transform transform;
	}

	public Camera _mainCamera;

	public Material _lineMaterial;

	public float _fRadius;

	private CombineInstance[] _arrCombineInstances;

	private Mesh _lineRootMesh;

	private float _fPointSize;

	public Transform[] _arrBeginAndEndPoints;

	private PointObject[] _arrPointObjs;

	public int _nSegCount = 30;

	public bool _bReload;

	private void Init()
	{
		LineRenderer component = base.gameObject.GetComponent<LineRenderer>();
		Vector3 position = _arrBeginAndEndPoints[0].position;
		Vector3 position2 = _arrBeginAndEndPoints[1].position;
		component.SetVertexCount(12);
		int num = 0;
		component.SetPosition(num, position);
		num++;
		float num2 = Mathf.Abs(position2.x) + Mathf.Abs(position2.z);
		float y = position2.y;
		float num3 = Mathf.Abs(position.x) + Mathf.Abs(position.z) - num2;
		num3 = (position.y - y) / (num3 * num3);
		for (float num4 = 0f; num4 < 1f; num4 += 0.1f)
		{
			Vector3 position3 = Vector3.Lerp(position, position2, num4);
			float num5 = Mathf.Abs(position3.x) + Mathf.Abs(position3.z) - num2;
			position3.y = num3 * num5 * num5 + y;
			component.SetPosition(num, position3);
			num++;
		}
		Debug.Log(num);
		component.SetPosition(num, position2);
	}

	private void Start()
	{
		LineRenderer lineRenderer = base.gameObject.AddComponent<LineRenderer>();
		lineRenderer.SetWidth(0.1f, 0.1f);
		Vector3 position = _arrBeginAndEndPoints[0].position;
		Vector3 position2 = _arrBeginAndEndPoints[1].position;
		lineRenderer.SetVertexCount(12);
		int num = 0;
		lineRenderer.SetPosition(num, position);
		num++;
		Vector3 v = (position + position2) * 0.5f;
		v.y = Mathf.Min(position.y, position2.y);
		Bezier3 bezier = new Bezier3(position, v, position);
		for (float num2 = 0f; num2 < 1f; num2 += 0.1f)
		{
			Vector3 pointAtTime = bezier.GetPointAtTime(num2);
			lineRenderer.SetPosition(num, pointAtTime);
			num++;
		}
		Debug.Log(num);
		lineRenderer.SetPosition(num, position2);
		num++;
		lineRenderer.material = _lineMaterial;
	}

	private void Update()
	{
		if (_bReload)
		{
			Init();
			_bReload = false;
		}
	}

	private Mesh GeneratePointMesh()
	{
		float num = Vector3.Distance(_arrBeginAndEndPoints[0].position, _arrBeginAndEndPoints[1].position);
		Debug.Log(num);
		Vector3[] vertices = new Vector3[4]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0.1f, 0f),
			new Vector3(num, 0f, 0f),
			new Vector3(num, 0.1f, 0f)
		};
		Vector2[] uv = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		int[] triangles = new int[6] { 0, 1, 2, 2, 1, 3 };
		Color[] colors = new Color[4]
		{
			Color.white,
			Color.white,
			Color.white,
			Color.white
		};
		Mesh mesh = new Mesh();
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.colors = colors;
		return mesh;
	}
}
