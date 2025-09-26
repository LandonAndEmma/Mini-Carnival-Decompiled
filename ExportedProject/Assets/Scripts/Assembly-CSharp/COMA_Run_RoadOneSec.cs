using UnityEngine;

public class COMA_Run_RoadOneSec : MonoBehaviour
{
	[SerializeField]
	public GameObject[] _diffLevels1;

	[SerializeField]
	public GameObject[] _diffLevels2;

	[SerializeField]
	public GameObject[] _diffLevels3;

	[SerializeField]
	public Transform[] _NextPos;

	public void InitSecRoadDiff(int nDiff, int nIndex)
	{
		switch (nDiff)
		{
		case 0:
		{
			for (int l = 0; l < _diffLevels1.Length; l++)
			{
				if (l == nIndex)
				{
					_diffLevels1[l].SetActive(true);
				}
				else
				{
					_diffLevels1[l].SetActive(false);
				}
			}
			for (int m = 0; m < _diffLevels2.Length; m++)
			{
				_diffLevels2[m].SetActive(false);
			}
			for (int n = 0; n < _diffLevels3.Length; n++)
			{
				_diffLevels3[n].SetActive(false);
			}
			break;
		}
		case 1:
		{
			for (int num = 0; num < _diffLevels2.Length; num++)
			{
				if (num == nIndex)
				{
					_diffLevels2[num].SetActive(true);
				}
				else
				{
					_diffLevels2[num].SetActive(false);
				}
			}
			for (int num2 = 0; num2 < _diffLevels1.Length; num2++)
			{
				_diffLevels1[num2].SetActive(false);
			}
			for (int num3 = 0; num3 < _diffLevels3.Length; num3++)
			{
				_diffLevels3[num3].SetActive(false);
			}
			break;
		}
		case 2:
		{
			for (int i = 0; i < _diffLevels3.Length; i++)
			{
				if (i == nIndex)
				{
					_diffLevels3[i].SetActive(true);
				}
				else
				{
					_diffLevels3[i].SetActive(false);
				}
			}
			for (int j = 0; j < _diffLevels1.Length; j++)
			{
				_diffLevels1[j].SetActive(false);
			}
			for (int k = 0; k < _diffLevels2.Length; k++)
			{
				_diffLevels2[k].SetActive(false);
			}
			break;
		}
		}
	}

	public void InitSecRoadNext(int nIndex)
	{
		for (int i = 0; i < _NextPos.Length; i++)
		{
			if (i == nIndex)
			{
				_NextPos[i].gameObject.SetActive(true);
			}
			else
			{
				_NextPos[i].gameObject.SetActive(false);
			}
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void TickOptimize()
	{
		switch (CheckOptimize())
		{
		case 0:
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
			}
			break;
		case 1:
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
			}
			break;
		}
	}

	private int CheckOptimize()
	{
		if (COMA_PlayerSelf.Instance == null)
		{
			return -1;
		}
		float z = COMA_PlayerSelf.Instance.transform.position.z;
		float num = 150f;
		float num2 = 50f;
		float z2 = base.transform.position.z;
		float z3 = _NextPos[0].position.z;
		if (z2 - z > num || z - z3 > num2)
		{
			return 1;
		}
		return 0;
	}
}
