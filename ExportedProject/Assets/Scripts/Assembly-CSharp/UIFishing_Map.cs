using UnityEngine;

public class UIFishing_Map : MonoBehaviour
{
	[SerializeField]
	private Transform _posCursor;

	private void Start()
	{
	}

	private void Update()
	{
		if (COMA_PlayerSelf.Instance != null)
		{
			Vector3 position = COMA_PlayerSelf.Instance.transform.position;
			Vector2 vector = WorldPosToUIPos(new Vector2(position.x, position.z));
			_posCursor.localPosition = new Vector3(vector.x, vector.y, _posCursor.localPosition.z);
		}
	}

	protected Vector2 WorldPosToUIPos(Vector2 worldPos)
	{
		return new Vector2((0f - worldPos.y) / 75f * 24f, worldPos.x / 75f * 24f);
	}
}
