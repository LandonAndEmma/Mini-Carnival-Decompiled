using UnityEngine;

public class UIFishing_CurFishPoleInfo : MonoBehaviour
{
	[SerializeField]
	private GameObject[] _fishPole;

	[SerializeField]
	private TUILabel _numLabel;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetCurFishPole(int n)
	{
		for (int i = 0; i < _fishPole.Length; i++)
		{
			if (i == n)
			{
				_fishPole[i].SetActive(true);
			}
			else
			{
				_fishPole[i].SetActive(false);
			}
		}
	}

	public void SetCurFishPoleNum(int num)
	{
		_numLabel.Text = num.ToString();
	}

	public int GetCurFishPoleNum()
	{
		return int.Parse(_numLabel.Text);
	}
}
