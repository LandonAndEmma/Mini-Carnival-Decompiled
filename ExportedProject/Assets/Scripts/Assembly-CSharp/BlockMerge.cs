using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class BlockMerge : MonoBehaviour
{
	private int xLim = 2;

	private int zLim = 2;

	private int yLim = -1;

	private int[] blocks = new int[8] { 1, 0, 1, 0, 1, 1, 0, 0 };

	private Mesh _mesh;

	private void Awake()
	{
		yLim = (blocks.Length - 1) / (zLim * xLim) + 1;
		xLim = 10;
		zLim = 10;
		yLim = 10;
		string text = "{1";
		for (int i = 0; i < yLim; i++)
		{
			for (int j = 0; j < zLim; j++)
			{
				for (int k = 0; k < xLim; k++)
				{
					text += ",1";
				}
			}
		}
		text += "};";
		Debug.Log(text);
	}

	private void Start()
	{
		MeshFilter meshFilter = base.gameObject.GetComponent("MeshFilter") as MeshFilter;
		_mesh = meshFilter.mesh;
	}

	private void CalculateMesh()
	{
		_mesh.Clear();
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < blocks.Length; i++)
		{
			if (blocks[i] != 0)
			{
				int num = zLim * xLim;
				int num2 = i / num;
				int num3 = i - num2 * num;
				int num4 = num3 / xLim;
				int num5 = num3 % xLim;
				int num6 = num2 * zLim * xLim + num4 * xLim + num5;
				Vector3[] array = new Vector3[8]
				{
					new Vector3(num5, num2, num4),
					new Vector3(num5 + 1, num2, num4),
					new Vector3(num5 + 1, num2, num4 + 1),
					new Vector3(num5, num2, num4 + 1),
					new Vector3(num5, num2 + 1, num4),
					new Vector3(num5 + 1, num2 + 1, num4),
					new Vector3(num5 + 1, num2 + 1, num4 + 1),
					new Vector3(num5, num2 + 1, num4 + 1)
				};
				if (num2 <= 0 || blocks[(num2 - 1) * zLim * xLim + num4 * xLim + num5] == 0)
				{
					list.Add(array[3]);
					list.Add(array[2]);
					list.Add(array[1]);
					list.Add(array[0]);
				}
				if (num2 >= yLim - 1 || blocks[(num2 + 1) * zLim * xLim + num4 * xLim + num5] == 0)
				{
					list.Add(array[4]);
					list.Add(array[5]);
					list.Add(array[6]);
					list.Add(array[7]);
				}
				if (num4 <= 0 || blocks[num2 * zLim * xLim + (num4 - 1) * xLim + num5] == 0)
				{
					list.Add(array[0]);
					list.Add(array[1]);
					list.Add(array[5]);
					list.Add(array[4]);
				}
				if (num4 >= zLim - 1 || blocks[num2 * zLim * xLim + (num4 + 1) * xLim + num5] == 0)
				{
					list.Add(array[2]);
					list.Add(array[3]);
					list.Add(array[7]);
					list.Add(array[6]);
				}
				if (num5 <= 0 || blocks[num2 * zLim * xLim + num4 * xLim + (num5 - 1)] == 0)
				{
					list.Add(array[3]);
					list.Add(array[0]);
					list.Add(array[4]);
					list.Add(array[7]);
				}
				if (num5 >= xLim - 1 || blocks[num2 * zLim * xLim + num4 * xLim + (num5 + 1)] == 0)
				{
					list.Add(array[1]);
					list.Add(array[2]);
					list.Add(array[6]);
					list.Add(array[5]);
				}
			}
		}
		int num7 = list.Count / 4;
		List<Vector2> list2 = new List<Vector2>();
		List<int> list3 = new List<int>();
		for (int j = 0; j < num7; j++)
		{
			list2.Add(new Vector2(0f, 0f));
			list2.Add(new Vector2(1f, 0f));
			list2.Add(new Vector2(1f, 1f));
			list2.Add(new Vector2(0f, 1f));
			list3.Add(j * 4);
			list3.Add(j * 4 + 3);
			list3.Add(j * 4 + 2);
			list3.Add(j * 4 + 2);
			list3.Add(j * 4 + 1);
			list3.Add(j * 4);
		}
		Debug.Log("faces:" + num7 + " vt:" + list.Count + " uv:" + list2.Count + " tri:" + list3.Count);
		_mesh.vertices = list.ToArray();
		_mesh.uv = list2.ToArray();
		_mesh.triangles = list3.ToArray();
	}
}
