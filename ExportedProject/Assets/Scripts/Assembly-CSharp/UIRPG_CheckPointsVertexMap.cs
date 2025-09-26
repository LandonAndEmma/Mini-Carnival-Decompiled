using UnityEngine;

public class UIRPG_CheckPointsVertexMap
{
	private int _vertexIndex;

	private Vector3 _directionPos;

	public int VertexIndex
	{
		get
		{
			return _vertexIndex;
		}
	}

	public Vector3 DirectionPos
	{
		get
		{
			return _directionPos;
		}
	}

	public UIRPG_CheckPointsVertexMap(int index, Vector3 pos)
	{
		_vertexIndex = index;
		_directionPos = pos;
	}
}
