using UnityEngine;

public class UIWaitingRoomPlayerNum : MonoBehaviour
{
	[SerializeField]
	private TUILabel _cur;

	[SerializeField]
	private TUILabel _max;

	public int CurNum
	{
		get
		{
			return int.Parse(_cur.Text);
		}
		set
		{
			_cur.Text = value.ToString();
		}
	}

	public int MaxNum
	{
		get
		{
			return int.Parse(_max.Text);
		}
		set
		{
			_max.Text = value.ToString();
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
