using System;
using System.Collections.Generic;
using UnityEngine;

public class COMA_Tank_AimAssist : MonoBehaviour
{
	public float _fNodeDelta = 0.1f;

	public float _fGravity = 10f;

	public float _fInitialSpeed = 10f;

	private List<Transform> _nodes = new List<Transform>();

	public float _fMovingSpeed = 0.005f;

	private float _fOffset;

	private Transform getNode(int nIndex)
	{
		if (_nodes.Count < nIndex + 1)
		{
			for (int i = _nodes.Count; i < nIndex + 1; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("FBX/Scene/Tank/Prefab/AnimNode")) as GameObject;
				gameObject.name = "node" + i;
				_nodes.Add(gameObject.transform);
				gameObject.transform.parent = base.transform;
			}
		}
		return _nodes[nIndex];
	}

	private void Start()
	{
	}

	private void Update()
	{
		float num = 0f;
		int num2 = 0;
		_fOffset += _fMovingSpeed;
		if (Mathf.Abs(_fOffset - _fNodeDelta) < _fMovingSpeed)
		{
			_fOffset = 0f;
		}
		do
		{
			float num3 = (float)num2 * _fNodeDelta;
			Vector3 nodePosWorld = getNodePosWorld(num3 + _fOffset);
			Transform node = getNode(num2);
			node.position = base.transform.position + nodePosWorld;
			node.rotation = Quaternion.identity;
			num = getNode(num2).position.y;
			num2++;
		}
		while (num2 < 15);
	}

	private Vector3 getNodePosWorld(float fForwardStep)
	{
		float num = (0f - Mathf.Sin(base.transform.rotation.eulerAngles.x * (float)Math.PI / 180f)) * _fInitialSpeed;
		float y = num * fForwardStep - 0.5f * _fGravity * fForwardStep * fForwardStep;
		float num2 = Mathf.Abs(Mathf.Cos(base.transform.rotation.eulerAngles.x * (float)Math.PI / 180f) * _fInitialSpeed);
		return new Vector3(base.transform.forward.x * fForwardStep * num2, y, base.transform.forward.z * fForwardStep * num2);
	}
}
