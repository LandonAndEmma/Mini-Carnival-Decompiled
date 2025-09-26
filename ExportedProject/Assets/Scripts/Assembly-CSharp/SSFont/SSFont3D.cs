using System.Collections;
using UnityEngine;

namespace SSFont
{
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	public class SSFont3D : MonoBehaviour
	{
		private Mesh _mesh;

		private Material _mat;

		private int _texWidth;

		private int _texHeight;

		private int _cellWidth;

		private int _cellHeight;

		private int _cellxOffset;

		private int _cellyOffset;

		public ArrayList _widths = new ArrayList();

		public ArrayList _heights = new ArrayList();

		private float _space;

		private float _scale = 0.02f;

		private BoardType _type;

		private AlignStyle _style = AlignStyle.center;

		private float raiseUpSpeed = 0.8f;

		private float raiseUpTime;

		private float RaiseUpTime = 2f;

		private float jumpUpSpeed = 4f;

		private float jumpUpTime;

		private float JumpUpTime = 1f;

		private float jumpUpGravity = 8f;

		private float jumpXSpeed;

		private void Start()
		{
			jumpXSpeed = ((Random.Range(0, 1000) % 2 != 1) ? (-0.5f) : 0.5f);
		}

		private void SetMesh()
		{
			MeshFilter meshFilter = base.gameObject.GetComponent("MeshFilter") as MeshFilter;
			_mesh = meshFilter.mesh;
		}

		public void SetFont(Material fontMat, TextAsset fontCfg)
		{
			SetMesh();
			_mat = fontMat;
			MeshRenderer meshRenderer = base.gameObject.GetComponent("MeshRenderer") as MeshRenderer;
			meshRenderer.material = _mat;
			meshRenderer.material.color = _mat.color;
			if (fontCfg != null && fontCfg.text != null)
			{
				string[] array = fontCfg.text.Split('\n');
				string[] array2 = array[0].Split(' ');
				_texWidth = int.Parse(array2[0]);
				_texHeight = int.Parse(array2[1]);
				_cellWidth = int.Parse(array2[2]);
				_cellHeight = int.Parse(array2[3]);
				_cellxOffset = int.Parse(array2[4]);
				_cellyOffset = int.Parse(array2[5]);
				string[] array3 = array[1].Split(' ');
				for (int i = 0; i < array3.Length; i++)
				{
					_widths.Add(float.Parse(array3[i]));
				}
			}
			else
			{
				Debug.LogError("3D Font Error : Cannot find font text file");
			}
		}

		public void SetString(string text)
		{
			if (text == string.Empty)
			{
				_mesh.Clear();
				return;
			}
			Vector2[] array = new Vector2[text.Length];
			Vector2[] array2 = new Vector2[text.Length];
			Vector2[] array3 = new Vector2[text.Length];
			Vector2[] array4 = new Vector2[text.Length];
			float num = 0f;
			int num2 = _texWidth / _cellWidth;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				int num3 = c - 32;
				int num4 = num3 % num2;
				int num5 = num3 / num2;
				float num6 = num4 * _cellWidth;
				float num7 = num5 * _cellHeight;
				float num8 = (float)_widths[num3] + (float)(_cellxOffset * 2);
				float num9 = num + num8 * 0.5f;
				float num10 = 1f / (float)_texWidth;
				float num11 = 1f / (float)_texHeight;
				array[i] = new Vector2(num9 * _scale, 0f);
				array2[i] = new Vector2(num8 * _scale, (float)_cellHeight * _scale);
				array3[i] = new Vector2(num6 * num10, 1f - num7 * num11);
				array4[i] = new Vector2(num8 * num10, (float)_cellHeight * num11);
				num += num8 + _space;
				if (c == '-')
				{
				}
			}
			num -= _space;
			addMesh(array, array2, array3, array4, num * 0.5f * _scale);
		}

