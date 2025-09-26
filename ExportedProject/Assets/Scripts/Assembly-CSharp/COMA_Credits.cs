using UnityEngine;

public class COMA_Credits : MonoBehaviour
{
	private static COMA_Credits _instance;

	public Transform cmrNode;

	public Transform[] pots;

	private int potIndex;

	private bool bMoving;

	private float curMovingX;

	private float lastSpeedX;

	public static COMA_Credits Instance
	{
		get
		{
			return _instance;
		}
	}

	private void OnEnable()
	{
		_instance = this;
	}

	private void OnDisable()
	{
		_instance = null;
	}

	private void Awake()
	{
		if (COMA_Platform.Instance != null)
		{
			COMA_Platform.Instance.DestroyPlatform();
		}
		if (pots.Length < 1)
		{
			Debug.LogError("must be some pots!!");
		}
	}

	private void Update()
	{
		if (bMoving)
		{
			cmrNode.position += new Vector3(curMovingX, 0f, 0f);
			curMovingX = 0f;
		}
		else if (Mathf.Abs(lastSpeedX) > 0.1f)
		{
			cmrNode.position += new Vector3(lastSpeedX, 0f, 0f);
			lastSpeedX -= Mathf.Sign(lastSpeedX) * 2f * Time.deltaTime;
		}
		if (cmrNode.position.x > pots[0].position.x)
		{
			cmrNode.position = pots[0].position;
		}
		else if (cmrNode.position.x < pots[pots.Length - 1].position.x)
		{
			cmrNode.position = pots[pots.Length - 1].position;
		}
	}

	public void Left()
	{
		potIndex--;
		if (potIndex < 0)
		{
			potIndex = pots.Length - 1;
		}
	}

	public void Right()
	{
		potIndex++;
		if (potIndex > pots.Length - 1)
		{
			potIndex = 0;
		}
	}

	public void MoveBegin(float dx, float dy)
	{
		bMoving = true;
	}

	public void Moveing(float dx, float dy)
	{
		curMovingX = dx * 0.05f;
		lastSpeedX = curMovingX;
	}

	public void MoveEnd(float dx, float dy)
	{
		bMoving = false;
	}
}
