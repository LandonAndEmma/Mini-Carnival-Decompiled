using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class Font3D : MonoBehaviour
{
	private string fontPrePath = "FBX/Common/Font3D/";

	private float _limitWidth = -1f;

	private Mesh _mesh;

	private Material _mat;

	private int _texWidth;

	private int _texHeight;

	private int _cellWidth;

	private int _cellHeight;

	private int _cellxOffset;

	public ArrayList _widths = new ArrayList();

	private float _space;

	private float _lineSpace;

	private float _uvHeightOffset;

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

	private GameObject cameraObj;

	public float lineWidthMax;

	public static Font3D CreateFont3D(string content)
	{
		GameObject gameObject = new GameObject("Font3D");
		Font3D font3D = gameObject.AddComponent("Font3D") as Font3D;
		font3D.SetFont("Font_EnemyHurt");
		font3D.SetString(content, 0.01f, 0.35f, new Vector3(0f, 0f, 0f), BoardType.None);
		return font3D;
	}

	private void Start()
	{
		jumpXSpeed = ((Random.Range(0, 1000) % 2 != 1) ? (-0.5f) : 0.5f);
	}

	private void SetMesh()
	{
		MeshFilter meshFilter = base.gameObject.GetComponent("MeshFilter") as MeshFilter;
		_mesh = meshFilter.mesh;
	}

	public void SetFont(string fontName)
	{
		SetFont(fontPrePath, fontName);
	}

	public void SetFont(string prePath, string fontName)
	{
		SetMesh();
		_mat = Resources.Load(prePath + fontName + "_M") as Material;
		if (_mat == null)
		{
			Debug.LogError("Cannot find text_matertial : " + fontName);
		}
		MeshRenderer meshRenderer = base.gameObject.GetComponent("MeshRenderer") as MeshRenderer;
		meshRenderer.material = _mat;
		meshRenderer.material.color = _mat.color;
		TextAsset textAsset = Resources.Load(prePath + fontName + "_cfg") as TextAsset;
		if (textAsset != null && textAsset.text != null)
		{
			string[] array = textAsset.text.Split('\n');
			string[] array2 = array[0].Split(' ');
			_texWidth = int.Parse(array2[0]);
			_texHeight = int.Parse(array2[1]);
			_cellWidth = int.Parse(array2[2]);
			_cellHeight = int.Parse(array2[3]);
			_cellxOffset = int.Parse(array2[4]);
			string[] array3 = array[1].Split(' ');
			for (int i = 0; i < array3.Length; i++)
			{
				_widths.Add(float.Parse(array3[i]));
			}
		}
		else
		{
			Debug.LogError("Cannot find font text file : " + fontName);
		}
	}

	public int SetString(string text)
	{
		if (text == string.Empty)
		{
			_mesh.Clear();
			return 0;
		}
		Vector2[] array = new Vector2[text.Length];
		Vector2[] array2 = new Vector2[text.Length];
		Vector2[] array3 = new Vector2[text.Length];
		Vector2[] array4 = new Vector2[text.Length];
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		int num4 = 1;
		int num5 = _texWidth / _cellWidth;
		for (int i = 0; i < text.Length; i++)
		{
			char c = text[i];
			int num6 = c - 32;
			int num7 = num6 % num5;
			int num8 = num6 / num5;
			float num9 = num7 * _cellWidth;
			float num10 = num8 * _cellHeight;
			float num11 = (float)_widths[num6] + (float)(_cellxOffset * 2);
			float num12 = 1f / (float)_texWidth;
			float num13 = 1f / (float)_texHeight;
			array[i] = new Vector2((0f - num) * _scale, (0f - num2) * _scale);
			array2[i] = new Vector2(num11 * _scale, (float)_cellHeight * _scale);
			array3[i] = new Vector2(num9 * num12, 1f - num10 * num13);
			array4[i] = new Vector2(num11 * num12, ((float)_cellHeight + _uvHeightOffset) * num13);
			num += num11 + _space;
			if (_limitWidth > 0f && num * _scale >= _limitWidth)
			{
				if (num3 < num - _space)
				{
					num3 = num - _space;
				}
				num = 0f;
				num2 += (float)_cellHeight + _lineSpace;
				num4++;
			}
		}
		if (num4 <= 1)
		{
			num3 = num - _space;
		}
		num2 += (float)_cellHeight + _lineSpace;
		num3 *= _scale;
		num2 *= _scale;
		addMesh(array, array2, array3, array4, num3, num2);
		return num4;
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

	public int SetString(string text, float spaceSize, float lineSize, float fontHeightOffset, float cellSize, float widthLimit, BoardType curType, AlignStyle curStyle)
	{
		_scale = cellSize / (float)_cellWidth;
		_space = spaceSize / _scale;
		_lineSpace = lineSize / _scale;
		_uvHeightOffset = fontHeightOffset / _scale;
		_limitWidth = widthLimit;
		_type = curType;
		_style = curStyle;
		return SetString(text);
	}

	public void SetColor(Color clr)
	{
		base.renderer.material.color = clr;
	}

	private void addMesh(Vector2[] pos, Vector2[] size, Vector2[] uvp, Vector2[] uvs, float totalWidth, float totalHeight)
	{
		lineWidthMax = totalWidth * base.transform.lossyScale.x;
		float num = 0f;
		float num2 = 0f;
		switch (_style)
		{
		case AlignStyle.top_left:
			num = 0f;
			num2 = 0f;
			break;
		case AlignStyle.top_center:
			num = totalWidth / 2f;
			num2 = 0f;
			break;
		case AlignStyle.top_right:
			num = totalWidth;
			num2 = 0f;
			break;
		case AlignStyle.left:
			num = 0f;
			num2 = totalHeight / 2f;
			break;
		case AlignStyle.center:
			num = totalWidth / 2f;
			num2 = totalHeight / 2f;
			break;
		case AlignStyle.right:
			num = totalWidth;
			num2 = totalHeight / 2f;
			break;
		case AlignStyle.bottom_left:
			num = 0f;
			num2 = totalHeight;
			break;
		case AlignStyle.bottom_center:
			num = totalWidth / 2f;
			num2 = totalHeight;
			break;
		case AlignStyle.bottom_right:
			num = totalWidth;
			num2 = totalHeight;
			break;
		}
		int num3 = pos.Length;
		Vector3[] array = new Vector3[num3 * 4];
		Vector3[] array2 = new Vector3[num3 * 4];
		Color[] array3 = new Color[num3 * 4];
		Vector2[] array4 = new Vector2[num3 * 4];
		int[] array5 = new int[num3 * 6];
		for (int i = 0; i < num3; i++)
		{
			array[i * 4] = new Vector3(pos[i].x + num, pos[i].y + num2, 0f);
			array[i * 4 + 1] = new Vector3(pos[i].x + num, pos[i].y - size[i].y + num2, 0f);
			array[i * 4 + 2] = new Vector3(pos[i].x - size[i].x + num, pos[i].y + num2, 0f);
			array[i * 4 + 3] = new Vector3(pos[i].x - size[i].x + num, pos[i].y - size[i].y + num2, 0f);
			array4[i * 4] = new Vector2(uvp[i].x, uvp[i].y);
			array4[i * 4 + 1] = new Vector2(uvp[i].x, uvp[i].y - uvs[i].y);
			array4[i * 4 + 2] = new Vector2(uvp[i].x + uvs[i].x, uvp[i].y);
			array4[i * 4 + 3] = new Vector2(uvp[i].x + uvs[i].x, uvp[i].y - uvs[i].y);
			array3[i * 4] = new Color(1f, 0f, 0f, 1f);
			array3[i * 4 + 1] = new Color(1f, 1f, 0f, 1f);
			array3[i * 4 + 2] = new Color(1f, 1f, 0f, 1f);
			array3[i * 4 + 3] = new Color(1f, 0f, 0f, 1f);
			array2[i * 4] = new Vector3(0f, 0f, 1f);
			array2[i * 4 + 1] = new Vector3(0f, 0f, 1f);
			array2[i * 4 + 2] = new Vector3(0f, 0f, 1f);
			array2[i * 4 + 3] = new Vector3(0f, 0f, 1f);
			array5[i * 6] = i * 4;
			array5[i * 6 + 1] = i * 4 + 2;
			array5[i * 6 + 2] = i * 4 + 1;
			array5[i * 6 + 3] = i * 4 + 1;
			array5[i * 6 + 4] = i * 4 + 2;
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
		switch (_type)
		{
		case BoardType.HitNumberRaise:
		{
			raiseUpTime += Time.deltaTime;
			if (raiseUpTime > RaiseUpTime)
			{
				_mesh.Clear();
				Object.DestroyObject(base.gameObject);
				break;
			}
			Vector3 position = base.transform.position;
			position.y += raiseUpSpeed * Time.deltaTime;
			base.transform.position = position;
			Color color = base.renderer.material.color;
			color.a = 1f - 2f * raiseUpTime / RaiseUpTime;
			base.renderer.material.color = color;
			break;
		}
		case BoardType.HitNumberJump:
		{
			jumpUpTime += Time.deltaTime;
			if (jumpUpTime > JumpUpTime)
			{
				_mesh.Clear();
				Object.DestroyObject(base.gameObject);
				break;
			}
			Vector3 position2 = base.transform.position;
			position2.y += jumpUpSpeed * Time.deltaTime;
			position2.x += jumpXSpeed * Time.deltaTime;
			base.transform.position = position2;
			jumpUpSpeed -= jumpUpGravity * Time.deltaTime;
			break;
		}
		case BoardType.Name:
			if (cameraObj == null)
			{
				cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
			}
			Debug.Log(cameraObj.transform.rotation);
			base.transform.forward = -cameraObj.transform.forward;
			break;
		case BoardType.Hold:
			break;
		}
	}
}
