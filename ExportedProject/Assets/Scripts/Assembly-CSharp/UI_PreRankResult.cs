using UnityEngine;

public class UI_PreRankResult : MonoBehaviour
{
	[SerializeField]
	private TUILabel[] _name;

	[SerializeField]
	private TUILabel[] _weight;

	[SerializeField]
	private GameObject[] _btns;

	[SerializeField]
	private TUILabel[] _awards;

	public string[] _id;

	public void SetName(string str, int index)
	{
		_name[index].Text = str;
		if (_id[index] == string.Empty || _id[index] == COMA_Server_ID.Instance.GID)
		{
			_btns[index].SetActive(false);
		}
		else
		{
			_btns[index].SetActive(true);
		}
	}

	public void SetWeight(string str, int index)
	{
		_weight[index].Text = str;
	}

	public void SetAward(string str, int index)
	{
		_awards[index].Text = "x" + str;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
