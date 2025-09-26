using UnityEngine;

public class RPGParseMaxExp : MonoBehaviour
{
	public float time_span = 1124f;

	public float fRenew = 300f;

	private void Start()
	{
		int value = Mathf.FloorToInt(time_span);
		value = Mathf.Clamp(value, 0, 5);
		Debug.Log(value);
	}

	private void Update()
	{
	}
}