		public void SetString(string text, float spaceSize, float cellSize, BoardType curType)
		{
			_scale = cellSize / (float)_cellWidth;
			_space = spaceSize / _scale;
			_type = curType;
			SetString(text);
		}

		public void SetString(string text, float spaceSize, float cellSize, Vector3 rotation, BoardType curType)
		{
			_scale = cellSize / (float)_cellWidth;
			_space = spaceSize / _scale;
			_type = curType;
			SetString(text);
			base.transform.localEulerAngles = rotation;
		}

		public void SetString(string text, float spaceSize, float cellSize, Vector3 rotation, BoardType curType, AlignStyle curStyle)
		{
			_scale = cellSize / (float)_cellWidth;
			_space = spaceSize / _scale;
			_type = curType;
			_style = curStyle;
			SetString(text);
			base.transform.localEulerAngles = rotation;
		}

		public void SetColor(Color clr)
		{
			Debug.Log("------clr=" + clr);
			base.renderer.material.color = clr;
		}

		private void addMesh(Vector2[] pos, Vector2[] size, Vector2[] uvp, Vector2[] uvs, float halfWidth)
		{
			if (_style == AlignStyle.center)
			{
				halfWidth = 0f - halfWidth;
			}
			else if (_style == AlignStyle.right)
			{
				halfWidth *= 2f;
			}
			int num = pos.Length;
			Vector3[] array = new Vector3[num * 4];
			Vector3[] array2 = new Vector3[num * 4];
			Color[] array3 = new Color[num * 4];
			Vector2[] array4 = new Vector2[num * 4];
			int[] array5 = new int[num * 6];
			for (int i = 0; i < num; i++)
			{
				array[i * 4] = new Vector3(pos[i].x + size[i].x * 0.5f + halfWidth, pos[i].y + size[i].y * 0.5f, 0f);
				array[i * 4 + 1] = new Vector3(pos[i].x + size[i].x * 0.5f + halfWidth, pos[i].y - size[i].y * 0.5f, 0f);
				array[i * 4 + 2] = new Vector3(pos[i].x - size[i].x * 0.5f + halfWidth, pos[i].y + size[i].y * 0.5f, 0f);
				array[i * 4 + 3] = new Vector3(pos[i].x - size[i].x * 0.5f + halfWidth, pos[i].y - size[i].y * 0.5f, 0f);
				array4[i * 4 + 2] = new Vector2(uvp[i].x, uvp[i].y);
				array4[i * 4 + 3] = new Vector2(uvp[i].x, uvp[i].y - uvs[i].y);
				array4[i * 4] = new Vector2(uvp[i].x + uvs[i].x, uvp[i].y);
				array4[i * 4 + 1] = new Vector2(uvp[i].x + uvs[i].x, uvp[i].y - uvs[i].y);
				array3[i * 4] = new Color(1f, 0f, 0f, 1f);
				array3[i * 4 + 1] = new Color(1f, 1f, 0f, 1f);
				array3[i * 4 + 2] = new Color(1f, 1f, 0f, 1f);
				array3[i * 4 + 3] = new Color(1f, 0f, 0f, 1f);
				array2[i * 4] = new Vector3(0f, 0f, 1f);
				array2[i * 4 + 1] = new Vector3(0f, 0f, 1f);
				array2[i * 4 + 2] = new Vector3(0f, 0f, 1f);
				array2[i * 4 + 3] = new Vector3(0f, 0f, 1f);
				array5[i * 6] = i * 4;
				array5[i * 6 + 1] = i * 4 + 1;
				array5[i * 6 + 2] = i * 4 + 2;
				array5[i * 6 + 3] = i * 4 + 2;
				array5[i * 6 + 4] = i * 4 + 1;
				array5[i * 6 + 5] = i * 4 + 3;
			}
			_mesh.Clear();
			_mesh.vertices = array;
			_mesh.uv = array4;
			_mesh.colors = array3;
			_mesh.normals = array2;
			_mesh.triangles = array5;
		}

		private void Update()
		{
		}
	}
}
