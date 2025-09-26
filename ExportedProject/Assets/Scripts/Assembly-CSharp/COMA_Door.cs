using System.Collections.Generic;
using UnityEngine;

public class COMA_Door : COMA_Target
{
	public Transform[] blockTrs;

	private int blockCount = 7;

	private float hpPerBlock;

	private Vector3[] posStr = new Vector3[7]
	{
		new Vector3(0f, 0.5f, 0f),
		new Vector3(-1f, 1.5f, 0f),
		new Vector3(0f, 1.5f, 0f),
		new Vector3(1f, 1.5f, 0f),
		new Vector3(-1f, 2.5f, 0f),
		new Vector3(0f, 2.5f, 0f),
		new Vector3(1f, 2.5f, 0f)
	};

	private Quaternion[] rotStr = new Quaternion[7]
	{
		new Quaternion(0f, 0.717f, 0f, 0.717f),
		new Quaternion(0f, 0.717f, 0f, 0.717f),
		new Quaternion(0f, 0.717f, 0f, 0.717f),
		new Quaternion(0f, 0.717f, 0f, 0.717f),
		new Quaternion(0f, 0.717f, 0f, 0.717f),
		new Quaternion(0f, 0.717f, 0f, 0.717f),
		new Quaternion(0f, 0.717f, 0f, 0.717f)
	};

	private Vector3[] posEnd = new Vector3[7];

	private Quaternion[] rotEnd = new Quaternion[7];

	private bool[] broken = new bool[7];

	private List<int> indexOfLeftBlocks = new List<int>(new int[7] { 0, 1, 2, 3, 4, 5, 6 });

	private List<int> indexOfBrokenBlocks = new List<int>();

	private float moveSpeed = 10f;

	private bool isOpening;

	private bool isClosing;

	protected override void Start()
	{
		base.Start();
		if (blockTrs.Length <= blockCount)
		{
			Debug.LogError("The Number is Wrong!!");
		}
		hpPerBlock = HP / (float)blockCount + 0.1f;
		for (int i = 0; i < blockCount; i++)
		{
			posEnd[i] = blockTrs[i].localPosition;
			rotEnd[i] = blockTrs[i].localRotation;
		}
	}

	private void Update()
	{
		if (isOpening)
		{
			for (int i = 0; i < blockTrs.Length; i++)
			{
				if ((i > 6 || !broken[i]) && blockTrs[i].localPosition.y < 3.5f)
				{
					blockTrs[i].localPosition += Vector3.up * moveSpeed * Time.deltaTime;
				}
			}
			return;
		}
		if (isClosing)
		{
			bool flag = false;
			for (int j = 0; j < blockTrs.Length; j++)
			{
				if (j > 6)
				{
					if (blockTrs[j].localPosition.y > 0.5f)
					{
						blockTrs[j].localPosition -= Vector3.up * moveSpeed * Time.deltaTime;
						flag = true;
					}
					else
					{
						blockTrs[j].localPosition = new Vector3(blockTrs[j].localPosition.x, 0.5f, blockTrs[j].localPosition.z);
					}
				}
				else if (!broken[j])
				{
					if (blockTrs[j].localPosition.y > posStr[j].y)
					{
						blockTrs[j].localPosition -= Vector3.up * moveSpeed * Time.deltaTime;
						flag = true;
					}
					else
					{
						blockTrs[j].localPosition = new Vector3(blockTrs[j].localPosition.x, posStr[j].y, blockTrs[j].localPosition.z);
					}
				}
			}
			isClosing = flag;
			if (!isClosing)
			{
				base.collider.enabled = true;
			}
			return;
		}
		for (int k = 0; k < blockCount; k++)
		{
			if (broken[k])
			{
				blockTrs[k].localPosition = Vector3.Lerp(blockTrs[k].localPosition, posEnd[k], 0.2f);
				blockTrs[k].localRotation = Quaternion.Lerp(blockTrs[k].localRotation, rotEnd[k], 0.2f);
			}
			else
			{
				blockTrs[k].localPosition = Vector3.Lerp(blockTrs[k].localPosition, posStr[k], 0.2f);
				blockTrs[k].localRotation = Quaternion.Lerp(blockTrs[k].localRotation, rotStr[k], 0.2f);
			}
		}
	}

	private void BlockBreak()
	{
		if (indexOfLeftBlocks.Count > 0)
		{
			int index = Random.Range(0, indexOfLeftBlocks.Count);
			int num = indexOfLeftBlocks[index];
			broken[num] = true;
			indexOfLeftBlocks.RemoveAt(index);
			indexOfBrokenBlocks.Add(num);
		}
	}

	private void BlockRecover()
	{
		if (indexOfBrokenBlocks.Count > 0)
		{
			int index = Random.Range(0, indexOfBrokenBlocks.Count);
			int num = indexOfBrokenBlocks[index];
			broken[num] = false;
			indexOfBrokenBlocks.RemoveAt(index);
			indexOfLeftBlocks.Add(num);
		}
	}

	private void CheckBlocks()
	{
		int num = Mathf.FloorToInt(hp / hpPerBlock) + 1;
		while (indexOfLeftBlocks.Count > num)
		{
			BlockBreak();
		}
		while (indexOfLeftBlocks.Count < num)
		{
			BlockRecover();
		}
	}

	public override void BeHit(float hitAp)
	{
		if (!(hp < 1f))
		{
			hp -= hitAp;
			if (hp < 1f)
			{
				hp = -0.1f;
				base.collider.enabled = false;
			}
			CheckBlocks();
		}
	}

	public override void BeHeal(float healPoint)
	{
		hp += healPoint;
		if (hp >= 1f)
		{
			if (hp > HP)
			{
				hp = HP;
			}
			base.collider.enabled = true;
		}
		CheckBlocks();
	}

	public float GetHPRate()
	{
		return hp / HP;
	}

	public void OpenDoor()
	{
		if (!(hp < 1f) && !isOpening)
		{
			base.collider.enabled = false;
			isOpening = true;
		}
	}

	public void CloseDoor()
	{
		isOpening = false;
		isClosing = true;
	}
}
