using UnityEngine;

public class UIInGame_DirTag : MonoBehaviour
{
	[SerializeField]
	private TUILabel _disLabel;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void RefreshDis(int dis)
	{
		if (_disLabel != null)
		{
			_disLabel.Text = dis + "m";
		}
	}
}
